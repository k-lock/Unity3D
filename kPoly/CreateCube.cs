using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

class CreateCube
{
    public static GameObject CreateMesh(
                                          string _meshName,
                                          /*int _uSegments,
                                          int _vSegments,*/
                                          float _width,
                                          float _height,
                                          float _deep
                                        /*  int _faceIndex,
                                          int _windinIndex,
                                          int _pivotIndex*/)
    {

        GameObject quad = new GameObject();

        if (!string.IsNullOrEmpty(_meshName))
            quad.name = _meshName;
        else
            quad.name = "kPolyCube";

        quad.transform.position = Vector3.zero;

        int vCount = 4 * 6;
        int tCount = 6;
        Vector3[] vertices = new Vector3[vCount];
        Vector2[] uvs = new Vector2[vCount];
        int[] triangles = new int[tCount];

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
		};
        int faces = 6; // here a face = 2 triangles
        
        List<int> trianglesL = new List<int>();
        List<Vector2> uvsL = new List<Vector2>();

        for (int i = 0; i < faces; i++)
        {
            int triangleOffset = i * 4;
            trianglesL.Add(0 + triangleOffset);
            trianglesL.Add(2 + triangleOffset);
            trianglesL.Add(1 + triangleOffset);

            trianglesL.Add(0 + triangleOffset);
            trianglesL.Add(3 + triangleOffset);
            trianglesL.Add(2 + triangleOffset);

            // same uvs for all faces
            uvsL.Add(new Vector2(0, 0));
            uvsL.Add(new Vector2(1, 0));
            uvsL.Add(new Vector2(1, 1));
            uvsL.Add(new Vector2(0, 1));
        }

        MeshFilter mf = quad.AddComponent<MeshFilter>();
        quad.AddComponent<MeshRenderer>();  
        Mesh m = new Mesh();

        m.name = quad.name;
        m.vertices = vertices;
        m.uv = uvsL.ToArray();
        m.triangles = trianglesL.ToArray();

        mf.sharedMesh = m;

        m.RecalculateNormals();
        m.RecalculateBounds();
        m.Optimize();

        return quad;
    }
}