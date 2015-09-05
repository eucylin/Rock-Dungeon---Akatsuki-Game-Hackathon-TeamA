using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Grid))]
public class GridEditor : Editor{
	public override void OnInspectorGUI(){
		Grid grid = (Grid)target;

		DrawDefaultInspector();

		if(GUILayout.Button("Create Grid")){
			grid.CreateGrid();
		}

		EditorGUILayout.HelpBox("This will create grid without deleting previous grids.", MessageType.Info);
	}
}
