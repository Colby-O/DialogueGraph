using DialogueGraph.Editor.Nodes;
using DialogueGraph.Enumeration;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DialogueGraph.Editor.Views
{
    internal sealed class NodeSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private DialogueGraphView _graphView;

        public void Initialize(DialogueGraphView view)
        {
            _graphView = view;
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> entries = new List<SearchTreeEntry>()
            {
                new SearchTreeGroupEntry(new GUIContent("Create Element")),
                new SearchTreeGroupEntry(new GUIContent("Create Node"), 1),
                new SearchTreeEntry(new GUIContent("Single Choice"))
                {
                    level = 2,
                    userData = DialogueType.SingleChoice,
                },
                new SearchTreeEntry(new GUIContent("Multiple Choice"))
                {
                    level = 2,
                    userData = DialogueType.MultipleChoice,
                },
                new SearchTreeEntry(new GUIContent("Set Boolean"))
                {
                    level = 2,
                    userData = DialogueType.SetBoolean,
                },
                new SearchTreeEntry(new GUIContent("Branch"))
                {
                    level = 2,
                    userData = DialogueType.Branch,
                },
                new SearchTreeEntry(new GUIContent("SetInt"))
                {
                    level = 2,
                    userData = DialogueType.SetInt,
                },
                new SearchTreeEntry(new GUIContent("Increment"))
                {
                    level = 2,
                    userData = DialogueType.Increment,
                },
                new SearchTreeEntry(new GUIContent("Comparator"))
                {
                    level = 2,
                    userData = DialogueType.Comparator,
                },
                new SearchTreeEntry(new GUIContent("Connector"))
                {
                    level = 2,
                    userData = DialogueType.Connector,
                },
                new SearchTreeEntry(new GUIContent("Emit Event"))
                {
                    level = 2,
                    userData = DialogueType.EmitEvent,
                },
                new SearchTreeGroupEntry(new GUIContent("Create Group"), 1),
                new SearchTreeEntry(new GUIContent("Single Group"))
                {
                    level = 2,
                    userData = new Group(),
                },
            };

            return entries;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            Vector2 mousePosition = _graphView.GetLocalMousePosition(context.screenMousePosition, true);

            switch (SearchTreeEntry.userData)
            {
                case DialogueType.SingleChoice:
                    BaseNode singlecChoiceNode = _graphView.CreateNode(DialogueType.SingleChoice, mousePosition);
                    _graphView.AddElement(singlecChoiceNode);
                    return true;
                case DialogueType.MultipleChoice:
                    BaseNode multipleChoiceNode = _graphView.CreateNode(DialogueType.MultipleChoice, mousePosition);
                    _graphView.AddElement(multipleChoiceNode);
                    return true;
                case DialogueType.SetBoolean:
                    BaseNode setBooleanNode = _graphView.CreateNode(DialogueType.SetBoolean, mousePosition);
                    _graphView.AddElement(setBooleanNode);
                    return true;
                case DialogueType.Branch:
                    BaseNode branchNode = _graphView.CreateNode(DialogueType.Branch, mousePosition);
                    _graphView.AddElement(branchNode);
                    return true;
                case DialogueType.Increment:
                    BaseNode incrementNode = _graphView.CreateNode(DialogueType.Increment, mousePosition);
                    _graphView.AddElement(incrementNode);
                    return true;
                case DialogueType.Comparator:
                    BaseNode comparatorNode = _graphView.CreateNode(DialogueType.Comparator, mousePosition);
                    _graphView.AddElement(comparatorNode);
                    return true;
                case DialogueType.Connector:
                    BaseNode connectorNode = _graphView.CreateNode(DialogueType.Connector, mousePosition);
                    _graphView.AddElement(connectorNode);
                    return true;
                case DialogueType.SetInt:
                    BaseNode setIntNode = _graphView.CreateNode(DialogueType.SetInt, mousePosition);
                    _graphView.AddElement(setIntNode);
                    return true;
                case DialogueType.EmitEvent:
                    BaseNode emitEventNode = _graphView.CreateNode(DialogueType.EmitEvent, mousePosition);
                    _graphView.AddElement(emitEventNode);
                    return true;
                case Group _:
                    GraphElement group = _graphView.CreateGroup("New Group", true);
                    _graphView.AddElement(group);
                    return true;
            }

            return false;
        }
    }
}
