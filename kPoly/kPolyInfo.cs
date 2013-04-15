/** http://www.k-lock.de  | Paul Knab 
 * 	_______________________________________
 * 	
 * 	kPolyInfo | V.1.0.0 | 07.04.2013
 *  ________________________________________
 * 
 * 	Editor Window  getting info about the 
 * 	mesh component for the current selected 
 * 	gamobject. 
 */
using UnityEngine;
using UnityEditor;
using System.Collections;

public class kPolyInfo: EditorWindow
{
    #region vars
	/** Static instance to this editor class. */
	public 	static kPolyInfo 	instance;
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
	private static bool 		_LIST_HANDS = true;
	private static bool 		_LIST_VERTS = false;
	private static bool 		_LIST_TRIAS = false;
	private static bool 		_LIST_NEIBS = false;
	private 		Vector2 	scp1 = Vector2.zero;
	private 		Vector2 	scp2 = Vector2.zero;
	private 		Vector2 	scp3 = Vector2.zero;
	private static TriPoint[] 	neigbourList = null;
	//private static bool _SHOW_VERTS = true;
	private static bool 		_SHOW_TRIAS = true;
	private static bool 		_SHOW_NEIBS = false;
	
	#endregion
	#region Editor
	/** The Unity EditorWindow start function.*/
	[MenuItem("Window/klock/kMesh/kPolyInfo %M2")]
	public static void Init ()
	{
		instance = (kPolyInfo)EditorWindow.GetWindow (typeof(kPolyInfo), false, "Poly Info");
		instance.Show ();
		instance.OnEnable ();
		instance.position = new Rect (200, 100, 200, 228);
        instance.minSize = new Vector2(190, 200);
        instance.maxSize = new Vector2(250, 250);
	}
    public static kPolyInfo Create()
    {
        return CreateInstance<kPolyInfo>();
    }
	#endregion
	#region Unity
	private void OnEnable ()
	{
		if (instance == null) {
			instance = this;	
		}
		_instanceHash = instance.GetHashCode ();
		/*if (_onSceneGUI_ == null) {
			_onSceneGUI_ = new SceneView.OnSceneFunc (OnSceneGUI);
			SceneView.onSceneGUIDelegate += _onSceneGUI_;
		}*/
	}

	/*private void OnDisable ()
	{
	
	}
	
	private void Update ()
	{
	
	}*/

	private void OnGUI ()
	{	
		DrawPanel2 ();
	}

	public void OnSelectionChange ()
	{
        Debug.Log("INFO - OnSelectionChange" + _selection);
		_selection = Selection.activeGameObject;

		if (_selection != null) {
			_selectMeshFilter = _selection.GetComponent<MeshFilter> ();
			if (_selectMeshFilter != null) {
				_selectMesh = _selectMeshFilter.sharedMesh;
			}
			Calc_Neigbours ();
		} else {
			
			_LIST_VERTS = _LIST_TRIAS = false;
            
		}
		Repaint ();
	}

	public static void OnSceneGUI (SceneView sceneview)
	{ 
		int controlID = GUIUtility.GetControlID (instance.GetHashCode(), FocusType.Native);	
		Event e = Event.current;
		switch (e.GetTypeForControl (controlID)) {
		case EventType.mouseMove:
		case EventType.repaint:
		
			if (_selectMesh != null && _selection.gameObject != null) {
				if (_SHOW_NEIBS || _SHOW_TRIAS)
					Draw_Handles ();
			}

			break;
		case EventType.layout:
			
			break;
				
		}
	}
	#endregion
	#region Editor GUI
	/** Main GUI draw function.*/
    public void DrawPanel()
    {
        if (_selection == null) OnSelectionChange();
        Debug.Log("--> "+ instance);
        DrawPanel2();
        Repaint();
    }
	private void DrawPanel2 ()
	{
		EditorGUILayout.BeginVertical (new GUIStyle { contentOffset = new Vector2 (-10, 0) });
		EditorGUILayout.Space ();
		
		// current selection idendifier
		GUI.enabled = (_selection != null);
		EditorGUILayout.ObjectField ("Selection ", _selection, typeof(GameObject), true);
		GUILayout.Label ("SubMeshes : " + (_selection != null && _selectMesh != null ? _selectMesh.subMeshCount : 0));
        GUILayout.Label("MeshFilters : " + (_selection != null && _selectMesh != null ? _selection.GetComponentsInChildren<MeshFilter>().Length : 0));
		EditorGUILayout.Space ();
		
		_LIST_HANDS = EditorGUILayout.Foldout (_LIST_HANDS, "Display Handles");
		
		if (_LIST_HANDS && _selectMesh != null) {
			EditorGUILayout.Space (); 
			_SHOW_TRIAS = EditorGUILayout.Toggle ("Triangles", _SHOW_TRIAS);
			EditorGUILayout.Space ();
			_SHOW_NEIBS = EditorGUILayout.Toggle ("Neigbours", _SHOW_NEIBS);
			GUILayout.FlexibleSpace ();
		}
		
		EditorGUILayout.Space ();
		DrawVectorLists ();
		
		EditorGUILayout.EndVertical ();		
	}

	private void DrawVectorLists ()
	{
		GUIStyle gs = new GUIStyle ();
		gs.wordWrap = true;
		gs.contentOffset = new Vector2 (10, 0);
		
		// list verticies
		int vc = (_selection != null) ? _selectMesh.vertexCount : -1;
		_LIST_VERTS = EditorGUILayout.Foldout (_LIST_VERTS, "Verticies [ " + vc + " ]");
		
		if (_LIST_VERTS && _selectMesh != null) {

			scp1 = GUILayout.BeginScrollView (scp1);
			vc = _selectMesh.vertices.Length;
				
			for (int i = 0; i < vc; i++) {
					
				if (_selectMesh.vertices [i].GetType () == typeof(Vector3)) {
					GUILayout.BeginHorizontal ();
					
					GUILayout.Label (i + " ");
					GUILayout.Space (2);
					GUILayout.Label (_selectMesh.vertices [i].x + " ");
					GUILayout.Space (2);
					GUILayout.Label (_selectMesh.vertices [i].y + " ");
					GUILayout.Space (2);
					GUILayout.Label (_selectMesh.vertices [i].z + " ");
					
					GUILayout.EndHorizontal ();
				}		
			}
			GUILayout.EndScrollView ();	
		}
		EditorGUILayout.Space ();
		// list triangles
		vc = (_selection != null) ? _selectMesh.triangles.Length : -1;
		_LIST_TRIAS = EditorGUILayout.Foldout (_LIST_TRIAS, "Triangles [ " + vc / 3 + " ]");
		if (_LIST_TRIAS && _selectMesh != null) {
			
			scp2 = GUILayout.BeginScrollView (scp2);
			int n = _selectMesh.triangles.Length;
			
			for (int i = 0; i < n; i += 6) {
				string s = " " + _selectMesh.triangles [i];
				s += "," + _selectMesh.triangles [i + 1];
				s += "," + _selectMesh.triangles [i + 2] + "| ";
		
				string s2 = " " + _selectMesh.triangles [i + 3];
				s2 += "," + _selectMesh.triangles [i + 4];
				s2 += "," + _selectMesh.triangles [i + 5] + "||";
				
				//	gs.normal.textColor = (i == _hitTriangle * 3) ? ((TriangleOpen) ? Color.yellow : Color.green) : (i == _pairTriangle * 3) ? Color.magenta : Color.black;
		
				
				GUILayout.BeginHorizontal ();
				GUILayout.Label (s, gs);
//				gs.normal.textColor = 
//					(i + 3 == _hitTriangle * 3) ? Color.green : (i + 3 == _pairTriangle * 3) ? Color.magenta : Color.black;
				GUILayout.Label (s2, gs);
				GUILayout.EndHorizontal ();
			}
			GUILayout.EndScrollView ();	
		}
		EditorGUILayout.Space ();
		// list neighbores
		vc = (_selection != null && neigbourList != null) ? neigbourList.Length : -1;
		_LIST_NEIBS = EditorGUILayout.Foldout (_LIST_NEIBS, "Neighbours [ " + vc / 6 + " ]");
		if (_LIST_NEIBS && neigbourList != null) {
			scp3 = GUILayout.BeginScrollView (scp3);
			int n = neigbourList.Length;
			for (int i = 0; i < n; i ++) {
				
				if (neigbourList [i] != null) {
					GUILayout.BeginHorizontal ();
					
					GUILayout.Label (i + " ");
					GUILayout.FlexibleSpace ();
					GUILayout.Label (((TriPoint)neigbourList [i])._p1 + " ");
					GUILayout.FlexibleSpace ();
					GUILayout.Label (((TriPoint)neigbourList [i])._p2 + " ");
					GUILayout.FlexibleSpace ();
					GUILayout.Label (((TriPoint)neigbourList [i])._p3 + " ");
					
					GUILayout.EndHorizontal ();
				}		
			}
			GUILayout.EndScrollView ();	
		}
		EditorGUILayout.Space ();
	}
	#endregion
	#region MeshTactics
	private void Calc_Neigbours ()
	{
		int n = _selectMesh.triangles.Length;
		
		if (neigbourList == null) {
			neigbourList = new TriPoint[n];
		}

		for (int i = 0; i < n; i += 3) {

			int p1 = _selectMesh.triangles [i];
			int p2 = _selectMesh.triangles [i + 1];
			int p3 = _selectMesh.triangles [i + 2];
			
			TriPoint tp = (p1 < n && neigbourList [p1] == null) ? new TriPoint () : neigbourList [p1];
			
			if (p2 + 1 != p3) {
				
				tp._p2 = p2;
				tp._p3 = p3;
				
			} else {
				
				tp._p1 = p2;	
				
			}
			
			neigbourList [p1] = tp;
		}
	}

	private static void Draw_Handles ()
	{
		if (_selectMesh.vertices == null || _selection == null)
			return;
		
		int _hitTriangle = HitTriangle ();
		if (_hitTriangle != -1) {
			Transform root = _selection.transform;
			int[] tList = _selectMesh.triangles;
			int tIndex = _hitTriangle * 3;
		
			if (tList == null || tIndex > tList.Length)
				return;
		
			int t1 = tList [tIndex + 0];
			int t2 = tList [tIndex + 1];
			int t3 = tList [tIndex + 2];
		
			// Draw current Triangle 
			//	bool isOpen = TriangleOpen;
			//	int tIndicie = _hitTriangle % 2;
			float cubeSize = HandleUtility.GetHandleSize (_selectMesh.vertices [t1]) * .1f;
		
			Vector3 v1 = root.TransformPoint (_selectMesh.vertices [t1]);
			Vector3 v2 = root.TransformPoint (_selectMesh.vertices [t2]);
			Vector3 v3 = root.TransformPoint (_selectMesh.vertices [t3]);
		
			Handles.color = new Color (Color.yellow.r, Color.yellow.g, Color.yellow.b, .85f);
			Handles.CubeCap (0, v1, root.rotation, cubeSize);
		
			int neighbourID = -1;
			Vector3 neighbourVector = Vector3.zero;
			if (t1 + 1 == t3) {
				if (_SHOW_NEIBS)
					neighbourVector = _selectMesh.vertices [((TriPoint)neigbourList [t1])._p1];
				Handles.color = new Color (Color.green.r, Color.green.g, Color.green.b, .85f); // TRI Point a
			} else {
				if (_SHOW_NEIBS)
					neighbourVector = _selectMesh.vertices [((TriPoint)neigbourList [t1])._p3];
				Handles.color = new Color (Color.red.r, Color.red.g, Color.red.b, .85f); // TRI Point b 
			}
		
			Handles.CubeCap (0, v2, root.rotation, cubeSize);
			Handles.CubeCap (0, v3, root.rotation, cubeSize);
			if (_SHOW_NEIBS) {
				Handles.color = new Color (Color.blue.r, Color.blue.g, Color.blue.b, .85f);
				Handles.CubeCap (0, root.TransformPoint (neighbourVector), root.rotation, cubeSize);
			}
		
			Handles.Label (v1, new GUIContent ("" + t1));
			Handles.Label (v2, new GUIContent ("" + t2));
			Handles.Label (v3, new GUIContent ("" + t3));
		
			Handles.Label ((v1 + v2 + v3) / 3, new GUIContent ("" + t1));

		}
	}

	private static int HitTriangle ()
	{
		Vector2 mp = Event.current.mousePosition;
		Ray r = HandleUtility.GUIPointToWorldRay (mp);
		RaycastHit hit;
		if (!Physics.Raycast (r, out hit, float.MaxValue)) {
			return-1;
		}
		if (hit.collider.gameObject != _selection) {
			_selection = hit.collider.gameObject;
			
			if (_selection != null) {
				_selectMeshFilter = _selection.GetComponent<MeshFilter> ();
				if (_selectMeshFilter != null) {
					_selectMesh = _selectMeshFilter.sharedMesh;
					neigbourList = null;
				}
				instance.Calc_Neigbours ();	
			}
		}
		instance.Repaint ();
		return hit.triangleIndex;
	}
	#endregion
}
