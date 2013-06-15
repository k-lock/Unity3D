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
        TileBase.FACING faceTemp = t.facing;
        DrawDefaultInspector();
    
        if (GUI.changed)
        {
            t._width = Mathf.Clamp(t._width, 0, 999);
            t._height = Mathf.Clamp(t._height, 0, 999);

            if (faceTemp != t.facing) t.transform.localEulerAngles = new Vector3(0,0,0);

            t.MESH_refresh();
        }
    }

}