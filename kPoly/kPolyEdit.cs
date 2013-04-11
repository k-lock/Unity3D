/** http://www.k-lock.de  | Paul Knab 
 * 	_______________________________________
 * 	
 * 	kPolyEdit | V.1.0.0 | 07.04.2013
 *  ________________________________________
 * 
 * 	Editor Window for editing the current
 * 	selected gamobject mesh component.  
 * 	 
 */
using UnityEngine;
using UnityEditor;
using System.Collections;

public class kPolyEdit : EditorWindow
{
    #region vars
	/** Static instance to this editor class. */
	public 	static kPolyEdit 	instance;
	private static int[] 		_SelectionIndicies = null;
	/** Static gui id to this editor window. */
	private static int 			_instanceHash = -1;
	/**The Selected gameobject from the scene view.*/
	private static GameObject 	_selection = null;
	/**The meshfilter componente from the current selected gameobject.*/
	private static MeshFilter 	_selectMeshFilter = null;
	/**The mesh/shared mesh componente from the current selected gameobject.*/
	private static Mesh 		_selectMesh = null;
	/** GUI Delegate method to update the sceneview from the editor window.*/
	private static SceneView.OnSceneFunc _onSceneGUI_ = null;
	private static TriPoint[] 	neigbourList = null;
	//private static bool _SHOW_VERTS = true;
	private static bool 		_SHOW_TRIAS = true;
	private static bool 		_SHOW_NEIBS = false;
	private static MODE			_editorMode = MODE.None;
	private 		int 			_sIndex = -1;
	private			Transform		_sTrans = null;
	private			GameObject		_sObjec = null;
	private			bool			_freeze = false;
	
	#endregion
	#region Editor
	/** The Unity EditorWindow start function.*/
	[MenuItem("Window/klock/kMesh/kPolyEdit %M3")]
	public static void Init ()
	{
		/*System.Type[]	ts = new System.Type[3]{typeof(kPolyCreate),typeof(kPolyEdit),typeof(kPolyInfo)};
		kPolyCreate pm = null;
		if(FindObjectsOfType( typeof(kPolyCreate) ).Length == 0 ){
			pm = kPolyCreate.Init();
		}else{
			pm = kPolyCreate.instance;
		}

		instance = (kPolyEdit)EditorWindow.GetWindow<kPolyEdit>("Edit", ts);
		 */
		instance = (kPolyEdit)EditorWindow.GetWindow (typeof(kPolyEdit), false, "Edit");
		instance.Show ();
		instance.OnEnable ();
		instance.position = new Rect (200, 100, 200, 228);
		instance.minSize = new Vector2 (190, 200);
		instance.maxSize = new Vector2 (250, 250);
	}
	#endregion
	#region Unity
	private void OnEnable ()
	{
		if (instance == null) {
			instance = this;	
		}
		_instanceHash = instance.GetHashCode ();
		if (_onSceneGUI_ == null) {
			_onSceneGUI_ = new SceneView.OnSceneFunc (OnSceneGUI);
			SceneView.onSceneGUIDelegate += _onSceneGUI_;
		}
	}

	private void OnDisable ()
	{
		_selection = null;
		_selectMesh = null;
		verts = null;
	}
	
	private void Update ()
	{
		//if (_freeze && Selection.activeInstanceID != _sIndex)
		//	OnSelectionChange ();
	}

	void OnInspectorUpdate ()
	{
		if (_freeze && Selection.activeInstanceID != _sIndex)
			OnSelectionChange ();
	}

	private void OnGUI ()
	{	
		DrawPanel ();
		Repaint ();
	}

	private void OnSelectionChange ()
	{

		if (!_freeze && Selection.activeInstanceID != _sIndex) {
			ResetSelection ();
			GetSelection ();		
		}
		
		if (_freeze && Selection.activeInstanceID != _sIndex) {
			SetSelection ();			
		}

		if (_selection != null) {
			_selectMeshFilter = _selection.GetComponent<MeshFilter> ();
			if (_selectMeshFilter != null) {
				_selectMesh = _selectMeshFilter.sharedMesh;
			}
			neigbourList = kPoly.Neigbours (_selectMesh);	
		} 

		Repaint ();
	}
	#endregion
	#region Editor GUI
	/** Main GUI draw function.*/
	private void DrawPanel ()
	{
		EditorGUILayout.BeginVertical (new GUIStyle { contentOffset = new Vector2 (-10, 0) });
		EditorGUILayout.Space ();
		
		// current selection idendifier
		GUI.enabled = (_selection != null);
		EditorGUILayout.ObjectField ("Selection ", _selection, typeof(GameObject), true);
		EditorGUILayout.Space ();
		
		GUILayout.BeginHorizontal ();
		GUI.color = (_editorMode == MODE.E_Point) ? new Color (0, .5f, 1, .7f) : Color.white;
		if (GUILayout.Button (new GUIContent ("Point")))
			_editorMode = (_editorMode == MODE.E_Point) ? MODE.None : MODE.E_Point;
		GUI.color = Color.white;
		GUI.color = (_editorMode == MODE.E_Line) ? new Color (0, .5f, 1, .7f) : Color.white;
		if (GUILayout.Button (new GUIContent ("Line")))
			_editorMode = (_editorMode == MODE.E_Line) ? MODE.None : MODE.E_Line;
		GUI.color = Color.white;
		GUI.color = (_editorMode == MODE.E_Quad) ? new Color (0, .5f, 1, .7f) : Color.white;
		if (GUILayout.Button (new GUIContent ("Segment")))
			_editorMode = (_editorMode == MODE.E_Quad) ? MODE.None : MODE.E_Quad;
		GUI.color = Color.white;
		GUI.color = (_editorMode == MODE.E_All) ? new Color (0, .5f, 1, .7f) : Color.white;
		if (GUILayout.Button (new GUIContent ("Complete")))
			_editorMode = (_editorMode == MODE.E_All) ? MODE.None : MODE.E_All;
		GUI.color = Color.white;
		GUILayout.EndHorizontal ();
		EditorGUILayout.Space ();
		GUILayout.Label( ""+EDITOR_activeSelection );
		_freeze = (_editorMode != MODE.None);
		
		EditorGUILayout.EndVertical ();		
	}
	#endregion
	#region Scene GUI
	/*[DrawGizmo(GizmoType.NotSelected)]
	public static void RenderCustomGizmo (Transform objectTransform, GizmoType gizmoType)
	{
		if (instance == null)
			return;
 		
	}*/

	public static void OnSceneGUI (SceneView sceneview)
	{ 
		/*int controlID = GUIUtility.GetControlID (_instanceHash, FocusType.Passive);	
		Event e = Event.current;
		switch (e.GetTypeForControl (controlID)) {
		case EventType.mouseDown:
		case EventType.mouseUp:
		case EventType.mouseMove:
		case EventType.repaint:
		case EventType.layout:*/
		if (_selectMesh != null) {
			if (_editorMode != MODE.None) {
				if (verts == null && _selectMesh != null) verts = _selectMesh.vertices;
				switch (_editorMode) {
				case MODE.E_Point:	
					Draw_Handles2 ();
					break;
				}
				_selectMesh.vertices = verts;
				_selectMesh.RecalculateNormals ();
				_selectMesh.RecalculateBounds ();
				//_selectMeshFilter.sharedMesh = _selectMesh;	
			}
		}
		//	break;
		
		/*if (Selection.activeObject != _selection ||
				_selection == null && _editorMode != MODE.None) {
				
				Selection.objects = sList;
				Selection.activeGameObject = sList[0];
			}*/
			 
		//break;
				
		//	}
	}

	static int curPointIndex = 0;
	static Vector3 dragPoint = Vector3.zero;
	static Vector3[] verts;

	private static void Draw_Handles2 ()
	{
		if (_selectMesh == null || _selection == null || verts == null) {
			return;
		}

		int someHashCode = instance.GetHashCode ();		
		Transform root = _selection.transform;
		//int i = 0;
		
		bool refreshMesh = false;
		
		for (int i =0; i<verts.Length; i++) {// (Vector3 mv in verts) {

			float cubeSize = HandleUtility.GetHandleSize (verts [i]);
			Vector3 v1 = root.TransformPoint (verts [i]);
			
			int controlIDBeforeHandle = GUIUtility.GetControlID (someHashCode, FocusType.Passive);
			bool isEventUsedBeforeHandle = (Event.current.type == EventType.used);

			if (curPointIndex == i)
				Handles.color = new Color (Color.red.r, Color.red.g, Color.red.b, .85f);
			else
				Handles.color = new Color (Color.green.r, Color.green.g, Color.green.b, .85f);

			Handles.ScaleValueHandle (0, v1, Quaternion.identity, cubeSize, Handles.CubeCap, 0);
			if (curPointIndex == i) {
				
				v1 = Handles.PositionHandle (v1, Quaternion.identity);
				verts [i] = root.InverseTransformPoint (v1);
				//Debug.Log("Moving "+ mv + " " + verts[i]);
				///refreshMesh = true;
				
			}

			int controlIDAfterHandle = GUIUtility.GetControlID (someHashCode, FocusType.Native);
			bool isEventUsedByHandle = !isEventUsedBeforeHandle && (Event.current.type == EventType.used);
 
			if
             ((controlIDBeforeHandle < GUIUtility.hotControl && 
				GUIUtility.hotControl < controlIDAfterHandle) || isEventUsedByHandle) {
				curPointIndex = i;
				EDITOR_activeSelection = "Point "+i;
			}

		}		
	}

	private static string EDITOR_activeSelection = "";
	/*
	private static void Draw_Handles ()
	{
		int _hitTriangle = kPoly.HitTriangle ();
		if (_hitTriangle == -1 || _selection == null) {
			return;
		}
		if (_SelectionIndicies == null) {
			_SelectionIndicies = new int[99];
			_SelectionIndicies [0] = 0;
		}

		Transform root = _selection.transform;
		int[] tList = _selectMesh.triangles;
		int tIndex = _hitTriangle * 3;
		
		foreach (int index in _SelectionIndicies) {
			
			if (tList == null || tIndex > tList.Length)
				return;
		
			int t1 = tList [index + 0];
			float cubeSize = HandleUtility.GetHandleSize (_selectMesh.vertices [t1]) * .1f;
			Vector3 v1 = root.TransformPoint (_selectMesh.vertices [t1]);
			if (Event.current.type == EventType.mouseUp)
				Handles.color = new Color (Color.red.r, Color.red.g, Color.red.b, .85f);
			else
				Handles.color = new Color (Color.green.r, Color.green.g, Color.green.b, .85f);
			Handles.CubeCap (0, v1, root.rotation, cubeSize);
			//Vector3 cv = Handles.PositionHandle (v1, root.rotation);
			
		}
	}*/
	#endregion
	#region SELECTION
	void SetSelection ()
	{
		Selection.activeTransform = _sTrans;
		Selection.activeInstanceID = _sIndex;
		Selection.activeGameObject = _selection = _sObjec;
	}

	void GetSelection ()
	{
		_sTrans = Selection.activeTransform;
		_sIndex = Selection.activeInstanceID;
		_selection = _sObjec = Selection.activeGameObject;
	}
	
	void ResetSelection ()
	{
		_selection = null;
		_selectMesh = null;
		verts = null;
		_sTrans = null;
		_sIndex = -1;
		_selection = _sObjec = null;
	}
	#endregion
}
