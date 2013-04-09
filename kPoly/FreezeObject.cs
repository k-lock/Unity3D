using UnityEngine;
using UnityEditor;
using System.Collections;

public class FreezeObject : EditorWindow
{
	public 	static	FreezeObject 	instance;
	private 		int 			_sIndex = -1;
	private			Transform		_sTrans = null;
	private			bool			_freeze = false;
	
	[MenuItem("Window/klock/kMesh/FreezEditor")]
	public static void Init ()
	{
		instance = (FreezeObject)EditorWindow.GetWindow (typeof(FreezeObject), false, "FreezEditor");
		instance.Show ();
		instance.OnEnable ();
		instance.position = new Rect (200, 100, 200, 250);
		instance.maxSize = new Vector2 (210, 260);
		instance.minSize = new Vector2 (190, 240);
	}
	
	void OnEnable ()
	{
		if (instance == null)
			instance = this;
	}
	
	void OnDisable()
	{
		instance = null;
	}
	
	void OnSelectionChange ()
	{
		if (!_freeze && Selection.activeInstanceID > 0) {
		
			_sTrans = Selection.activeTransform;
			_sIndex = Selection.activeInstanceID;
		
		}
		
		if ( _freeze && Selection.activeInstanceID != _sIndex) {
	
			Selection.activeTransform = _sTrans;
			Selection.activeInstanceID = _sIndex;
			Selection.activeGameObject = _sTrans.gameObject;
			
		}
	}

	void OnInspectorUpdate ()
	{
		if ( _freeze && Selection.activeInstanceID != _sIndex) OnSelectionChange ();
	}

	void OnGUI ()
	{
		DrawPanel ();
	}
	
	void DrawPanel ()
	{
		EditorGUILayout.BeginVertical (new GUIStyle { contentOffset = new Vector2 (-10, 0) });
		EditorGUILayout.Space ();
		
		// current selection idendifier
		EditorGUILayout.ObjectField ("Selection ", _sTrans, typeof(Transform), true);
		EditorGUILayout.Space ();
		
		// toggle freeze mode
		GUI.color = (_freeze) ? Color.yellow : Color.white;
		if (GUILayout.Button ("Freeze")) {
			_freeze = !_freeze;
		}		
		GUI.color = Color.white;
		
		EditorGUILayout.EndVertical ();
	}
}
