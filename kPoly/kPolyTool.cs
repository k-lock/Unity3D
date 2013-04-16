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
	public static kPolyTool instance;
	private static PANEL _PANEL = PANEL.CREATE;
	private SceneView.OnSceneFunc _onSceneGUI_ = null;

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
		instance.minSize = new Vector2 (100, 200);
		instance.maxSize = new Vector2 (500, 600);

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

	private void OnSelectionChange ()
	{
		Debug.Log ("TOOL - OnSelectionChange");
		//    if (_PANEL == PANEL.CREATE || _PANEL == PANEL.PREFS) return;

		/*GameObject _selection = Selection.activeGameObject;

        if (_selection != null)
        {*/
		switch (_PANEL) {
		case PANEL.EDIT:
                //CLEAR_EDITOR("Edit");
			if (pEdit._freeze)
				return;
			pEdit.OnSelectionChange ();
			pEdit.Repaint ();
			Repaint ();
			break;
		case PANEL.INFO:
                //CLEAR_EDITOR("Info");
			pInfo.OnSelectionChange ();
			pInfo.Repaint ();
			Repaint ();
			break;
		}
		/* }else{

         }*/
		//Repaint();
	}

	public void OnSceneGUI (SceneView sceneView)
	{

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

	private static kPolyCreate pCreate = null;
	private static kPolyEdit pEdit = null;
	private static kPolyInfo pInfo = null;

	private void InstanciesInit ()
	{

		if (pCreate == null) {
			pCreate = kPolyCreate.Create ();
		}
		if (pEdit == null) {
			pEdit = kPolyEdit.Create ();
		}
		if (pInfo == null) {
			pInfo = kPolyInfo.Create ();
			pInfo.OnSelectionChange ();
		}
		if (_PANEL != PANEL.CREATE || _PANEL != PANEL.PREFS) {
			/* if (_onSceneGUI_ == null)
             {
                 _onSceneGUI_ = new SceneView.OnSceneFunc(OnSceneGUI);
                 SceneView.onSceneGUIDelegate += _onSceneGUI_;
             }*/
		}
	}

	private void DrawActMenu ()
	{
		switch (_PANEL) {
		case PANEL.CREATE:
			pCreate.DrawPanel ();
			break;
		case PANEL.EDIT:
			pEdit.DrawPanel ();
			pEdit.OnEnable ();
			break;
		case PANEL.INFO:
			pInfo.DrawPanel ();
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
	public static EditorWindow[] FIND_ALL (string name)
	{
		
		switch (name) {
		case "Tool":
			return (kPolyTool[])(Resources.FindObjectsOfTypeAll (typeof(kPolyTool)));
			break;
		case "Create":
			return (kPolyCreate[])(Resources.FindObjectsOfTypeAll (typeof(kPolyCreate)));
			break;
		case "Edit":
			return (kPolyEdit[])(Resources.FindObjectsOfTypeAll (typeof(kPolyEdit)));
			break;
		case "Info":
			return (kPolyInfo[])(Resources.FindObjectsOfTypeAll (typeof(kPolyInfo)));
			break;
		}
			
		return null;
	}

	public static void CLEAR_EDITOR (string name)
	{
		EditorWindow[] windows = (EditorWindow[])(Resources.FindObjectsOfTypeAll (typeof(EditorWindow)));
		for (int i = 0; i < windows.Length; i++) {
			if (windows [i].title == name) {
				if (name == "Edit") {
					kPolyEdit f = windows [i] as kPolyEdit;
					//f.Close();
					ScriptableObject.DestroyImmediate (f);
				}
				if (name == "Create") {
					kPolyCreate t = windows [i] as kPolyCreate;
					ScriptableObject.DestroyImmediate (t);
				}
				if (name == "Info") {
					kPolyInfo d = windows [i] as kPolyInfo;
					ScriptableObject.DestroyImmediate (d);
				}
			}
		}
	}
}

public enum PANEL
{
	CREATE,
	EDIT,
	INFO,
	PREFS
}