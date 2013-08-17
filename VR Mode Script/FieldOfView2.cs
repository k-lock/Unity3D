using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class FieldOfView2 : MonoBehaviour
{
	
	public float FOV_width = 7f;
	public float FOV_length = 4f;
	protected Vector3[] verts = new Vector3[4];
	protected Vector2[] uvs = new Vector2[4];
	protected int[] trias = null;
	protected Color[] colors = new Color[4];
	[HideInInspector]
	private Mesh mesh = null;
	[HideInInspector]
	public MeshFilter meshFilter;
	[HideInInspector]
	public MeshRenderer meshRenderer;
	[HideInInspector]
	public Material mat = null;
	[HideInInspector]
	private MeshCollider meshCollider;
	private List<Vector2>  vertices2D;
	
	public Transform Hhmm;
	private Transform hhmm;

	
	void Awake ()
	{
	//	Hhmm = transform.Find("Hhmm") as Transform;
		MESH_init ();
	}

	void MESH_init ()
	{
		
		// Create Vector2 vertices
		vertices2D = new List<Vector2> ();
		vertices2D.Add (new Vector2 (0, 0));
		vertices2D.Add (new Vector2 (-FOV_length * .5f, FOV_width));		
		vertices2D.Add (new Vector2 (0, FOV_width));
		vertices2D.Add (new Vector2 (FOV_length * .5f, FOV_width));

        

 
		// Use the triangulator to get indices for creating triangles
		Triangulator tr = new Triangulator (vertices2D);
		trias = tr.Triangulate ();
 
		// Create the Vector3 vertices
		verts = new Vector3[vertices2D.Count];
		for (int i=0; i<verts.Length; i++) {
			verts [i] = new Vector3 (vertices2D [i].x, 0, vertices2D [i].y);
		}
 
		// Create the mesh
		mesh = new Mesh ();
		mesh.vertices = verts;
		mesh.triangles = trias;
		mesh.uv = vertices2D.ToArray ();
		
		mesh.RecalculateNormals ();
		mesh.RecalculateBounds ();
 
		if (mat == null) {		
			mat = new Material (Shader.Find ("Transparent/Diffuse"));
			//mat.mainTexture = texture;
			Color c = Color.green; c = new Color(c.r,c.g,c.b,.2f);
			mat.color = c; //new Color(1,.5f,0,.5f);
		}

		meshRenderer = (MeshRenderer)gameObject.GetComponent (typeof(MeshRenderer));
		if (meshRenderer == null)
			meshRenderer = (MeshRenderer)gameObject.AddComponent (typeof(MeshRenderer));

		meshFilter = (MeshFilter)gameObject.GetComponent (typeof(MeshFilter));
		if (meshFilter == null)
			meshFilter = (MeshFilter)gameObject.AddComponent (typeof(MeshFilter));
		meshFilter.mesh = mesh;
		meshRenderer.material = mat;
		
		meshCollider = (MeshCollider)gameObject.GetComponent (typeof(MeshCollider));
		if (meshCollider == null) {
			meshCollider = (MeshCollider)gameObject.AddComponent (typeof(MeshCollider));
		}
		meshCollider.sharedMesh = mesh;
		meshCollider.isTrigger = true;
	}

	void OnTriggerEnter (Collider collider)
	{
		
		Debug.Log (collider.transform.name);
		
		
		Color c = meshRenderer.material.color;
		if (collider.transform.name != "") {
			c = Color.red; 
			meshRenderer.material.color = new Color (c.r, c.g, c.b, .2f);
			
			
		} 
		
	
	}
	void OnTriggerExit (Collider collider)
	{
		
		Debug.Log (collider.transform.name);

		if (collider.transform.name != "") {

			Color c = Color.green; c = new Color(c.r,c.g,c.b,.2f);

			meshRenderer.material.color = c;
			
			
		} 
		
	
	}
	IEnumerator setGreenBack(){
		yield return new WaitForSeconds(2f);
		Color c = meshRenderer.material.color;
		c = Color.green; 
		meshRenderer.material.color = new Color (c.r, c.g, c.b, .2f);
		DestroyImmediate(hhmm);
	}
}
