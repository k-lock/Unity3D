/* kTileEditor V.0.1 - 2012 - Paul Knab */
using UnityEngine;
using UnityEditor;
using System.Collections;

//namespace klock.kTiles.editors
//{
   
[CanEditMultipleObjects, CustomEditor(typeof(kTile))]
    public class kTileEditor : Editor
{
	
	public override void OnInspectorGUI ()
	{
		kTile t = target as kTile;

		DrawDefaultInspector ();

		if (GUI.changed) {
			t.MESH_refresh ();
		}
	}
}
//}