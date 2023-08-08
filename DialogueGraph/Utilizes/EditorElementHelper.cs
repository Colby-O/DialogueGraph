using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueGraph.Utilities
{
    public static class EditorElementHelper
    {
        public static TextField CreateTextField(string val = null, string label = null, bool isReadOnly = false, EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            TextField textField = new TextField();
            textField.value = val;
            textField.label = label;
            textField.isReadOnly = isReadOnly;

            if (onValueChanged != null) textField.RegisterValueChangedCallback(onValueChanged);

            return textField;
        }

        public static TextField CreateTextArea(string val = null, string label = null, EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            TextField textArea = CreateTextField(val, label, false, onValueChanged);
            textArea.multiline = true;
            return textArea;
        }

        public static Foldout CreateFoldout(string title, bool collapsed = false)
        {
            Foldout foldout = new Foldout();
            foldout.text = title;
            foldout.value = !collapsed;
            return foldout;
        }

        public static Button CreateButton(string text, Action onCLick = null)
        {
            Button button = new Button(onCLick);
            button.text = text;
            return button;
        }

        public static Port CreatePort(this Node node, string portName = "", Orientation orientation = Orientation.Horizontal, Direction direction = Direction.Output, Port.Capacity capacity = Port.Capacity.Single)
        {
            Port port = node.InstantiatePort(orientation, direction, capacity, typeof(bool));
            port.portName = portName;
            return port;
        }
    }
}
