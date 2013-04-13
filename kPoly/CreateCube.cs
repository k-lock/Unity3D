using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

class CreateCube
{
	public static GameObject CreateMesh (
                                          string _meshName,
                                          int _uSegments,
                                          int _vSegments,
                                          int _zSegments,
                                          float _width,
                                          float _height,
                                          float _deep
                                        /*  int _faceIndex,
                                          int _windinIndex,
                                          int _pivotIndex*/)
	{

		GameObject quad = new GameObject ();

		if (!string.IsNullOrEmpty (_meshName))
			quad.name = _meshName;
		else
			quad.name = "kPolyCube";

		quad.transform.position = Vector3.zero;
        
		int xCount = _uSegments + 1;
		int yCount = _vSegments + 1;
		int zCount = _zSegments + 1;
		int vCount = 4 * 6;
		int tCount = 6;
		Vector3[] vertices = new Vector3[vCount];
		Vector2[] uvs = new Vector2[vCount];
		int[] triangles = new int[tCount];

		int index = 0;
		float xUV = 1.0f / _uSegments;
		float yUV = 1.0f / _vSegments;
		float zUV = 1.0f / _zSegments;
		float xSC = _width / _uSegments;
		float ySC = _height / _vSegments;
		float zSC = _height / _zSegments;

	/*	// Vector2 pivot = CreatePlane.PivotVector(_pivotIndex, _width, _height);
		for (float z = 0.0f; z<zCount; z++) {
			for (float y = 0.0f; y < yCount; y++) {
				for (float x = 0.0f; x < xCount; x++) {

					float dx = x * xSC - _width * .5f;// - pivot.x * .5f;
					float dy = y * ySC - _height * .5f;// - pivot.y * .5f;
					float dz = z * zSC - _deep * .5f;// - pivot.y * .5f;
					Vector3 vert = new Vector3( dx, dy, dz);
					
					vertices [index] = vert;
					uvs[index++] = new Vector2 (x * xUV, y * yUV);
				}
			}
		}*/

		for (int _faceIndex = 0; _faceIndex < 6; _faceIndex++) {
			for (float y = 0.0f; y < yCount; y++) {
				for (float x = 0.0f; x < xCount; x++) {

					float dx = x * xSC - _width * .5f;// - pivot.x * .5f;
					float dy = y * ySC - _height * .5f;// - pivot.y * .5f;
					float dz = y * zSC - _deep * .5f;// - pivot.y * .5f;
					
					Vector3 vert = Vector3.zero;
					if (_faceIndex == 0) {
						vert = new Vector3 (dx, _deep*.5f, dy); // xz
					} else if (_faceIndex == 1) {
						vert = new Vector3 (dx, dy, _deep*.5f); //xy
					} else if (_faceIndex == 2) {
						vert = new Vector3 (_deep*.5f, dy, dx); //zy
					} else if (_faceIndex == 3) {
						vert = new Vector3 (dy, -_deep*.5f, dx); //zx
					} else if (_faceIndex == 4) {
						vert = new Vector3 (dy, dx, -_deep*.5f); //yx
					} else if (_faceIndex == 5) {
						vert = new Vector3 (-_deep*.5f, dx, dy); //yz
					}

					vertices [index] = vert;
				//	Debug.Log(vert);
					uvs [index++] = new Vector2 (x * xUV, y * yUV);
				}
			}
		}
		/*	// xy,zy,xy,zy,zx,zx
		
        vertices = new Vector3[]{
			// face 1 (xy plane, z=0)
			new Vector3(0,0,0), 
			new Vector3(_width,0,0), 
			new Vector3(_width,_height,0), 
			new Vector3(0,_height,0), 
			// face 2 (zy plane, x=1)
			new Vector3(_width,0,0), 
			new Vector3(_width,0,_deep), 
			new Vector3(_width,_height,_deep), 
			new Vector3(_width,_height,0), 
			// face 3 (xy plane, z=1)
			new Vector3(_width,0,_deep), 
			new Vector3(0,0,_deep), 
			new Vector3(0,_height,_deep), 
			new Vector3(_width,_height,_deep), 
			// face 4 (zy plane, x=0)
			new Vector3(0,0,_deep), 
			new Vector3(0,0,0), 
			new Vector3(0,_height,0), 
			new Vector3(0,_height,_deep), 
			// face 5  (zx plane, y=1)
			new Vector3(0,_height,0), 
			new Vector3(_width,_height,0), 
			new Vector3(_width,_height,_deep), 
			new Vector3(0,_height,_deep), 
			// face 6 (zx plane, y=0)
			new Vector3(0,0,0), 
			new Vector3(0,0,_deep), 
			new Vector3(_width,0,_deep), 
			new Vector3(_width,0,0), 
		};*/
		int faces = 6; // here a face = 2 triangles
        
		List<int> trianglesL = new List<int> ();
		List<Vector2> uvsL = new List<Vector2> ();

		for (int i = 0; i < faces; i++) {
			int triangleOffset = i * 4;
			trianglesL.Add (0 + triangleOffset);
			trianglesL.Add (2 + triangleOffset);
			trianglesL.Add (1 + triangleOffset);

			trianglesL.Add (0 + triangleOffset);
			trianglesL.Add (3 + triangleOffset);
			trianglesL.Add (2 + triangleOffset);

			// same uvs for all faces
			uvsL.Add (new Vector2 (0, 0));
			uvsL.Add (new Vector2 (1, 0));
			uvsL.Add (new Vector2 (1, 1));
			uvsL.Add (new Vector2 (0, 1));
		}

		MeshFilter mf = quad.AddComponent<MeshFilter> ();
		quad.AddComponent<MeshRenderer> ();  
		Mesh m = new Mesh ();

		m.name = quad.name;
		m.vertices = vertices;
		m.uv = uvsL.ToArray ();
		m.triangles = trianglesL.ToArray ();

		mf.sharedMesh = m;

		m.RecalculateNormals ();
		m.RecalculateBounds ();
		m.Optimize ();
		
		CreatePlane.AddCollider (1, quad, m);

		return quad;
	}
	/* public static Vector3 PivotVector
        (int _pivotIndex,
        float _width,
        float _height,
        float _deep )
    {
        Vector3 p = Vector3.zero;
        switch (_pivotIndex)
        {
            case 0://TOP FRONT LEFT:
                p = new Vector3(-_width, _height, 0);
                break;
            case 1://TOP FRONT Center:
                p = new Vector3(0, _height, 0);
                break;
            case 2://TOP FRONT Right:
                p = new Vector3(_width, _height, 0);
                break;
            case 3://PIVOT FRONT Left:
                p = new Vector3(-_width, 0, 0);
                break;
            case 4://PIVOT FRONT Center:
                p = new Vector3(0, 0, _deep * .5f);
                break;
            case 5://TextAnchor.MiddleRight:
                p = new Vector2(_width, 0);
                break;
            case 6://TextAnchor.LowerLeft:
                p = new Vector2(-_width, -_height);
                break;
            case 7://TextAnchor.LowerCenter:
                p = new Vector2(0, -_height);
                break;
            case 8://TextAnchor.LowerRight:
                p = new Vector2(_width, -_height);
                break;
        }
        return p;

    }*/
}