/** http://www.k-lock.de  | Paul Knab 
 * 	_______________________________________
 * 	
 * 	kPolyCreate | V.1.0.1 | 05.04.2013
 *  ________________________________________
 * 
 * 	Editor Window for mesh creation  
 * 	in Unity3D Scene Editor
 * 
 * 
 * 	Properties for generating mesh component
 * 
 * 	- Segments 
 * 	- Size
 * 	- Pivot point for mesh
 * 	- Facing direction
 * 	- Triangle winding
 * 
 *  Extra :
 *  
 *  - Add your favorite MeshCollider / BoxCollider
 * 
 * */
using UnityEngine;
using UnityEditor;

public class kPolyCreate : EditorWindow
{
    #region vars
	/** Static instance to this editor class. */
	public static kPolyCreate 	instance;
	private 	  string 		_meshName = "";
	private 	  float 		_width = 1;
	private 	  float 		_height = 1;
	private 	  int 			_uSegments = 1;
	private 	  int			_vSegments = 1;
	private 	  int 			_pivotIndex = 0;
	//private 	  TextAnchor 	_pivot = TextAnchor.MiddleCenter;
	private 	  string[] 		_pivotLabels = {"UpperLeft","UpperCenter","UpperRight", "MiddleLeft","MiddleCenter","MiddleRight", "LowerLeft", "LowerCenter","LowerRight"};
	private 	  int 			_faceIndex = 0;
	private 	  string[] 		_faceLabels = {"XZ","XY"};
	private		  string[]		_windinLabels = { "TopLeft","TopRight", "ButtomLeft", "ButtomRight" };
	private		  int 			_windinIndex = 2;
	private		  string[]		_colliderLabels = { "none" ,"MeshCollider", "BoxCollider" };
	private		  int 			_colliderIndex = 1;
	
	#endregion
	#region Editor
	/** The Unity EditorWindow start function.*/
	[MenuItem("Window/klock/kMesh/kPolyCreate %M1")]
	public static kPolyCreate Init ()
	{
		instance = (kPolyCreate)EditorWindow.GetWindow (typeof(kPolyCreate), false, "Create");
		instance.Show ();
		//	instance.OnEnable ();
		instance.position = new Rect (200, 100, 200, 228);
		
		return instance;
	}

	public static kPolyCreate Create ()
	{
		return CreateInstance<kPolyCreate> ();
	}
	/** Reset the editor values to default.*/
	private void ResetEditorValues ()
	{
		_width = 1;
		_height = 1;
			
		_uSegments = 1;
		_vSegments = 1;
			
		_pivotIndex = 0;
		_faceIndex = 0;
		_colliderIndex = 1;
		_windinIndex = 2;
	}
    #endregion
	#region Unity
	private void OnEnable ()
	{
		if (instance == null) {
			instance = this;	
		}
		ResetEditorValues ();
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
	
	
	
	#endregion
	#region Editor GUI
	/** Main GUI draw function.*/
	
	public void DrawPanel ()
	{
		//Debug.Log(instance);
		DrawPanel3 ();
	}

	private void DrawPanel3 ()
	{
		bool GUI_TEMP = GUI.enabled;
		EditorGUILayout.BeginVertical ();//new GUIStyle { contentOffset = new Vector2 (0, 0) });
		//OBJECT_TYPES_INDEX = EditorGUILayout.Popup (OBJECT_TYPES_INDEX, OBJECT_TYPES);
		
		// OBJECT TYPE 
		FOLD_object = EditorGUILayout.Foldout (FOLD_object, "Object Type");
		if (FOLD_object) {
			GUILayout.BeginHorizontal ();
			//EditorGUILayout.Space ();
			GUI.color = Color.white;
			GUI.color = (P_OBJECT_TYPE_INDEX == 0) ? Color.grey : Color.white;
			if (GUILayout.Button ("Cube")) {
				P_OBJECT_TYPE_INDEX = (P_OBJECT_TYPE_INDEX == 0) ? -1 : 0;
			}
			//EditorGUILayout.Space ();
			GUI.color = Color.white;
			GUI.color = (P_OBJECT_TYPE_INDEX == 1) ? Color.grey : Color.white;
			if (GUILayout.Button ("Sphere")) {
				P_OBJECT_TYPE_INDEX = (P_OBJECT_TYPE_INDEX == 1) ? -1 : 1;
			}
			//EditorGUILayout.Space ();
			GUI.color = Color.white;
			GUI.color = (P_OBJECT_TYPE_INDEX == 2) ? Color.grey : Color.white;
			if (GUILayout.Button ("Plane")) {
				P_OBJECT_TYPE_INDEX = (P_OBJECT_TYPE_INDEX == 2) ? -1 : 2;
			}
			GUI.color = Color.white;
			//EditorGUILayout.Space ();
			GUILayout.EndHorizontal ();
		}
		EditorGUILayout.Space ();
		// OBJECT NAME
		FOLD_name = EditorGUILayout.Foldout (FOLD_name, "Object Name");
		
		if (FOLD_name) {
			GUILayout.BeginHorizontal ();
			EditorGUILayout.Space ();
			GUI.enabled = P_OBJECT_TYPE_INDEX != -1;
			_meshName = EditorGUILayout.TextField (_meshName);
			GUI.enabled = GUI_TEMP;
			EditorGUILayout.Space ();
			GUILayout.EndHorizontal ();
		}
		EditorGUILayout.Space ();
		// OBJECT PARAMETERS
		FOLD_para = EditorGUILayout.Foldout (FOLD_para, "Parameters");
		if (FOLD_para) {
			GUILayout.BeginHorizontal ();
			GUI.enabled = P_OBJECT_TYPE_INDEX != -1;
			
			switch (P_OBJECT_TYPE_INDEX) {
			case 0:
				
				break;	
			case 1:
				
				break;	
			case 2:
				DrawPanel2 ();
				break;	
			}
				
			
			GUILayout.EndHorizontal ();
		}
		EditorGUILayout.EndHorizontal ();
	}

	private void DrawPanel2 ()
	{
		EditorGUI.BeginChangeCheck ();
		EditorGUILayout.BeginVertical ();//new GUIStyle { contentOffset = new Vector2 (0, 0) });
		GUILayout.BeginHorizontal ();
		// Editor value reset button
		if (GUILayout.Button (new GUIContent ("Reset Editor"), EditorStyles.miniButton)) {
			ResetEditorValues ();
		}
		GUILayout.EndHorizontal ();
		EditorGUILayout.Space ();
		// Editor value for width and height of the created mesh [ float ]
		_width = EditorGUILayout.FloatField ("Width", _width);
		_height = EditorGUILayout.FloatField ("Height", _height);
		EditorGUILayout.Space ();
		// Editor value for width and height segments of the created mesh [ int ]
		_uSegments = EditorGUILayout.IntField ("uSegments", _uSegments);
		_vSegments = EditorGUILayout.IntField ("vSegments", _vSegments);
		EditorGUILayout.Space ();
		GUILayout.BeginHorizontal ();
		// Editor value for the pivot point of the created mesh Unity.TextAnchor
		GUILayout.Label ("Pivot");
		GUILayout.Space (18);
		_pivotIndex = EditorGUILayout.Popup (_pivotIndex, _pivotLabels);
		GUILayout.EndHorizontal ();
		GUILayout.BeginHorizontal ();
		// Editor value for the mesh face direction FACING.XZ
		GUILayout.Label ("Facing ");
		GUILayout.Space (10);
		_faceIndex = EditorGUILayout.Popup (_faceIndex, _faceLabels);
		GUILayout.EndHorizontal ();	
		GUILayout.BeginHorizontal ();
		// Editor value for triangle winding order
		GUILayout.Label ("Winding");
		GUILayout.Space (2);
		_windinIndex = EditorGUILayout.Popup (_windinIndex, _windinLabels);
		GUILayout.EndHorizontal ();	
		GUILayout.BeginHorizontal ();
		// Editor value for collider export
		GUILayout.Label ("Collider ");
		GUILayout.Space (3);
		_colliderIndex = EditorGUILayout.Popup (_colliderIndex, _colliderLabels);
		GUILayout.EndHorizontal ();	
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		// Starting GUI changes check
		if (EditorGUI.EndChangeCheck ()) {
			_width = Mathf.Clamp (_width, 0, int.MaxValue);
			_height = Mathf.Clamp (_height, 0, int.MaxValue);
			_uSegments = Mathf.Clamp (_uSegments, 1, int.MaxValue);
			_vSegments = Mathf.Clamp (_vSegments, 1, int.MaxValue);
			Debug.Log ("Change Editor");
		}
		// Editor Button for start mesh creation
		if (GUILayout.Button (new GUIContent ("Create Mesh"))) {
			CreateMesh ();
		}
		EditorGUILayout.EndVertical ();
	}

	#endregion
	#region Export - Mesh Creation
	/** GameObject Export / Mesh Creation function*/
	private void CreateMesh ()
	{
		//string assetName = "Assets/quad.asset";
		GameObject quad = new GameObject ();
		if (!string.IsNullOrEmpty (_meshName))
			quad.name = _meshName;
		else
			quad.name = "kPoly";
		
		quad.transform.position = Vector3.zero;
		
		
	
		int xCount = _uSegments + 1;
		int yCount = _vSegments + 1;
		int tCount = _uSegments * _vSegments * 6;
		int vCount = xCount * yCount;

		Vector3[] vertices = new Vector3[vCount];
		Vector2[] uvs = new Vector2[vCount];
		int[] triangles = new int[tCount];
			
		int index = 0;
		float xUV = 1.0f / _uSegments;
		float yUV = 1.0f / _vSegments;
		float xSC = _width / _uSegments;
		float ySC = _height / _vSegments;
			
		Vector2 pivot = PivotVector;
		//Vector2 windin = TriangleWinding;
						
		for (float y = 0.0f; y < yCount; y++) {
			for (float x = 0.0f; x < xCount; x++) {

				float dx = x * xSC - _width * .5f - pivot.x * .5f;
				float dy = y * ySC - _height * .5f - pivot.y * .5f;
					
				vertices [index] = (_faceIndex == 0) ? new Vector3 (dx, 0.0f, dy) : new Vector3 (dx, dy, 0.0f);		
				uvs [index++] = new Vector2 (x * xUV, y * yUV);
			}
		}

		index = 0;
		for (int y = 0; y < _vSegments; y++) {
			for (int x = 0; x < _uSegments; x++) {

				int p1 = (y * xCount) + x;
				int p2 = (y * xCount) + x + 1;
				int p3 = ((y + 1) * xCount) + x;
				int p4 = ((y + 1) * xCount) + x + 1;
				
				switch (_windinIndex) {
				case 0:
					triangles [index] = p3;
					triangles [index + 1] = p2;
					triangles [index + 2] = p1;
 
					triangles [index + 3] = p3;
					triangles [index + 4] = p4;
					triangles [index + 5] = p2;
					break;
				case 1:
					triangles [index] = p4;
					triangles [index + 1] = p1;
					triangles [index + 2] = p3;
 
					triangles [index + 3] = p4;
					triangles [index + 4] = p2;
					triangles [index + 5] = p1;
					break;
				case 2:
					triangles [index] = p1;
					triangles [index + 1] = p4;
					triangles [index + 2] = p2;
 
					triangles [index + 3] = p1;
					triangles [index + 4] = p3;
					triangles [index + 5] = p4;
					break;
				case 3:
					triangles [index] = p2;
					triangles [index + 1] = p3;
					triangles [index + 2] = p4;
 
					triangles [index + 3] = p2;
					triangles [index + 4] = p1;
					triangles [index + 5] = p3;
					break;
				}					
				index += 6;
			}
		}
		
		MeshFilter mf = quad.AddComponent<MeshFilter> ();
		quad.AddComponent<MeshRenderer> (); //MeshRenderer mr = 
		Mesh m = new Mesh ();
		
		m.name = quad.name;
		m.vertices = vertices;
		m.uv = uvs;
		m.triangles = triangles;	

		mf.sharedMesh = m;
			
		m.RecalculateNormals ();
		m.RecalculateBounds ();
		
		AddCollider (quad, m);

		//	AssetDatabase.CreateAsset (m, assetName);
		//	AssetDatabase.SaveAssets ();
		
	}
	/** Add a collider object to the generated gameobject.
	 *  @params GameObject quad - The target gameobject.
	 * 	@params Mesh m - The sharedMesh property for a MeshCollider componete.
	 */
	private void AddCollider (GameObject quad, Mesh m)
	{
		if (_colliderIndex > 0) {
				
			switch (_colliderIndex) {
			case 1: // mesh 
				MeshCollider mc = quad.AddComponent<MeshCollider> ();
				mc.sharedMesh = m;
				break;
			case 2: // box
				quad.AddComponent<BoxCollider> ();
				break;
			}				
		}
	}
	/** Returns a vector2 containing the winding order for the triangles in the generated mesh.*/
	private Vector2 TriangleWinding {
		get {
			Vector2 p = Vector2.zero;
			switch (_windinIndex) {
			case 0:
				p = -Vector2.up;
				break;
			case 1:
				p = -Vector2.right;
				break;
			case 2:
				p = Vector2.right;
				break;
			case 3:
				p = Vector2.up;
				break;
			}
			return p;
		}
	}
	/** Returns a vector2 containing the new position for the transform point of the generated mesh.*/
	private Vector2 PivotVector {
		get {
			Vector2 p = Vector2.zero;
			switch (_pivotIndex) {
			case 0://TextAnchor.UpperLeft:
				p = new Vector2 (-_width, _height);
				break;
			case 1://TextAnchor.UpperCenter:
				p = new Vector2 (0, _height);
				break;
			case 2://TextAnchor.UpperRight:
				p = new Vector2 (_width, _height);
				break;		
			case 3://TextAnchor.MiddleLeft:
				p = new Vector2 (-_width, 0);
				break;
			case 4://TextAnchor.MiddleCenter:
				p = Vector2.zero;
				break;
			case 5://TextAnchor.MiddleRight:
				p = new Vector2 (_width, 0);
				break;	
			case 6://TextAnchor.LowerLeft:
				p = new Vector2 (-_width, -_height);
				break;
			case 7://TextAnchor.LowerCenter:
				p = new Vector2 (0, -_height);
				break;
			case 8://TextAnchor.LowerRight:
				p = new Vector2 (_width, -_height);
				break;	
			}
			return p;	
		}
	}
	#endregion
	private bool FOLD_para = false;
	private bool FOLD_name = false;
	private bool FOLD_object = false;
	private int OBJECT_TYPES_INDEX = 0;
	private string[] OBJECT_TYPES = new string[2]{"Standard","Extra"};
	private int P_OBJECT_TYPE_INDEX = -1;
	private string[] P_OBJECT_TYPE = new string[3]{"Cube","Sphere","Plane"};
}
