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

	public override void OnInspectorGUI ()
	{
		TileAnim t = target as TileAnim;
		DrawDefaultInspector ();

		if (GUI.changed) {
			
			t._width = Mathf.Clamp (t._width, 0, 999);
			t._height = Mathf.Clamp (t._height, 0, 999);
			t._currentFrame = Mathf.Clamp (t._currentFrame, 0, t._lastFrame);
			t._lastFrame = Mathf.Clamp (t._lastFrame, 0, t._frameRects.Length - 1);
			t._frameTick = Mathf.Clamp (t._frameTick, 0, 20);

			t.MESH_refresh ();
		}
	}
}