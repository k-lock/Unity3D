using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class kTileEditorMonitor :EditorWindow
{
    
	private static kTileEditorMonitor instance;
	
	[MenuItem("Window/klock/Editor Monitor")]
	public static void Init ()
	{

		if (instance != null) {
			instance.Show ();
			return;
		} else {
			instance = (kTileEditorMonitor)EditorWindow.GetWindow (typeof(kTileEditorMonitor), false, "Tile Monitor");
			instance.wantsMouseMove = true;
			instance.Show ();
			instance.position = new Rect (800, 50, 400, 50);
		}
	}

	private void OnInspectorUpdate ()
	{
		Repaint ();
	}

	private void OnGUI ()
	{
		DRAW ();
	}

	private void DRAW ()
	{
		
		EditorGUILayout.BeginVertical();

        GUILayout.BeginHorizontal();
		GUILayout.Label("kPolyTool : "+ kPolyTool.FIND_ALL_OF("Tool").Length);
        GUILayout.Label("ANY KEY " + kPolyEdit.ANY_KEY);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
		GUILayout.Label("kPolyCreate : "+ kPolyTool.FIND_ALL_OF("Create").Length);
        GameObject go = Selection.activeGameObject;
        GUILayout.Label("flag : " +(go!=null? go.hideFlags.ToString() : "NONE" ));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
   		GUILayout.Label("kPolyEdit : "+ kPolyTool.FIND_ALL_OF("Edit").Length);
        GUILayout.Label("Active GO : " + (go != null ? go.name : "NONE"));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
		GUILayout.Label("kPolyInfo : "+ kPolyTool.FIND_ALL_OF("Info").Length);
        GUILayout.Label("Active ID : " + Selection.activeInstanceID );
        GUILayout.EndHorizontal();
		GUILayout.Label("hotControl  ->  " +GUIUtility.hotControl );
 		GUILayout.Label("keyboardControl  ->  " +GUIUtility.keyboardControl  );
		
		EditorGUILayout.EndVertical();
		Repaint ();
	}
}
