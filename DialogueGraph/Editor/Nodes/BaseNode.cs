using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

namespace DialogueGraph.Editor.Nodes
{
    internal abstract class BaseNode : Node
    {
        protected string _id;
        protected Vector2 _defaultNodeSize = new Vector2(200, 250);

        public string ID { get => _id; set => _id = value; }

        public BaseNode()
        {

        }

        //public abstract void Draw();
    }
}
