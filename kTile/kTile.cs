/* kTile V.0.1 - 2012 - Paul Knab */

using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class kTile : MonoBehaviour
{

	//public bool 			editable = false;
	public Rect 			uvRect	 = new Rect (0, 0, 50, 50);
	public float 			_width 	 = 1;
	public float 			_height  = 1;
//	public bool 			sharedMesh = true;
	
	protected Vector3[] 	verts 	 = new Vector3[4];
	protected Vector2[] 	uvs 	 = new Vector2[4];
	protected int[]			trias    = new int[6];
	protected Color[] 		colors   = new Color[4];
	
	[HideInInspector]
	private Mesh 			mesh = null;
	[HideInInspector]
	public MeshFilter 	    meshFilter;
	[HideInInspector]
	public MeshRenderer 	meshRenderer;
	
	
	
	public Mesh MESH_init()
	{
		//Debug.Log ( "MESH_init -------------------------->");
		
		meshFilter = (MeshFilter)gameObject.GetComponent (typeof(MeshFilter));
		if (meshFilter == null) meshFilter = (MeshFilter)gameObject.AddComponent (typeof(MeshFilter));

		meshRenderer = (MeshRenderer)gameObject.GetComponent (typeof(MeshRenderer));
		if (meshRenderer == null) meshRenderer = (MeshRenderer)gameObject.AddComponent (typeof(MeshRenderer));
		
		Mesh m = new Mesh ();
		m.name = "kTile";
		m.vertices = VERTS;
		m.uv = UVS;
		m.triangles = TRIAS;	
		m.RecalculateNormals ();
	
		meshFilter.sharedMesh = new Mesh();
		mesh = meshFilter.mesh = m;
		
		m.RecalculateBounds ();	
	
		return m;
	}
	
	public void MESH_update()
	{
		
		SetupArrays ();
			
		Mesh m =  ( mesh != null ) ? mesh : MESH_init (); // ( sharedMesh && !Application.isPlaying) ? meshFilter.sharedMesh : 

		m.Clear();
		m.vertices = VERTS;
		m.uv = UVS;
		m.triangles = TRIAS;
       
		m.RecalculateNormals ();
		//m.RecalculateBounds (); -  Assigning triangles will automatically Recalculate the bounding	
	}
	
	public void Awake ()
	{
	
	}

	void Update ()
	{

	}
	
	public void SetupArrays (float width, float height, Rect UVrect)
	{
		_width  = UVrect.width/100;
		_height = UVrect.height/100;
		
		uvRect  = UVrect;
	
		SetupArrays ();
		//Debug.Log (_width+" " + _height);
	}

	void SetupArrays ()
	{
		
		verts = new Vector3[4] { new Vector3 (0, 0, 0), new Vector3 (0, _height, 0), new Vector3 (_width, _height, 0), new Vector3 (_width, 0, 0)};	
		uvs   = new Vector2[4] { new Vector2 (0, 0),    new Vector2 (0, 1), 	     new Vector2 (1, 1), 			   new Vector2 (1, 0) };
		
		if ( meshRenderer != null) {
		
			Vector2 lLeftUV  = PixelCoordToUVCoord (new Vector2 (uvRect.x, uvRect.y));
			Vector2 UVDimen  = PixelSpaceToUVSpace (new Vector2 (uvRect.width, -uvRect.height));
	
			uvs [0] = lLeftUV + Vector2.up * UVDimen.y;		 		
			uvs [1] = lLeftUV;	 										
			uvs [2] = lLeftUV + Vector2.right * UVDimen.x;			
			uvs [3] = lLeftUV + UVDimen;
	
		}

		trias = new int[6] {0, 1, 3, 3, 1, 2};

	}
		
	public Vector2 PixelSpaceToUVSpace (Vector2 xy)
	{
		Texture t = meshRenderer.sharedMaterial.mainTexture;

		return new Vector2 (xy.x / ((float)t.width), xy.y / ((float)t.height));
	}

	public Vector2 PixelCoordToUVCoord (Vector2 xy)
	{
		Vector2 p = PixelSpaceToUVSpace (xy);
		p.y = 1.0f - p.y;
		return p;
	}
	
	public Vector3[] VERTS {
		get{ return verts; }	
	}
	
	public Vector2[] UVS {
		get{ return uvs; }	
	}
	
	public int[] TRIAS {
		get{ return trias; }	
	}

}

