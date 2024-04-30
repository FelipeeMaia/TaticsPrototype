using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Tactics.Grids
{
    [CustomEditor(typeof(GridCreator))]
    public class GridCreatorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GridCreator gridCreator = (GridCreator)target;
            if(GUILayout.Button("Spawn Grid"))
            {
                gridCreator.CreateGrid();
            }
        }
    }
}