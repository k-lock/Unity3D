/* kTile V.0.1 - 2012 - Paul Knab */
using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class kTile : MonoBehaviour
{

	public 		Rect 		uvRect 	= new Rect (0, 0, 50, 50);
	public 		float 		_width 	= 1;
	public 		float 		_height = 1;

	protected 	Vector3[] 	verts 	= new Vector3[4];
	protected 	Vector2[] 	uvs 	= new Vector2[4];
	protected 	int[]		trias 	= new int[6];
	protected 	Color[] 	colors 	= new Color[4];
	[HideInInspector]
	private 	Mesh 		mesh = null;
	[HideInInspector]
	public 		MeshFilter 	meshFilter;
	[HideInInspector]
	public 		MeshRenderer meshRenderer;

    #region UNITY
//#if UNITY_EDITOR
	
	public void Awake ()
	{
		if( mesh != null) mesh = null;
	}

	void Update ()
	{

	}

    void OnEnable ()
	{
		MESH_refresh ();
	}
	
	void OnDisable ()
	{
		//GetComponent<MeshFilter> ().mesh = mesh = null;
		//DestroyImmediate (mesh);
	}

//	#endif
	public void MESH_refresh ()
	{

		Components ();
		MeshUpdate ();
		
	}
	
    #endregion
	void MeshUpdate ()
	{
	//	if( gameObject.name=="LOGO_big" )Debug.Log(  meshRenderer.sharedMaterial);
		if(meshRenderer.sharedMaterial==null)return; 
	
		Mesh m = (mesh == null ) ? GetComponent<MeshFilter> ().sharedMesh : mesh ;
		
		// Setup mesh
		m.Clear ();
		m.vertices = verts = new Vector3[4] { new Vector3 (0, 0, 0), new Vector3 (0, _height, 0), new Vector3 (_width, _height, 0), new Vector3 (_width, 0, 0) };
		m.triangles = trias = new int[6] { 0, 1, 3, 3, 1, 2 };
		m.uv = uvs = UV_setup ();

		// Update mesh
		m.RecalculateBounds ();
		m.RecalculateNormals ();
	}

	public void MESH_setup (Rect UVrect)
	{
		MESH_setup (UVrect.width, UVrect.height, UVrect);
	}

	public void MESH_setup (float width, float height, Rect UVrect)
	{
		_width = UVrect.width / 100;
		_height = UVrect.height / 100;

		uvRect = UVrect;

		MESH_refresh ();

	}

	public void Components ()
	{
		meshRenderer = GetComponent<MeshRenderer> ();
		if (meshRenderer == null) {
			meshRenderer = gameObject.AddComponent<MeshRenderer> ();
		}
		MeshFilter meshFilter = GetComponent<MeshFilter> ();
		if (meshFilter == null) {
			meshFilter = gameObject.AddComponent<MeshFilter> ();
			//meshRenderer.material = mat;
		}
		if (mesh == null) {
			GetComponent<MeshFilter> ().mesh = mesh = new Mesh ();
			mesh.name = "kMesh";
			//mesh.hideFlags = HideFlags.HideAndDontSave;
		}
		/*if (mat == null) {
			GetComponent<MeshRenderer> ().sharedMaterial = mat = new Material (Shader.Find ("Transparent/Diffuse"));
		}*/
		//meshFilter.mesh = mesh;
	}
    #region HELPER
	private Vector2[] UV_setup ()
	{
		if (meshRenderer != null) {
			Vector2 lLeftUV = PixelCoordToUVCoord (new Vector2 (uvRect.x, uvRect.y));
			Vector2 UVDimen = PixelSpaceToUVSpace (new Vector2 (uvRect.width, -uvRect.height));

			uvs [0] = lLeftUV + Vector2.up * UVDimen.y;
			uvs [1] = lLeftUV;
			uvs [2] = lLeftUV + Vector2.right * UVDimen.x;
			uvs [3] = lLeftUV + UVDimen;
	//		Debug.Log (meshRenderer.sharedMaterial.mainTexture);
		}
		
		return uvs;
	}

	private Vector2 PixelSpaceToUVSpace (Vector2 xy)
	{
		Texture t = meshRenderer.sharedMaterial.mainTexture;

		return new Vector2 (xy.x / ((float)t.width), xy.y / ((float)t.height));
	}

	private Vector2 PixelCoordToUVCoord (Vector2 xy)
	{
		Vector2 p = PixelSpaceToUVSpace (xy);
		p.y = 1.0f - p.y;
		return p;
	}
    #endregion
    #region GETTER

	public Vector3[] VERTS {
		get { return verts; }
	}

	public Vector2[] UVS {
		get { return uvs; }
	}

	public int[] TRIAS {
		get { return trias; }
	}
    #endregion
}

