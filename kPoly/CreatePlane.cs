using UnityEngine;
using UnityEditor;
using System.Collections;

public class CreatePlane
{
	/** Create plane mesh in a unity gameobject.
     * 
     *  @params string - A string value for the gameobject.name
     *  @params int - The count of horizontal segments.
     *  @params int - The count of vertical segments.
     *  @params float - The horizontal size for the created mesh.
     *  @params float - The vertical size for the created mesh.
     *  @params int - The index for the normal facing.
     *  @params int - The index for the triangle winding.
     *  
     *  @return GameObject - The generated object.
     */
	public static GameObject CreateMesh (string _meshName,
                                  int _uSegments,
                                  int _vSegments,
                                  float _width,
                                  float _height,
                                  int _faceIndex,
                                  int _windinIndex,
                                  int _pivotIndex,
								  int _colliderIndex)
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

		Vector2 pivot = PivotVector (_pivotIndex, _width, _height);
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
		
		AddCollider (_colliderIndex, quad, m);
		
		return quad;
	}
	/** Returns a vector2 containing the new position for the transform point of the generated mesh.*/
	private static Vector2 PivotVector
            (int _pivotIndex,
            float _width,
            float _height)
	{
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
	/** Add a collider object to the generated gameobject.
	 *  @params GameObject quad - The target gameobject.
	 * 	@params Mesh m - The sharedMesh property for a MeshCollider componete.
	 */
	public static void AddCollider (int colIndex, GameObject quad, Mesh m)
	{
		if (colIndex > 0) {
				
			switch (colIndex) {
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
}

