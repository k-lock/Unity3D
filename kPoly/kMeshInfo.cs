using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class kMeshInfo : EditorWindow
{
    #region vars
	/** Static instance to this editor class. */
	public static kMeshInfo instance;
	/** Static gui id to this editor window. */
	private static int _instanceHash;
	/** GUI Delegate method to update the sceneview from the editor window.*/
	private static SceneView.OnSceneFunc _onSceneGUI_ = null;

	/** The Selected gameobject from the scene view.*/
	private static GameObject _selection = null;
	/** The meshfilter componente from the current selected gameobject.*/
	private static MeshFilter _selectMeshFilter = null;
	/** The mesh/shared mesh componente from the current selected gameobject.*/
	private static Mesh _selectMesh = null;
	/** The mesh.triangle index for the mouseposition.*/
	private static int _hitTriangle = -1;
	/** The pair mesh.triangle index for the mouseposition.*/
	private static int _pairTriangle = -1;
	/** Helper for gui text scroll field.*/
	private Vector2 sc = Vector2.zero;
	private Vector2 sc2 = Vector2.zero;
	private Vector2 sc3 = Vector2.zero;
	private static bool _displayActTri = true;
	private static bool _displayParTri = false;
	

    #endregion
    #region editor
	/** The Unity EditorWindow start function.*/
	[MenuItem("Window/klock/kMesh/MeshInfo %M1")]
	public static void Init ()
	{
		instance = (kMeshInfo)EditorWindow.GetWindow (typeof(kMeshInfo), false, "Mesh Info");
		instance.Show ();
		instance.OnEnable ();
		instance.position = new Rect (200, 100, 200, 228);
	}
    #endregion
    #region unity
	void OnEnable ()
	{
		if (instance == null)
			instance = this;
		
		_instanceHash = instance.GetHashCode ();
		if (_onSceneGUI_ == null) {
			_onSceneGUI_ = new SceneView.OnSceneFunc (OnSceneGUI);
			SceneView.onSceneGUIDelegate += _onSceneGUI_;
		}
		
		InstanceRefresh ();
		FillList ();
	}
		
	static TriPoint[] list = null;

	void FillList ()
	{
		if (_selectMesh == null)
			return;
		
		
		int n = _selectMesh.triangles.Length;
		
		if (list == null)
			list = new TriPoint[n];
		
		/*	for (int i = 0; i < n; i += 3) {
			
			int p1 = _selectMesh.triangles [i];
			int p2 = _selectMesh.triangles [i + 1];
			int p3 = _selectMesh.triangles [i + 2];
			
			Debug.Log (i + "  " +  p1 + " " + p2 + " " + p3);
		}*/
		
		
		int t = 0;
		for (int i = 0; i < n; i += 3) {

			int p1 = _selectMesh.triangles [i];
			int p2 = _selectMesh.triangles [i + 1];
			int p3 = _selectMesh.triangles [i + 2];
			
			//Debug.Log (i/3+ " " +  TriList [i/3]);
			TriPoint tp = (p1 < n && list [p1] == null) ? new TriPoint () : list [p1];
			//	Debug.Log ("Has Local "+ list [p1]);
			if (p2 + 1 != p3) {
				tp._p2 = p2;
				tp._p3 = p3;			
			} else {
				tp._p1 = p2;					
			}
			list [p1] = tp;
			//Debug.Log (i+" Save : " + p1 + " - " + tp.Trace());
			//if( (i/3)%2 == 1) t++;
			//Debug.Log (i + " " + p1 + "  " + tp.Trace () + " is Open : " + (p2 + 1 == p3));
		}
	}

	void OnDisable ()
	{
		_onSceneGUI_ = null;
	}

	void Update ()
	{
		
	}

	void OnGUI ()
	{
		if (_selectMesh == null || _selection == null)
			InstanceRefresh ();
			
		DrawPanel ();
	}

	void OnInspectorUpdate ()
	{
	}

	void OnSelectionChange ()
	{

		InstanceRefresh ();
		Repaint ();
	}

	private static void InstanceRefresh ()
	{
		_selection = Selection.activeGameObject;
		if (_selection != null) {
			_selectMeshFilter = _selection.GetComponent<MeshFilter> ();
			if (_selectMeshFilter != null) {
				_selectMesh = _selectMeshFilter.sharedMesh;
				//TriList = null;
			}
			//	CreateTriList ();	
		}
		
	}
    #endregion
	#region listener
	public static void OnSceneGUI (SceneView sceneview)
	{ 
		
		Event e = Event.current;
		switch (e.type) {
		case EventType.mouseMove:
		case EventType.repaint:
			int hitTemp = _hitTriangle;
			
			_hitTriangle = HitTriangle ();
			/*_pairTriangle = PairTriangle ();//FindNeighbour ();//
			*/
			if (_hitTriangle != -1 && _selectMesh != null && _selection.gameObject != null) {
				DrawHandles ();
			}
			/*
			Vector3[] verts = _selectMesh.vertices;
	 		int n = verts.Length;
	    	for (int i = 0; i<n;i++)
	        	verts[i] = verts[i] + Handles.PositionHandle( _selection.transform.position + verts[i], Quaternion.identity);
	    	//_selectMesh.vertices = verts;	
			*/
			break;
		case EventType.layout:
			
			break;
				
		}
	}
    #endregion
    #region meshInfo
	private static void DrawHandles ()
	{
		Transform root = _selection.transform;
		//	Vector3[] vertices = _selectMesh.vertices;
		int[] tList = _selectMesh.triangles;
		int tIndex = _hitTriangle * 3;
		
		
		int t1 = tList [tIndex + 0];
		int t2 = tList [tIndex + 1];
		int t3 = tList [tIndex + 2];
		
		// Draw current Triangle 
		bool isOpen = TriangleOpen;
		int tIndicie = _hitTriangle % 2;
		float cubeSize = .1f;
		
		Vector3 v1 = root.TransformPoint (_selectMesh.vertices [t1]);
		Vector3 v2 = root.TransformPoint (_selectMesh.vertices [t2]);
		Vector3 v3 = root.TransformPoint (_selectMesh.vertices [t3]);
		
		Handles.color = new Color (Color.yellow.r, Color.yellow.g, Color.yellow.b, .85f);
		Handles.CubeCap (0, v1, root.rotation, cubeSize);
		
		if (t1 + 1 == t3)
			Handles.color = new Color (Color.green.r, Color.green.g, Color.green.b, .85f); // TRI Point a
		else
			Handles.color = new Color (Color.red.r, Color.red.g, Color.red.b, .85f); // TRI Point b 
		
		
		Handles.CubeCap (0, v2, root.rotation, cubeSize);
		Handles.CubeCap (0, v3, root.rotation, cubeSize);
		
		Handles.Label (v1, new GUIContent ("" + t1));
		Handles.Label (v2, new GUIContent ("" + t2));
		Handles.Label (v3, new GUIContent ("" + t3));
		
		Handles.Label ((v1 + v2 + v3) / 3, new GUIContent ("" + t1));
		if (_displayParTri) {
			
			int pIndex = _pairTriangle * 3;
		
			int p1 = tList [pIndex + 0];
			int p2 = tList [pIndex + 1];
			int p3 = tList [pIndex + 2];
		
			Vector3 vp1 = root.TransformPoint (_selectMesh.vertices [p1]);
			Vector3 vp2 = root.TransformPoint (_selectMesh.vertices [p2]);
			Vector3 vp3 = root.TransformPoint (_selectMesh.vertices [p3]);

			if (pIndex % 2 == 0)
				Handles.color = new Color (Color.yellow.r, Color.yellow.g, Color.yellow.b, .25f);
			else
				Handles.color = new Color (Color.cyan.r, Color.cyan.g, Color.cyan.b, .25f);	
		
			Handles.CubeCap (0, vp1, root.rotation, cubeSize * 2);
			Handles.CubeCap (0, vp2, root.rotation, cubeSize * 2);
			Handles.CubeCap (0, vp3, root.rotation, cubeSize * 2);
		}
	}

	void DrawPanel ()
	{
		EditorGUILayout.BeginVertical (new GUIStyle { contentOffset = new Vector2 (-10, 0) });
		EditorGUILayout.Space ();

		GUI.enabled = (_selectMesh != null);

		EditorGUILayout.ObjectField ("GameObject ", _selection, typeof(GameObject), true);
		GUILayout.BeginHorizontal ();
		
		GUILayout.Label ("Vertices: " + ((_selectMesh != null) ? _selectMesh.vertices.Length : -1));
		GUILayout.Label ("Triangles: " + ((_selectMesh != null) ? _selectMesh.triangles.Length / 3 : -1));
		
		GUILayout.EndHorizontal ();
		string s = "";
		if (_selectMesh != null) {
			EditorGUILayout.Space ();
			EditorGUILayout.Separator ();
			GUIStyle gs = new GUIStyle ();
			gs.wordWrap = true;
			gs.contentOffset = new Vector2 (10, 0);
			
			if (_selectMesh != null) {
				sc3 = GUILayout.BeginScrollView (sc3);
				int vc = _selectMesh.vertices.Length;//Debug.Log (n);
				GUILayout.Label ("Verts :" + vc);
				for (int i = 0; i < vc; i++) {
					if (_selectMesh.vertices [i] != null) {
						GUILayout.Label (i + " - "
					+ _selectMesh.vertices [i].x + " "
					+ _selectMesh.vertices [i].y + " "
					+ _selectMesh.vertices [i].z + " ", gs);
					}		
				}
				GUILayout.EndScrollView ();	
			}
			EditorGUILayout.Space ();
			sc = GUILayout.BeginScrollView (sc);
			int n = _selectMesh.triangles.Length;
			for (int i = 0; i < n; i += 6) {
				s = "";
				s += " " + _selectMesh.triangles [i];//+ " ~ "  + _selectMesh.vertices [_selectMesh.triangles [i]];
				s += "," + _selectMesh.triangles [i + 1];
				s += "," + _selectMesh.triangles [i + 2] + "| ";
		
				string s2 = " " + _selectMesh.triangles [i + 3];
				s2 += "," + _selectMesh.triangles [i + 4];
				s2 += "," + _selectMesh.triangles [i + 5] + "||";
				
				gs.normal.textColor = (i == _hitTriangle * 3) ? ((TriangleOpen) ? Color.yellow : Color.green) : (i == _pairTriangle * 3) ? Color.magenta : Color.black;
		
				
				GUILayout.BeginHorizontal ();
				GUILayout.Label (s, gs);
				gs.normal.textColor = (i + 3 == _hitTriangle * 3) ? Color.green : (i + 3 == _pairTriangle * 3) ? Color.magenta : Color.black;
				GUILayout.Label (s2, gs);
				GUILayout.EndHorizontal ();
			}
			GUILayout.EndScrollView ();	
		}
		EditorGUILayout.Space ();
		EditorGUILayout.Separator ();
		if (list != null) {
			sc2 = GUILayout.BeginScrollView (sc2);
			GUIStyle gs = new GUIStyle ();
			gs.wordWrap = true;
			gs.contentOffset = new Vector2 (10, 0);
			
			int n = list.Length;//Debug.Log (n);
			GUILayout.Label ("tList :" + n);
			for (int i = 0; i < n; i++) {
				if (((TriPoint)list [i]) != null) {
					GUILayout.Label (i + " - "
					+ ((TriPoint)list [i])._p1 + " "
					+ ((TriPoint)list [i])._p2 + " "
					+ ((TriPoint)list [i])._p3 + " "
					//+ ((TriPoint)TriList [i])._p4
						, gs);
				}
				//Debug.Log (connections [i]);
				/*if (connections [i] != null) {
					VertexConnection vc = connections [i];
					if (vc.connections.Count > 0) {
						s = i / 3 + " - ";
						foreach (int vci in vc.connections) {
							s += vci + " ";
							//Debug.Log(s);
						}
						GUILayout.Label (s);
							
					}
				}	*/			
			}
			GUILayout.EndScrollView ();	
		}
		EditorGUILayout.Space ();
		EditorGUILayout.Separator ();
		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Hit Triangle: " + _hitTriangle);
		GUILayout.Label ("Pair Triangle: " + _pairTriangle);
		GUILayout.EndHorizontal ();
		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Act Triangle");
		_displayActTri = EditorGUILayout.Toggle (_displayActTri);
		GUILayout.Label ("Pair Triangle");
		_displayParTri = EditorGUILayout.Toggle (_displayParTri);
		GUILayout.EndHorizontal ();

		EditorGUILayout.EndVertical ();
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
					//TriList = null;
				}
				//	CreateTriList ();	
			}
		}
		instance.Repaint ();
		return hit.triangleIndex;
	}
	
	private static int PairTriangle ()
	{
		int n = Mathf.Clamp (_hitTriangle + (_hitTriangle % 2 == 0 ? ((TriangleOpen) ? 1 : -1) : -1), -1, 99999);
		/*if (_selectMesh.triangles [_hitTriangle * 3] != _selectMesh.triangles [_hitTriangle * 3 + 3]) {
			//Debug.Log (" Dont Compare !!! " + _pairTriangle);
			n -= 1;
		}*/
		return n;
	}
	
	private static TriPoint[] TriList = null;
	
	/** 
	 * 	Create a vector4 quad list from the current selected mesh.triangles.
	 * 	The list holds a vector4 object for every P1 vertex in the mesh.
	 * 	
	 * 	TriList [ x ] = TriPoint ( p1, p2, p3, p4 ) 
 	 * 
	 * P4		  	P1
	 * 	 __________
	 * 	| 		  /|		[ TriangleIndex * 3 == P1 ]
	 * 	| close  / |
	 * 	| tri 	/  |		if( P1.value +1 == P4.value ) 
	 * 	|      /   |			close tri
	 * 	|     /	   |		else
	 * 	|    /     |			open tri
	 * 	|   / 	   |
	 * 	|  /  open |
	 * 	| /    tri |
	 * 	|/_________|
	 * 
	 * P3			P2
	 * 
	 * 	Get the oppsite triangle from the current mouse hit data.
	 *  Calculate the current triangleIndex ( hitIndex *3 ), 
	 *  check is triangle open - or close, to get the fourth point.
	 * 	
	 *  if( open tri )
	 *  	TriList [ hitTriangleIndex*3  ]._p4
	 *  else
	 * 		TriList [ hitTriangleIndex*3  ]._p2
	 * 
	 */
	
	private static void CreateTriList ()
	{
		int n = _selectMesh.triangles.Length;
		if (TriList == null) {
			TriList = new TriPoint[999999];
			/*for (int i = 0; i < n; i ++) TriList [i] = null;*/
		}
		int t = 0;
		for (int i = 0; i < n; i += 3) {
			
			if (i > 60)
				return;
			
			int p1 = _selectMesh.triangles [i];
			int p2 = _selectMesh.triangles [i + 1];
			int p3 = _selectMesh.triangles [i + 2];
			
			//Debug.Log (i/3+ " " +  TriList [i/3]);
			TriPoint tp = (TriList [i / 3] == null) ? new TriPoint () : TriList [t];
			Debug.Log (i + "  " + t + " " + p1 + " " + p2 + " " + p3);
			tp.Trace ();
			/*if( i < 6 ){
			Debug.Log(p1 +" // check if triangle is open or closed " +(p1 + 1 != p3) + " / " + TriList [i/3]) ;
			}*/
			
			
			/*if (p1 + 1 != p3) {
				//tp._p1 = p2;
				tp._p2 = p2;
				tp._p3 = p3;
				//if( i < 6 )tp.Trace();
			} else {
				
				tp._p1 = p2;			
				//if( i < 6 )tp.Trace();
				//tp._p3 = p3;				
			}
		
			TriList [i/3] = tp;*/
			//if( i < 6 )TriList [t].Trace();
			t += 2;
		}	
	}
	/*private static int FindNeighbour ()
	{
		int neiTri = -1;
		int i = _hitTriangle * 3; // each triangle occupies 3 entries in the triangles array
		if (_selectMesh != null && _hitTriangle != -1) {
			int v1 = _selectMesh.triangles [i++]; // get v1, v2 and v3, 
			int v2 = _selectMesh.triangles [i++]; // the 3 vertex indexes of
			int v3 = _selectMesh.triangles [i];   // triangle #_hitTriangle
			for (i = 0; i < _selectMesh.triangles.Length; i++) {
				// compare each vertex index to v1, v2 and v3:
				int v = _selectMesh.triangles [i];
				if (v == v1 || v == v2 || v == v3) { // if any common vertex found...
					neiTri = i / 3; // find the triangle number...
					if (neiTri != _hitTriangle && neiTri != _pairTriangle) { // and if it's diff from #_hitTriangle...
						// triangle #nTri is neighbour of triangle #_hitTriangle
						_pairTriangle = neiTri;
					}
				}
			}
		}
		return neiTri;
	}*/

	private static bool TriangleOpen {
		get {
			return (_selectMesh.triangles [_hitTriangle * 3 + 1] + 1 == _selectMesh.triangles [_hitTriangle * 3 + 2]);
		}
	}

	
	
    #endregion
}

public class TriPoint
{
	public  int _p1, _p2, _p3 = -1;

	public TriPoint Init (int p1=-1, int p2=-1, int p3=-1)//, int p4=-1)
	{
		if (p1 != -1)
			_p1 = p1;
		if (p2 != -1)
			_p2 = p2;
		if (p3 != -1)
			_p3 = p3;
		//_p4 = p4;
		
		return this;
	}

	public string Trace ()
	{
		return this._p1 + " " + this._p2 + " " + this._p3;// + " " + this._p4 + " ");	
	}
}
/*
public class VertexConnection
{
	public List<int> connections = new List<int> ();
}*/