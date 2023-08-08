using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;


namespace DialogueGraph.Editor.Views
{
    internal sealed class DialogueGraphEditorWindow : EditorWindow
    {
        private const string DefaultWindowTitle = "Dialogue Graph";

        /// <summary>
        /// Creates a GraphView for the Dialogue Graph window
        /// </summary>
        private void CreateGraphView()
        {
            // Creates a new dialogue system graph view element
            DialogueGraphView graphView = new DialogueGraphView(this);
            // Stretches the graph view to the parnets size
            graphView.StretchToParentSize();
            // Adds the visual element to the Editor Window
            rootVisualElement.Add(graphView);
        }

        /// <summary>
        /// 
        /// </summary>
        private void CreateToolbar()
        {
            /*
            Toolbar tb = new Toolbar();
            TextField filenameTextField = DSHelper.CreateTextField(DefaultFilename, "Filename:");
            _saveButton = DSHelper.CreateButton("Save");
            tb.Add(filenameTextField);
            tb.Add(_saveButton);
            tb.AddStyleSheets("DialogueSystemStyles/DSToolBarStyle.uss");
            rootVisualElement.Add(tb);
            */
        }

        private void OnEnable()
        {
            CreateGraphView();
            CreateToolbar();
        }

        /// <summary>
        /// This function is called whenever a new Dialogue Graph window is opened
        /// </summary>
        [MenuItem("Window/Dialogue Graph")]
        public static void OpenWindow()
        {
            DialogueGraphEditorWindow wnd = GetWindow<DialogueGraphEditorWindow>();
            wnd.titleContent = new GUIContent(DefaultWindowTitle);
        }
    }
}
