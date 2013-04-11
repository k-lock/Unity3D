/** http://www.k-lock.de  | Paul Knab 
 * 	_______________________________________
 * 	
 * 	kPolyTool | V.1.0.0 | 11.04.2013
 *  ________________________________________
 * 
 * 	Editor Window kPoly Utility
 * 
 * */
using UnityEngine;
using UnityEditor;

public class kPolyTool : EditorWindow
{
    #region vars
	/** Static instance to this editor class. */
	public 	static	kPolyTool 	instance;
	private static	PANEL		_PANEL = PANEL.CREATE;
	
	#endregion
	#region Editor
	/** The Unity EditorWindow start function.*/
	[MenuItem("Window/klock/kMesh/kPolyTools %M")]
	public static kPolyTool Init ()
	{
		instance = (kPolyTool)EditorWindow.GetWindow (typeof(kPolyTool), false, "Tools");
		instance.Show ();
		instance.OnEnable ();
		instance.position = new Rect (200, 100, 250, 400);
		
		return instance;
	}
    #endregion
	#region Unity
	private void OnEnable ()
	{
		if (instance == null) {
			instance = this;	
		}
		InstanciesInit ();
		Repaint ();
	}

	private void OnDisable ()
	{
		instance = null;
	}

	private void OnGUI ()
	{	
		DrawPanel ();
		
	}
	#endregion
	#region Editor GUI
	/** Main GUI draw function.*/
	private void DrawPanel ()
	{
		int controlID = GUIUtility.GetControlID (instance.GetHashCode (), FocusType.Passive);	
		Event e = Event.current;
		switch (e.GetTypeForControl (controlID)) {
		case EventType.mouseDown:
		case EventType.mouseUp:
		case EventType.mouseDrag:
		case EventType.keyUp:
		case EventType.keyDown:
		case EventType.repaint:
		case EventType.layout:
		case EventType.ExecuteCommand:
		case EventType.ValidateCommand:
		
			DrawPanelSelector ();
			DrawActMenu ();
			
			break;
		}		
	}
	
	private static kPolyCreate 	pCreate = null;
	private static kPolyEdit	pEdit	= null;
	
	void InstanciesInit ()
	{
		
		if (pCreate == null) {
			pCreate = kPolyCreate.Create ();
		}
		if (pEdit == null) {
			pEdit = kPolyEdit.Create ();
		}
	}
	
	private void DrawActMenu ()
	{
		switch (_PANEL) {
		case PANEL.CREATE:
			pCreate.DrawPanel ();
			break;
		case PANEL.EDIT:
			pEdit.DrawPanel();
			break;
		case PANEL.INFO:
			break;
		case PANEL.PREFS:
			break;
		}
		
	}
	
	private void DrawPanelSelector ()
	{
		EditorGUI.BeginChangeCheck ();
		EditorGUILayout.BeginVertical ();//new GUIStyle { contentOffset = new Vector2 (0, 0) });
		GUILayout.BeginHorizontal ();
		
		GUI.color = (_PANEL == PANEL.CREATE) ? Color.grey : Color.white;
		if (GUILayout.Button (new GUIContent ("Create"))) {
			_PANEL = PANEL.CREATE;
		}
		GUI.color = Color.white;
		GUI.color = (_PANEL == PANEL.EDIT) ? Color.grey : Color.white;
		if (GUILayout.Button (new GUIContent ("Edit"))) {
			_PANEL = PANEL.EDIT;
		}
		GUI.color = Color.white;
		GUI.color = (_PANEL == PANEL.INFO) ? Color.grey : Color.white;
		if (GUILayout.Button (new GUIContent ("Info"))) {
			_PANEL = PANEL.INFO;
		}
		GUI.color = Color.white;
		GUI.color = (_PANEL == PANEL.PREFS) ? Color.grey : Color.white;
		if (GUILayout.Button (new GUIContent ("Prefs"))) {
			_PANEL = PANEL.PREFS;
		}
		GUI.color = Color.white;
		GUILayout.EndHorizontal ();
		EditorGUILayout.EndVertical ();
		if (EditorGUI.EndChangeCheck ()) {
			Repaint ();
		}
	}
	
	
	#endregion
	
	
}

public enum PANEL
{
	CREATE,
	EDIT,
	INFO,
	PREFS
}