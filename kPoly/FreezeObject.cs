using UnityEngine;
using UnityEditor;
using System.Collections;

public class FreezeObject : EditorWindow
{
	public 	static	FreezeObject 	instance;
	private 		int 			_sIndex = -1;
	private			Transform		_sTrans = null;
	private			GameObject		_sObjec = null;
	private			bool			_freeze = false;
	
	[MenuItem("Window/klock/FreezEditor")]
	public static void Init ()
	{
		instance = (FreezeObject)EditorWindow.GetWindow (typeof(FreezeObject), false, "FreezEditor");
		
		instance.Show ();
		instance.OnEnable ();
		
		instance.position = new Rect (200, 100, 200, 30);
		instance.maxSize = new Vector2 (210, 30);
		instance.minSize = new Vector2 (190, 30);
	}
	
	void OnEnable ()
	{
		if (instance == null)
			instance = this;
		
		OnSelectionChange ();
	}
	
	void OnDisable ()
	{
		instance = null;
	}
	
	void OnSelectionChange ()
	{
		if (!_freeze && Selection.activeInstanceID > -1) {
			ResetSelection ();
			GetSelection ();		
		}
		
		if (_freeze && Selection.activeInstanceID != _sIndex) {
			SetSelection ();			
		}
	}
	
	void OnInspectorUpdate ()
	{
		if (_freeze && Selection.activeInstanceID != _sIndex)
			OnSelectionChange ();
	}

	void OnGUI ()
	{
		DrawPanel ();
	}
	
	void DrawPanel ()
	{
		EditorGUILayout.BeginVertical (new GUIStyle { contentOffset = new Vector2 (-10, 0) });
		EditorGUILayout.Space ();

		// toggle freeze mode
		GUI.color = (_freeze) ? Color.yellow : Color.white;
		if (GUILayout.Button ("Freeze ")) {
			ResetSelection ();	
			
			_freeze = !_freeze;
			
			if (_freeze)
				GetSelection ();
		}		
		GUI.color = Color.white;
		
		EditorGUILayout.EndVertical ();
	}

	void SetSelection ()
	{
		Selection.activeTransform = _sTrans;
		Selection.activeInstanceID = _sIndex;
		Selection.activeGameObject = _sObjec;
	}

	void GetSelection ()
	{
		_sTrans = Selection.activeTransform;
		_sIndex = Selection.activeInstanceID;
		_sObjec = Selection.activeGameObject;
	}
	
	void ResetSelection ()
	{
		_sTrans = null;
		_sIndex = -1;
		_sObjec = null;
	}
}
