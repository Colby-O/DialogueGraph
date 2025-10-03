using DialogueGraph.Attribute;
using DialogueGraph.Data;
using DialogueGraph.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

namespace DialogueGraph
{
    public abstract class DialogueController : MonoBehaviour
    {
        [Header("Database")]
        [SerializeField] protected DialogueGraphDatabase _db;

        protected Dictionary<string, DialogueExposedPropertiesData> _exposedVariables = new Dictionary<string, DialogueExposedPropertiesData>();
        protected Dictionary<string, UnityEvent<int>> _events = new Dictionary<string, UnityEvent<int>>();
        protected Dictionary<string, DialogueGraphSO> _dialogueGrapths;

        [Header("Debugging")]
        [SerializeField, ReadOnly] protected DialogueGraphSO _currentDialogue;
        [SerializeField, ReadOnly] protected DialogueNodeData _currentDialogueNode;
        [SerializeField, ReadOnly] protected DialogueType _currentNodeType;
        [SerializeField, ReadOnly] protected bool _isDialogueInProgress;
        private System.Action _completelyFinishedCallback;

        public bool IsPlayingDialogue()
        {
            return _isDialogueInProgress;
        }

        public void AddListener(string eventTag, UnityAction<int> callback)
        {
            if (!_events.ContainsKey(eventTag)) _events.Add(eventTag, new UnityEvent<int>());
            _events[eventTag].AddListener(callback);
        }

        public bool GetFlag(string name)
        {
            if (_exposedVariables.ContainsKey(name))
            {
                if (_exposedVariables[name].Type == "Bool") return _exposedVariables[name].BooleanValue;
            }
            else if (_currentDialogue != null && _currentDialogue.Properties.Select(e => e.Name).Contains(name))
            {
                DialogueExposedPropertiesData property = _currentDialogue.Properties.First(e => e.Name == name);
                if (property.Type == "Bool")  return property.BooleanValue;
            }

            return false;
        }

        public void SetFlag(string name, bool value)
        {
            if (_exposedVariables.ContainsKey(name))
            {
                if (_exposedVariables[name].Type == "Bool") _exposedVariables[name].BooleanValue = value;
            }
            else
            {
                DialogueExposedPropertiesData property = new DialogueExposedPropertiesData();
                property.Name = name;
                property.BooleanValue = value;
                property.Type = "Bool";
                _exposedVariables.Add(name, property);
            }
        }

        public int? GetInt(string name)
        {
            if (_exposedVariables.ContainsKey(name))
            {
                if (_exposedVariables[name].Type == "Int") return _exposedVariables[name].IntValue;
            }
            else if (_currentDialogue != null && _currentDialogue.Properties.Select(e => e.Name).Contains(name))
            {
                DialogueExposedPropertiesData property = _currentDialogue.Properties.First(e => e.Name == name);
                if (property.Type == "Int") return property.IntValue;
            }

            return null;
        }

        public void SetInt(string name, int value)
        {
            if (_exposedVariables.ContainsKey(name))
            {
                if (_exposedVariables[name].Type == "Int")_exposedVariables[name].IntValue = value;
            }
            else
            {
                DialogueExposedPropertiesData property = new DialogueExposedPropertiesData();
                property.Name = name;
                property.IntValue = value;
                property.Type = "Int";
                _exposedVariables.Add(name, property);
            }
        }


        public virtual void StartDialogue(string dialogueName, System.Action finishCallback = null)
        {
            if (_dialogueGrapths.ContainsKey(dialogueName))
            {
                _completelyFinishedCallback = finishCallback;
                _currentDialogue = _dialogueGrapths[dialogueName];
                _isDialogueInProgress = true;
                _currentDialogueNode = _currentDialogue.Nodes.Find(n => n.Type == Enumeration.DialogueType.Start);
                _currentNodeType = _currentDialogueNode.Type;
                ProcessCurrentNode();
            }
            else
            {
                Debug.LogWarning($"'{dialogueName}' is an invaild Dialogue Graph name. Cannot start dialogue.");
            }
        }

        public void FinishDialogue()
        {
            _completelyFinishedCallback?.Invoke();
            _completelyFinishedCallback = null;
            _currentDialogue = null;
            _currentDialogueNode = null;
            _isDialogueInProgress = false;
            OnDialogueFinished();
        }

        protected void InitializeFlag()
        {
            if (_currentDialogueNode != null && _currentDialogueNode.PropertyType == "Bool")
            {
                string flagName = _currentDialogueNode.PropertyName;
                bool flag = _currentDialogueNode.PropertyBooleanValue;
                if (flagName != string.Empty && flagName != null)
                {
                    if (_exposedVariables.ContainsKey(flagName)) _exposedVariables[flagName].BooleanValue = flag;
                    else 
                    {
                        DialogueExposedPropertiesData property = new DialogueExposedPropertiesData();
                        property.Name = flagName;
                        property.BooleanValue = flag;
                        property.Type = "Bool";
                        _exposedVariables.Add(flagName, property);
                    }
                }
            }
        }

        protected string GetCurrentFlag()
        {
            string flag = _currentDialogueNode.PropertyName;
            return flag;
        }

        protected void NextNode(int choice)
        {
            if (_currentDialogueNode.Choices.Count <= choice || _currentDialogueNode.Choices[choice].Next == string.Empty || _currentDialogueNode.Choices[choice].Next == null)
            {
                FinishDialogue();
                return;
            }
            _currentDialogueNode = _currentDialogue.Nodes.FirstOrDefault<DialogueNodeData>(n => _currentDialogueNode.Choices[choice].Next == n.Guid);
            _currentNodeType = _currentDialogueNode.Type;
            ProcessCurrentNode();
        }

        private void IncrementVariable()
        {
            if (_currentDialogueNode != null && _currentDialogueNode.PropertyType == "Int")
            {
                string flagName = _currentDialogueNode.PropertyName;
                int value = GetInt(flagName) ?? 0;
                int incrementAmount = _currentDialogueNode.IncrementAmount;
                if (!string.IsNullOrEmpty(flagName))
                {
                    if (_exposedVariables.ContainsKey(flagName)) _exposedVariables[flagName].IntValue = value + incrementAmount;
                    else
                    {
                        DialogueExposedPropertiesData property = new DialogueExposedPropertiesData();
                        property.Name = flagName;
                        property.IntValue = value + incrementAmount;
                        property.Type = "Int";
                        _exposedVariables.Add(flagName, property);
                    }
                }
            }
        }

        private void SetInt()
        {
            if (_currentDialogueNode != null && _currentDialogueNode.PropertyType == "Int")
            {
                string flagName = _currentDialogueNode.PropertyName;
                int value = _currentDialogueNode.PropertyIntValue;
                if (!string.IsNullOrEmpty(flagName))
                {
                    if (_exposedVariables.ContainsKey(flagName)) _exposedVariables[flagName].IntValue = value;
                    else
                    {
                        DialogueExposedPropertiesData property = new DialogueExposedPropertiesData();
                        property.Name = flagName;
                        property.IntValue = value;
                        property.Type = "Int";
                        _exposedVariables.Add(flagName, property);
                    }
                }
            }
        }

        private int HandleComparator()
        {
            string flagName = _currentDialogueNode.PropertyName;
            int? hasValue = GetInt(flagName);
            if (hasValue == null) return 0;
            int value = hasValue.Value;
            ComparisonType comparisonType = _currentDialogueNode.ComparisonType;
            int condition = _currentDialogueNode.ComparsionCondition;

            switch (comparisonType)
            {
                case ComparisonType.Equals:
                    return (value == condition) ? 1 : 0;
                case ComparisonType.GreaterThan:
                    return (value > condition) ? 1 : 0;
                case ComparisonType.LessThan:
                    return (value < condition) ? 1 : 0;
            }

            return 0;
        } 

        private void EmitEvent(string eventName, int choice = -1)
        {
            if (!string.IsNullOrEmpty(eventName) && _events.TryGetValue(eventName, out UnityEvent<int> callback))
            {
                callback?.Invoke(choice);
            }
        }

        protected void ProcessCurrentNode()
        {
            switch (_currentNodeType)
            {
                case DialogueType.Start:
                case DialogueType.Connector:
                    NextNode(0);
                    break;
                case DialogueType.SingleChoice:
                case DialogueType.MultipleChoice:
                    PlayDialogueNode();
                    break;
                case DialogueType.SetBoolean:
                    InitializeFlag();
                    NextNode(0);
                    break;
                case DialogueType.Branch:
                    string flag = GetCurrentFlag();
                    NextNode(GetFlag(flag) ? 1 : 0);
                    break;
                case DialogueType.Increment:
                    IncrementVariable();
                    NextNode(0);
                    break;
                case DialogueType.Comparator:
                    NextNode(HandleComparator());
                    break;
                case DialogueType.SetInt:
                    SetInt();
                    NextNode(0);
                    break;
                case DialogueType.EmitEvent:
                    EmitEvent(_currentDialogueNode.EventID);
                    NextNode(0);
                    break;
            }
        }

        protected void InitializeDialogues()
        {
            _dialogueGrapths = new Dictionary<string, DialogueGraphSO>();
            List<DialogueGraphSO> sos = _db.GetAllEntries();
            foreach (DialogueGraphSO so in sos)
            {
                _dialogueGrapths.Add(so.Name, so);
            }
        }

        public void OnDialogueNodeFinish(int choice)
        {
            if (_currentDialogueNode == null)
            {
                return;
            }
            EmitEvent(_currentDialogueNode.EventID, choice);
            NextNode(choice);
        }

        protected abstract void PlayDialogueNode();
        public abstract void OnDialogueFinished();

        protected virtual void Awake()
        {
            InitializeDialogues();
        }
    }
}
