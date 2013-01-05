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
		//UpdateTile ();
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
		//UpdateTile ();
	}
	#endif
	void Update ()
	{
		TESTER2 ();
	}
	
	void TESTER2 ()
	{

		Components ();

		vertices2D = new List<Vector2> ();
		vertices2D.Add (new Vector2 (0, 0));
		

		
		bool hitter = false;
		
		for (float i = 0; i<.5f; i+=dpi) {
			
			Vector3 newPoint;
			Vector3 dirPoint = transform.rotation * new Vector3 (-length * (.5f - i), 0, width) + transform.position;
			
			Ray ray = new Ray (transform.position, dirPoint - transform.position);
			RaycastHit hit = RayHiter (ray);
			
			if (hit.collider == null) {
			
				Debug.DrawLine (transform.position, dirPoint, Color.cyan);
				dirPoint = dirPoint;
				vertices2D.Add (new Vector2 (dirPoint.x, dirPoint.z));
				
			} else {
				hitter = true;
				
				newPoint = hit.point;//+ offset * -ray.direction.normalized;
				newPoint = new Vector3 (newPoint.x, transform.position.y, newPoint.z);

				
				Debug.DrawLine (transform.position, newPoint, Color.red);
				vertices2D.Add (new Vector2 (newPoint.x, newPoint.z));
			}
				
		}
		
		if (!hitter) {
			vertices2D = new List<Vector2> ();
			vertices2D.Add (new Vector2 (0, 0));
			vertices2D.Add (new Vector2 (-length * .5f, width));		
			vertices2D.Add (new Vector2 (0, width));
			vertices2D.Add (new Vector2 (length * .5f, width));
		}
		
		// Use the triangulator to get indices for creating triangles
		Triangulator tr = new Triangulator (vertices2D);
		triangles = tr.Triangulate ();
 
		// Convert vertices - Vector2 in Vector3 
		vertices = new Vector3[vertices2D.Count];
		for (int i=0; i<vertices.Length; i++) {
			
			vertices [i] = new Vector3 (vertices2D [i].x, 0, vertices2D [i].y);
			
			if (!hitter) {
	
				if (i == 0)
					vertices [i] = (vertices [i]) + transform.localPosition;
				if (i > 0)
					Debug.DrawLine (transform.rotation * vertices [i - 1] + transform.position,
									transform.rotation * vertices [i] + transform.position,
									Color.magenta);	
				
				if (i > 0)
					vertices [i] = (transform.localRotation * vertices [i]);
		
			} else {
				
				if (i == 0)
					vertices [i] = (vertices [i]) + transform.position;
				
				if (i > 0)
					Debug.DrawLine (vertices [i - 1],
									vertices [i],
									Color.magenta);		
				
				vertices [i] = (vertices [i]) + transform.localPosition;//+mesh.vertices[0];	
			}	
		}
		Debug.DrawLine (transform.position, transform.rotation * new Vector3 (0, 0, width) + transform.position, Color.yellow);
		
		/*	
	// Setup mesh
		mesh.Clear();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = vertices2D.ToArray ();
			
		// Update mesh
		mesh.RecalculateNormals ();
		mesh.RecalculateBounds ();*/
	}
	
	void TESTER ()
	{
	
		
		Components ();
		
		List<Vector2> verticezz = new List<Vector2> ();
		List<Vector3> verticez = new List<Vector3> ();
		
		Vector3[] verts = new Vector3[100];
		verts [0] = new Vector3 (0, 0, 0);
		int counter = 0;
		for (float i = 0; i<.5f; i+=dpi/10f) {
			
			Vector3 newPoint;
			Vector3 dirPoint = //transform.TransformPoint (Vector3.forward * width + new Vector3 (-length * (.5f - i), 0, 0));
			//new Vector3 (-length* (.5f - i) , 0, width)+transform.position ;//+ transform.localEulerAngles;
			transform.rotation * new Vector3 (-length * (.5f - i), 0, width) + transform.position;
			
			
			Ray ray = new Ray (transform.position, dirPoint - transform.position);
			RaycastHit hit = RayHiter (ray);
			
			if (hit.collider == null) {
				//	Debug.DrawLine (transform.position, dirPoint, Color.yellow);
				//verticez.Add (dirPoint);
				verts [counter] = dirPoint;
			} else {

				newPoint = hit.point + offset * -new Ray (transform.position, transform.rotation * (dirPoint - transform.position).normalized).direction;
				newPoint = new Vector3 (newPoint.x, transform.position.y, newPoint.z);

				//		Debug.DrawLine (transform.position, newPoint, Color.red);
				
				///	verticez.Add (newPoint);
				verts [counter] = newPoint;
			}
			counter++;
		}
		
		Debug.Log (counter);
		Debug.DrawLine (transform.position, verts [0], Color.magenta);
		Debug.DrawLine (transform.position, verts [counter - 1], Color.magenta);
		
		List<Vector2> verties = new List<Vector2> ();
		
		for (int i=0; i<counter; i++) {
			
			if (i > 0)
				Debug.DrawLine (verts [i - 1], verts [i], Color.magenta);
			
			//	verticez[i] = verticez[i];
			verties.Add (new Vector2 (verts [i].x, verts [i].z));
		}
		
		// Use the triangulator to get indices for creating triangles
		Triangulator tr = new Triangulator (verties);
		triangles = tr.Triangulate ();
 		
		// Setup mesh
		mesh.Clear ();
		mesh.vertices = verts;
		mesh.triangles = triangles;
		mesh.uv = verties.ToArray ();
		
		// Update mesh
		mesh.RecalculateNormals ();
		mesh.RecalculateBounds ();
		
		 
	}
	
	
	/*void OnTriggerEnter (Collider collider)
	{
		if (dpi <= 0)
			dpi = .05f;

		Color c = GetComponent<MeshRenderer> ().material.color;

		if (collider.transform.name != "") {
			
			c = Color.red; 
			GetComponent<MeshRenderer> ().material.color = new Color (c.r, c.g, c.b, .2f);
		} 
	}
	void OnTriggerStay (Collider collider)
	{
		if (dpi <= 0) dpi = .05f;
		RayCheck();
		
	}
	
	void OnTriggerExit (Collider collider)
	{
		if (dpi <= 0)
			dpi = .05f;

		Color c = GetComponent<MeshRenderer> ().material.color;
		if (collider.transform.name != "") {
			
			c = Color.green; 
			GetComponent<MeshRenderer> ().material.color = new Color (c.r, c.g, c.b, .2f);
		} 
		ResetVerticies();
		UpdateTile ();
	}*/
	#endregion
	#region MESH 
	void RayCheck ()
	{
		
		Debug.Log (transform.rotation);
		// Reset vertices2D
		vertices2D = new List<Vector2> ();
		vertices2D.Add (new Vector2 (0, 0));
		
		// check the left side of the fov object
		for (float i = 0; i<.5f; i+=dpi/10f) {
			Vector3 newPoint;
			Vector3 dirPoint = //transform.TransformPoint (Vector3.forward * width + new Vector3 (-length * (.5f - i), 0, 0));
			//new Vector3 (-length* (.5f - i) , 0, width)+transform.position ;//+ transform.localEulerAngles;
			transform.rotation * new Vector3 (-length * (.5f - i), 0, width) + transform.position;
			
			//	Debug.DrawLine (transform.position, dirPoint, Color.yellow);
			
			Ray ray = new Ray (transform.position, dirPoint - transform.position);
			RaycastHit hit = RayHiter (ray);
			
			if (hit.collider != null) {
				
				newPoint = hit.point + offset * -new Ray (transform.position, transform.rotation * dirPoint - transform.position).direction;
				newPoint = new Vector3 (newPoint.x, transform.position.y, newPoint.z);
				
				Debug.DrawLine (transform.position, newPoint, Color.red);
				
				Vector3 cPoint = newPoint - transform.position;
				vertices2D.Add (new Vector2 (cPoint.x, cPoint.z));
			
				/*	for (float i2 = i; i2<i+.15f; i2+=(dpi/10)*.025f) {
				
					Vector3 np2 = new Vector3 (-length* (.5f - i2), 0, width);
					Ray ray2 = new Ray (transform.position, np2);
					RaycastHit hit2 = CheckRay (ray2);
					
					if(hit2.collider!=null){
						Debug.DrawRay (transform.position, (hit2.point + offset * -ray2.direction)-transform.position,Color.cyan);
						
					}else{
						newPoint = np2+transform.position;
						Debug.DrawLine (transform.position, np2+transform.position, Color.blue);
						i2=.5f;
					//	vertices2D.Add (new Vector2 (newPoint.x-transform.position.x, newPoint.z-transform.position.z));
					}
				}*/
			} else {
				newPoint = dirPoint;
				//	Debug.DrawLine (transform.position, newPoint, Color.yellow);
				vertices2D.Add (new Vector2 (newPoint.x - transform.position.x, newPoint.z - transform.position.z));
			}
		}
		/*	// check the right side of fov object
		for (float i = 0;  i>-.5f; i-=dpi/10f) {
			Vector3 newPoint;
			Vector3 dirPoint = transform.rotation*new Vector3 (length* -i, 0, width)+transform.position;
		//	Debug.DrawLine (transform.position, dirPoint, Color.yellow);
			
			Ray ray = new Ray (transform.position, dirPoint-transform.position);
			RaycastHit hit = RayHiter (ray);
			
			if (hit.collider != null) {
				
				newPoint = hit.point + offset * -ray.direction;
				newPoint = new Vector3(newPoint.x,transform.position.y,newPoint.z);
		//		Debug.DrawLine (transform.position, newPoint, Color.red);
				
				vertices2D.Add (new Vector2 (newPoint.x-transform.position.x, newPoint.z-transform.position.z));

			}else{
				newPoint = dirPoint;
		//		Debug.DrawLine (transform.position, newPoint, Color.yellow);
				vertices2D.Add (new Vector2 (newPoint.x-transform.position.x, newPoint.z-transform.position.z));
			}
		}*/
/*	//	
		
		vertices = new Vector3[vertices2D.Count];
		for (int i=0; i<vertices.Length; i++) {
			vertices [i] = new Vector3 (vertices2D [i].x, 0, vertices2D [i].y);
			if(i>0)Debug.DrawLine(vertices [i-1]+transform.position,vertices [i]+transform.position,Color.green);
		
		}*/
		//	Components();
		//	MeshCalc();
	}

	RaycastHit RayHiter (Ray ray)
	{
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, width)) {
			return hit;
		}
		return hit;
	}

	void ResetVerticies ()
	{
		// Create Vector2 vertices
		vertices2D = new List<Vector2> ();
		vertices2D.Add (new Vector2 (0, 0));
		vertices2D.Add (new Vector2 (-length * .5f, width));		
		vertices2D.Add (new Vector2 (0, width));
		vertices2D.Add (new Vector2 (length * .5f, width));
	}

	public void UpdateTile ()
	{
		Components ();
		MeshCalc ();
	}

	void MeshCalc ()
	{
		///	
		
		// Use the triangulator to get indices for creating triangles
		Triangulator tr = new Triangulator (vertices2D);
		triangles = tr.Triangulate ();
 
		// Convert vertices - Vector2 in Vector3 
		vertices = new Vector3[vertices2D.Count];
		for (int i=0; i<vertices.Length; i++) {
			vertices [i] = new Vector3 (vertices2D [i].x, 0, vertices2D [i].y);
			
			if (i > 0)
				Debug.DrawLine (vertices [i - 1] + transform.position, vertices [i] + transform.position, Color.magenta);
		
		}
		Debug.DrawLine (transform.position, transform.rotation * new Vector3 (0, 0, width) + transform.position, Color.yellow);
		
		
		// Setup mesh
		mesh.Clear ();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = vertices2D.ToArray ();
		
		// Update mesh
		mesh.RecalculateNormals ();
		mesh.RecalculateBounds ();
		
		//	transform.rotation = Quaternion.identity;
	}

	void Components ()
	{	
		if (vertices2D == null)
			ResetVerticies ();
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
		color1.a = .6f;
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
	#endregion
}
