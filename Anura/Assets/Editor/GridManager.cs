using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridPoint))]
public class GridManager : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GridPoint point = (GridPoint)target;

        //point.prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", point.prefab, typeof(GameObject), true);

        if (GUILayout.Button("Forward"))
        {
            point.CreatePoint(0);
        }
        if (GUILayout.Button("Left"))
        {
            point.CreatePoint(1);
        }
        if (GUILayout.Button("Back"))
        {
            point.CreatePoint(2);
        }
        if (GUILayout.Button("Right"))
        {
            point.CreatePoint(3);
        }
    }
}
