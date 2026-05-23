using Pathfinding;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(NavigationArea))]
    public class NavigationAreaEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            var navArea = (NavigationArea) target;

            if (GUILayout.Button("Generate"))
                navArea.GenerateNodes();
        }
    }
}