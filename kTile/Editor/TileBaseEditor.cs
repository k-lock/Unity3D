/* 

kTile Base Custom EditorV.0.1 - 2013 - Paul Knab 
____________________________________

Description : Editor GUI Layout for TileBase.cs
		
*/
using UnityEngine;
using UnityEditor;
using System.Collections;

[System.Serializable]
[CanEditMultipleObjects, CustomEditor(typeof(TileBase))]
public class TileBaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TileBase t = target as TileBase;
        DrawDefaultInspector();

        if (GUI.changed)
        {
            t.MESH_refresh();
        }
    }
}