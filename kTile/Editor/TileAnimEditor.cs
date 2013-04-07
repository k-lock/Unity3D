/* 

kTile Anim Custom Editor V.0.1 - 2013 - Paul Knab 
____________________________________

Description : Editor GUI Layout for TileBase.cs
		
*/
using UnityEngine;
using UnityEditor;
using System.Collections;

[System.Serializable]
[CanEditMultipleObjects, CustomEditor(typeof(TileAnim))]
public class TileAnimEditor : Editor
{

    public override void OnInspectorGUI()
    {
        TileAnim t = target as TileAnim;
        DrawDefaultInspector();

        if (GUI.changed)
        {
            t.MESH_refresh();
        }
    }
}