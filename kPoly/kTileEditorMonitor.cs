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
		
		GUILayout.Label("kPolyTool : "+ kPolyTool.FIND_ALL("Tool").Length);
		GUILayout.Label("kPolyCreate : "+ kPolyTool.FIND_ALL("Create").Length);
   		GUILayout.Label("kPolyEdit : "+ kPolyTool.FIND_ALL("Edit").Length);
		GUILayout.Label("kPolyInfo : "+ kPolyTool.FIND_ALL("Info").Length);
		
		
		GUILayout.Label("hotControl  ->  " +GUIUtility.hotControl );
 		GUILayout.Label("keyboardControl  ->  " +GUIUtility.keyboardControl  );
		
		EditorGUILayout.EndVertical();
		Repaint ();
	}
}
