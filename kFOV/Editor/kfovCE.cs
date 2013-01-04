using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects, CustomEditor(typeof(kFOV))]
public class kfovCE : Editor
{
	
	private static GUIContent
		emptyContent = GUIContent.none;
	private static GUILayoutOption
		elementWidth = GUILayout.MaxWidth (300f),
		titleWidth = GUILayout.MaxWidth (80f),
		toggleWidth = GUILayout.MaxWidth (20f);
	private SerializedObject 	_kFOV;
	private SerializedProperty 	
		_width, 
		_length, 
		_dpi, 
		_offset,
		_color1,
		_color2;
	
	void OnEnable ()
	{
		_kFOV = new SerializedObject (targets);
		
		_width = _kFOV.FindProperty ("width");
		_length = _kFOV.FindProperty ("length");
		_dpi = _kFOV.FindProperty ("dpi");
		_offset = _kFOV.FindProperty ("offset");
		_color1 = _kFOV.FindProperty ("color1");
		_color2 = _kFOV.FindProperty ("color2");
	}
	
	public override void OnInspectorGUI ()
	{
		_kFOV.Update ();	

		EditorGUI.BeginChangeCheck ();

		EditorGUILayout.BeginVertical ();

		GUILayout.Label ("Size", EditorStyles.boldLabel, titleWidth);
		EditorGUILayout.PropertyField (_width, elementWidth);
		EditorGUILayout.PropertyField (_length, elementWidth);

		GUILayout.Label ("Collision", EditorStyles.boldLabel, titleWidth);
		EditorGUILayout.PropertyField (_dpi, elementWidth);
		EditorGUILayout.PropertyField (_offset, elementWidth);
		
		GUILayout.Label ("Colors", EditorStyles.boldLabel, titleWidth);
		EditorGUILayout.PropertyField (_color1, elementWidth);
		EditorGUILayout.PropertyField (_color2, elementWidth);
		
		EditorGUILayout.EndVertical ();		
	
		if (EditorGUI.EndChangeCheck ()) {
			if (_kFOV.ApplyModifiedProperties () || 
				(Event.current.type == EventType.ValidateCommand && Event.current.commandName == "UndoRedoPerformed")) {
				foreach (kFOV k in targets) {
					if (PrefabUtility.GetPrefabType (k) != PrefabType.Prefab) {
						k.UpdateTile ();
					}
				}
			}
		}
	}

	void OnSceneGUI ()
	{

	}
}
