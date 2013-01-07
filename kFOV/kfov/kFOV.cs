using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class kFOV : MonoBehaviour
{

	[Serializable]
	public class Set
	{
		public Vector3 pos;
	}
	public bool showDebug = true;
	public Set[] points;
	public float width = 7f;
	public float length = 4f;
	public float dpi = .05f;
	public float offset = .05f;
	public Color color1 = Color.green;
	public Color color2 = Color.red;
	private Mesh mesh = null;
	private Vector3[] vertices;
	private Color[] colors;
	private int[] triangles;
	private Material mat;
	private List<Vector2> vertices2D;
	#region UNITY
	
	#if UNITY_EDITOR
	void OnEnable ()
	{
		//HitUpdate ();
		
	}
	
	void OnDisable ()
	{
		if (Application.isEditor) {
			GetComponent<MeshFilter> ().mesh = null;
			DestroyImmediate (mesh);
		}
	}
	
	void Reset ()
	{	
		//EditorUpdate ();
	}
	#endif
	void Update ()
	{
		if (dpi <= 0)
			dpi = .05f;
		
		//	MeshUpdate ();
		HitUpdate ();
	}
	/*
	void OnTriggerEnter (Collider collider)
	{
		if (collider.transform.name == "Player") { 
			GetComponent<MeshRenderer> ().material.color = new Color (color2.r, color2.g, color2.b, .2f);
		} 
	}

	void OnTriggerStay (Collider collider)
	{
		
		HitUpdate ();
		
	}
	
	void OnTriggerExit (Collider collider)
	{
		//if (collider.transform.name == "Player") {
			GetComponent<MeshRenderer> ().material.color = new Color (color1.r, color1.g, color1.b, .2f);
		//} 	
		//ResetVerticies();
		//MeshUpdate();
	}*/
	#endregion

	void HitUpdate ()
	{
		Components ();
		
		Vector3[] verts = new Vector3[999];
		vertices2D = new List<Vector2> ();
		int counter = 1;
		
		verts [0] = transform.position;
		
		//left sector
		for (float i = 0; i<.5f; i+=dpi/10) {
			Vector3 point = transform.rotation * new Vector3 (-length * (.5f - i), 0, width) + transform.position;
			Ray ray = new Ray (transform.position, point - transform.position);
			
			if (showDebug)
				Debug.DrawRay (transform.position, point - transform.position, Color.black);
			
			RaycastHit rayHit = RayHiter (ray);
			
			if (rayHit.collider == null) {
				verts [counter] = point;
			} else {
				if (rayHit.collider.tag == "kFOV")
					verts [counter] = rayHit.point + offset * -new Ray (transform.position, point - transform.position).direction;
					else verts [counter] = point;
			}
			
			if (showDebug)
				Debug.DrawLine (transform.position, verts [counter], Color.white);
			counter++;
			
		}
		//right sector
		for (float i = 0; i>-.5f; i-=dpi/10) {
			Vector3 point = transform.rotation * new Vector3 (-length * i, 0, width) + transform.position;
			Ray ray = new Ray (transform.position, point - transform.position);
			
			if (showDebug)
				Debug.DrawRay (transform.position, point - transform.position, Color.black);
			
			RaycastHit rayHit = RayHiter (ray);
			
			if (rayHit.collider == null) {
				verts [counter] = point;
			} else {
				if (rayHit.collider.tag == "kFOV")
					verts [counter] = rayHit.point + offset * -new Ray (transform.position, point - transform.position).direction;
					else verts [counter] = point;
			}
			
			if (showDebug)
				Debug.DrawLine (transform.position, verts [counter], Color.white);
			counter++;
		}
		
		verts [counter] = verts [0];

		Matrix4x4 transformMatrix = transform.worldToLocalMatrix;
		vertices = new Vector3[counter];
		
		// convert verts && transform verts
		for (int k=0; k<counter; k++) {
			vertices [k] = transformMatrix.MultiplyPoint3x4 (verts [k]);
			vertices2D.Add (new Vector2 (vertices [k].x, vertices [k].z));	
		}	
		MeshUpdate ();
	}
	
	void MeshUpdate ()
	{
		// Use the triangulator to get indices for creating triangles
		Triangulator tr = new Triangulator (vertices2D);
		triangles = tr.Triangulate ();
			
		// Setup mesh
		mesh.Clear ();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = vertices2D.ToArray ();
	
		// Update mesh
		mesh.RecalculateBounds ();
		mesh.RecalculateNormals ();
	}
	
	RaycastHit RayHiter (Ray ray)
	{
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, width)) {
			return hit;
		}
		return hit;
	}

	public void EditorUpdate ()
	{
		Components ();
		MeshUpdate ();
	}

	void Components ()
	{	
		MeshRenderer meshRenderer = GetComponent<MeshRenderer> ();
		if (meshRenderer == null) {
			meshRenderer = gameObject.AddComponent<MeshRenderer> ();
		}
		MeshFilter meshFilter = GetComponent<MeshFilter> ();
		if (meshFilter == null) {
			meshFilter = gameObject.AddComponent<MeshFilter> ();
			meshRenderer.material = mat;
		}
		if (mesh == null) {
			GetComponent<MeshFilter> ().mesh = mesh = new Mesh ();
			mesh.name = "kMesh";
			mesh.hideFlags = HideFlags.HideAndDontSave;	
		}
		meshFilter.mesh = mesh;

		if (mat == null) {
			GetComponent<MeshRenderer> ().sharedMaterial = mat = new Material (Shader.Find ("Transparent/Diffuse"));	
		}
		color1.a = color2.a =.6f;
		mat.color = color1;

		MeshCollider meshCollider = GetComponent<MeshCollider> ();
		if (meshCollider == null) {
			meshCollider = gameObject.AddComponent<MeshCollider> ();
		}
		meshCollider.sharedMesh = mesh;
		meshCollider.isTrigger = true;

		Rigidbody rigid = GetComponent<Rigidbody> ();
		if (rigid == null) {
			rigid = gameObject.AddComponent<Rigidbody> ();
			rigid.useGravity = false;
		}	
	}
	
	void ResetVerticies ()
	{
		// Create Vector2 vertices
		vertices2D = new List<Vector2> ();
		vertices2D.Add (new Vector2 (0, 0));
		vertices2D.Add (new Vector2 (-length * .5f, width));		
		vertices2D.Add (new Vector2 (0, width));
		vertices2D.Add (new Vector2 (length * .5f, width));
		
		vertices = new Vector3[vertices2D.Count];
		for (int k=0; k<vertices2D.Count; k++) {
			vertices [k] = new Vector3(vertices2D[k].x, 0, vertices2D[k].y);
		}	
	}
}
