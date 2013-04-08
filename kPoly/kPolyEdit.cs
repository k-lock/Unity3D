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
	
	#endregion
	#region Editor
	/** The Unity EditorWindow start function.*/
	[MenuItem("Window/klock/kMesh/kPolyEdit %M3")]
	public static void Init ()
	{
		instance = (kPolyEdit)EditorWindow.GetWindow (typeof(kPolyEdit), false, "Poly Edit");
		instance.Show ();
		instance.OnEnable ();
		instance.position = new Rect (200, 100, 200, 228);
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

	/*private void OnDisable ()
	{
	
	}
	
	private void Update ()
	{
	
	}*/

	private void OnGUI ()
	{	
		DrawPanel ();
	}

	private void OnSelectionChange ()
	{
		//if (_editorMode != MODE.None) {
		_selection = Selection.activeGameObject;

		if (_selection != null) {
			_selectMeshFilter = _selection.GetComponent<MeshFilter> ();
			if (_selectMeshFilter != null) {
				_selectMesh = _selectMeshFilter.sharedMesh;
			}
			neigbourList = kPoly.Neigbours (_selectMesh);
		} else {
			
			//	neigbourList = null;
		}
		/*}else{
			Selection.activeGameObject = _selection;	
		}*/
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
		int controlID = GUIUtility.GetControlID (_instanceHash, FocusType.Native);	
		Event e = Event.current;
		switch (e.GetTypeForControl (controlID)) {
		case EventType.mouseUp:
		case EventType.mouseMove:
		case EventType.repaint:
			if (_selectMesh != null) {
				if (_editorMode != MODE.None) {
					Draw_Handles ();
				}
			}
			break;
		case EventType.layout:
			
			break;
				
		}
	}

	private static int[] _SelectionIndicies = null;

	private static void Draw_Handles ()
	{
		int _hitTriangle = kPoly.HitTriangle ();
		if (_hitTriangle == -1) {
			return;
		}
		if (_SelectionIndicies == null) {
			_SelectionIndicies = new int[99];
			_SelectionIndicies [0] = 0;
		}
		
		//	Selection.activeGameObject = _selection;
		
		Transform root = _selection.transform;
		int[] tList = _selectMesh.triangles;
		int tIndex = _hitTriangle * 3;
		
		foreach (int index in _SelectionIndicies) {
			
			if (tList == null || tIndex > tList.Length)
				return;
		
			int t1 = tList [index + 0];
			float cubeSize = HandleUtility.GetHandleSize (_selectMesh.vertices [t1]) * .1f;
			Vector3 v1 = root.TransformPoint (_selectMesh.vertices [t1]);
			/*if (Event.current.type == EventType.mouseUp)
				Handles.color = new Color (Color.red.r, Color.red.g, Color.red.b, .85f);
			else*/
			Handles.color = new Color (Color.green.r, Color.green.g, Color.green.b, .85f);
			//Handles.CubeCap (0, v1, root.rotation, cubeSize);
			Vector3 cv = Handles.PositionHandle (v1, root.rotation);
			if (Event.current.type == EventType.mouseDrag) {
				
				/*.FreeMoveHandle (v1, 
                            Quaternion.identity,
                            .5f,
                            Vector3.zero, 
                            Handles.CubeCap);*/
			
				_selectMesh.vertices [t1] = _selection.transform.InverseTransformPoint (cv);
			}
		}
	}
	#endregion
}
