using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using DialogueGraph.Editor.Nodes;

namespace DialogueGraph.Editor.Views
{
    using Utilities;

    internal sealed class DialogueGraphView : GraphView
    {
        private DialogueGraphEditorWindow _editorWindow;

        /// <summary>
        /// DialogueGraphView's constructor
        /// </summary>
        public DialogueGraphView(DialogueGraphEditorWindow editorWindow)
        {
            _editorWindow = editorWindow;
            CreateGrid();
            AddManipulator();
            AddStyleSheets();
        }

        private IManipulator CreateNodeMenu(string title)
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction(title, e => AddElement(CreateNode(GetLocalMousePosition(e.eventInfo.localMousePosition))))
            );

            return contextualMenuManipulator;
        }

        /// <summary>
        /// Add all Manipulator's to the Graph View
        /// </summary>
        private void AddManipulator()
        {
            // Allow for draging movement by clicking middle mouse button
            this.AddManipulator(new ContentDragger());
            // Allows items selected to be dragged together
            this.AddManipulator(new SelectionDragger());
            // Allow items to be selected by holding left click and draging
            this.AddManipulator(new RectangleSelector());
            // Allow the screen to be moved by holding down the Alt key
            this.AddManipulator(new FreehandSelector());
            // Handle's the zoom behaviour
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        }

        /// <summary>
        /// Create a grid to display over the background
        /// </summary>
        private void CreateGrid()
        {
            // Create a new grid object
            GridBackground gridBackground = new GridBackground();

            // Stertches the grid to the parnet element size
            gridBackground.StretchToParentSize();

            // Inserts the grid to the first slot in the visual element content list
            Insert(0, gridBackground);
        }

        /// <summary>
        /// Adds stylesheets tp the editor element
        /// </summary>
        private void AddStyleSheets()
        {
            this.AddStyleSheets(
                "GraphViewStyles",
                "NodeStyle"
            );
        }

        public Vector2 GetLocalMousePosition(Vector2 pos, bool isSearchWindow = false)
        {
            if (isSearchWindow)
            {
                // The first .positon is a React that is why there is two .positions
                pos -= _editorWindow.position.position;
            }

            Vector2 localMousePos = contentViewContainer.WorldToLocal(pos);

            return localMousePos;
        }

        public BaseNode CreateNode(Vector2 pos)
        {
            return null;
        }
    }
}
