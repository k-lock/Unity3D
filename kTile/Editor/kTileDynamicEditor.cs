/* kTileDynamic V.0.1 - 2012 - Paul Knab */
using UnityEngine;
using UnityEditor;
using System.Collections;

//namespace klock.kTiles.editors
//{

[CanEditMultipleObjects, CustomEditor(typeof(kTileDynamic))]
    public class kTileDynamicEditor : Editor
{

	public override void OnInspectorGUI ()
	{
		kTileDynamic t = target as kTileDynamic;
		
		DrawDefaultInspector ();

		if (GUI.changed) {
			
			t._width = Mathf.Clamp( t._width ,0 , 999 );
			t._height = Mathf.Clamp( t._height ,0 , 999 );
			t._currentFrame = Mathf.Clamp( t._currentFrame, 0, t._lastFrame );
			t._lastFrame = Mathf.Clamp( t._lastFrame, 0, t._frameRects.Length-1 );
			t._frameTick = Mathf.Clamp( t._frameTick, 0, 20 );
			
			t.MESH_refresh ();
		}
	}
}
//}