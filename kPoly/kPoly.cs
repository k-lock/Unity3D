/** http://www.k-lock.de  | Paul Knab 
 * 	_______________________________________
 * 	
 * 	kPoly | V.1.0.1 | 05.04.2013
 *  ________________________________________
 * 
 * 	Class for mesh creation in Unity3D
 * 
 * 
 * */
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace klock.geometry
{
    class kPoly
    {
        #region MOUSE Ray HIT
        /** Static helper to create a ray from mouse position in scene view. [ GUIPointToWorldRay ]
         * 
         * 	@params returns Ray - */
        private static Ray HitRay()
        {
            Vector2 mp = Event.current.mousePosition;
            return HandleUtility.GUIPointToWorldRay(mp);
        }

        /** Returns the complete raycast hit object from the mouse position.
         * 
         * 	@returns RaycastHit hit.collider */
        public static RaycastHit HitTriangleObject()
        {
            Ray r = HitRay();
            RaycastHit hit;
            if (!Physics.Raycast(r, out hit, float.MaxValue))
            {
                return hit;
            }
            return hit;
        }
        /** Returns the hitting mesh collider from the mouse position.
         * 
         * 	@returns Collider hit.collider */
        public static Collider HitTriangleCollider()
        {
            Ray r = HitRay();
            RaycastHit hit;
            if (!Physics.Raycast(r, out hit, float.MaxValue))
            {
                return null;
            }
            return hit.collider;
        }
        /** Get the mesh triangle index from the mouse position
         * 	and the current selected mesh componente of the
         * 	selection.gameobject.
         * 
         * 	@returns int hit.triangleIndex */
        public static int HitTriangle()
        {
            Ray r = HitRay();
            RaycastHit hit;
            if (!Physics.Raycast(r, out hit, float.MaxValue))
            {
                return -1;
            }
            return hit.triangleIndex;
        }
        #endregion
        #region MeshFinder
        public static int[] TriangleIndicies(int p1, int p2, int[] tlist)
        {

            List<int> clist = new List<int>(tlist);
            List<int> dlist = new List<int>();
            for (int i = 0, n = clist.Count; i < n; i += 3)
            {
                bool i0 = (clist[i] == p1 || clist[i] == p2);
                bool i1 = (clist[i + 1] == p1 || clist[i + 1] == p2);
                bool i2 = (clist[i + 2] == p1 || clist[i + 2] == p2);

                if (i0 || i1 || i2) dlist.Add(i);
            }
            /* Debug.Log("________________________________");
             foreach (int i in clist)
             {
                 Debug.Log("->" + i);
             }*/
            /* List<int> dlist = clist.FindAll (x => x == p1 || x == p2);
             Debug.Log("________________________________" + p1 +" / " + p2);
             for (int i = 0, n = dlist.Count; i < n;i++ )
             {
                 if( clist[i] == p1
             }
  */
            //Debug.Log("________________________________");
            return dlist.ToArray();
        }

        /** Calculate the triangle index neighbour list. (Using Helper Class Tripoint) 
         * 
         * 	@returns TriPoint[] - A list containg the data to draw quads.*/
        public static TriPoint[] Neigbours(Mesh m)
        {
            int n = m.triangles.Length;
            TriPoint[] neigbourList = new TriPoint[n];

            for (int i = 0; i < n; i += 3)
            {

                int p1 = m.triangles[i];
                int p2 = m.triangles[i + 1];
                int p3 = m.triangles[i + 2];

                TriPoint tp = (p1 < n && neigbourList[p1] == null) ? new TriPoint() : neigbourList[p1];

                if (p2 + 1 != p3)
                {
                    tp._p2 = p2;
                    tp._p3 = p3;
                }
                else
                {
                    tp._p1 = p2;
                }
                neigbourList[p1] = tp;
            }
            return neigbourList;
        }
        public static int EdgeIndex(int p1, int p2, klock.kEditPoly.panels.KP_edit.Edge[] edges)
        {
            int r = -1;
            for (int i = 0, n = edges.Length; i < n; i++)
            {
                bool i1 = edges[i].vertexIndex[0] == p1 || edges[i].vertexIndex[0] == p2;
                bool i2 = edges[i].vertexIndex[1] == p1 || edges[i].vertexIndex[1] == p2;

                if (i1 && i2) 
                { 
                    r = i;  
                    break; 
                }
            }
            return r;
        }
        #endregion
        #region MESH INIT
        //---------------------------------------------------------------------------------- MESH 
        public static Mesh Create_Mesh(
                                                string _meshName,
                                                int _uSegments,
                                                int _vSegments,
                                                float _width,
                                                float _height,
                                                int _faceIndex = 0,
                                                int _windinIndex = 0,
                                                int _pivotIndex = 0/*,
                                                int _colliderIndex = 1*/
            )
        {

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

            Vector2 pivot = Pivot2D(_pivotIndex, _width, _height);

            for (float y = 0.0f; y < yCount; y++)
            {
                for (float x = 0.0f; x < xCount; x++)
                {

                    float dx = x * xSC - _width * .5f - pivot.x;
                    float dy = y * ySC - _height * .5f - pivot.y;

                    switch (_faceIndex)
                    {
                        case 0:
                            vertices[index] = new Vector3(dx, 0.0f, dy);
                            break;
                        case 1:
                            vertices[index] = new Vector3(dx, 0.0f, -dy);
                            break;

                        case 2:
                            vertices[index] = new Vector3(0.0f, dy, dx);
                            break;
                        case 3:
                            vertices[index] = new Vector3(0.0f, dy, -dx);
                            break;
                        case 4:
                            vertices[index] = new Vector3(dx, dy, 0.0f);
                            break;
                        case 5:
                            vertices[index] = new Vector3(-dx, dy, 0.0f);
                            break;
                    }

                    //vertices [index] = (_faceIndex == 0) ? new Vector3 (dx, 0.0f, dy) : new Vector3 (dx, dy, 0.0f);
                    uvs[index++] = new Vector2(x * xUV, y * yUV);
                }
            }

            index = 0;
            for (int y = 0; y < _vSegments; y++)
            {
                for (int x = 0; x < _uSegments; x++)
                {

                    int p1 = (y * xCount) + x;
                    int p2 = (y * xCount) + x + 1;
                    int p3 = ((y + 1) * xCount) + x;
                    int p4 = ((y + 1) * xCount) + x + 1;

                    switch (_windinIndex)
                    {
                        case 0:
                            triangles[index] = p3;
                            triangles[index + 1] = p2;
                            triangles[index + 2] = p1;

                            triangles[index + 3] = p3;
                            triangles[index + 4] = p4;
                            triangles[index + 5] = p2;
                            break;
                        case 1:
                            triangles[index] = p4;
                            triangles[index + 1] = p1;
                            triangles[index + 2] = p3;

                            triangles[index + 3] = p4;
                            triangles[index + 4] = p2;
                            triangles[index + 5] = p1;
                            break;
                        case 2:
                            triangles[index] = p1;
                            triangles[index + 1] = p4;
                            triangles[index + 2] = p2;

                            triangles[index + 3] = p1;
                            triangles[index + 4] = p3;
                            triangles[index + 5] = p4;
                            break;
                        case 3:
                            triangles[index] = p2;
                            triangles[index + 1] = p3;
                            triangles[index + 2] = p4;

                            triangles[index + 3] = p2;
                            triangles[index + 4] = p1;
                            triangles[index + 5] = p3;
                            break;
                    }
                    index += 6;
                }
            }
            Mesh m = new Mesh();
            m.name = _meshName;
            m.vertices = vertices;
            m.uv = uvs;
            m.triangles = triangles;
            /*if (norm != null)
                m.normals = norm;
            if (color != null)
                m.colors = color;
*/
            m.RecalculateNormals();
            m.RecalculateBounds();
            m.Optimize();
            return m;
        }
        #endregion
        #region CONE CREATION
        public static Mesh Create_Cone(string name,
                int numVertices = 10,
                float radiusTop = 0f,
                float radiusBottom = 1f,
                float length = 1f,
                float openingAngle = 0f,
                bool outside = true,
                bool inside = false
            )
        {
            if (openingAngle > 0 && openingAngle < 180)
            {
                radiusTop = 0;
                radiusBottom = length * Mathf.Tan(openingAngle * Mathf.Deg2Rad / 2);
            }
            string meshName = ((name != "") ? name : "kPolyCone");
            Mesh mesh = new Mesh();
            mesh.name = meshName;
            // can't access Camera.current
            //newCone.transform.position = Camera.current.transform.position + Camera.current.transform.forward * 5.0f;
            int multiplier = (outside ? 1 : 0) + (inside ? 1 : 0);
            int offset = (outside && inside ? 2 * numVertices : 0);
            Vector3[] vertices = new Vector3[2 * multiplier * numVertices]; // 0..n-1: top, n..2n-1: bottom
            Vector3[] normals = new Vector3[2 * multiplier * numVertices];
            Vector2[] uvs = new Vector2[2 * multiplier * numVertices];
            int[] tris;
            float slope = Mathf.Atan((radiusBottom - radiusTop) / length); // (rad difference)/height
            float slopeSin = Mathf.Sin(slope);
            float slopeCos = Mathf.Cos(slope);
            int i;

            for (i = 0; i < numVertices; i++)
            {
                float angle = 2 * Mathf.PI * i / numVertices;
                float angleSin = Mathf.Sin(angle);
                float angleCos = Mathf.Cos(angle);
                float angleHalf = 2 * Mathf.PI * (i + 0.5f) / numVertices; // for degenerated normals at cone tips
                float angleHalfSin = Mathf.Sin(angleHalf);
                float angleHalfCos = Mathf.Cos(angleHalf);

                vertices[i] = new Vector3(radiusTop * angleCos, radiusTop * angleSin, 0);
                vertices[i + numVertices] = new Vector3(radiusBottom * angleCos, radiusBottom * angleSin, length);

                if (radiusTop == 0)
                    normals[i] = new Vector3(angleHalfCos * slopeCos, angleHalfSin * slopeCos, -slopeSin);
                else
                    normals[i] = new Vector3(angleCos * slopeCos, angleSin * slopeCos, -slopeSin);
                if (radiusBottom == 0)
                    normals[i + numVertices] = new Vector3(angleHalfCos * slopeCos, angleHalfSin * slopeCos, -slopeSin);
                else
                    normals[i + numVertices] = new Vector3(angleCos * slopeCos, angleSin * slopeCos, -slopeSin);

                uvs[i] = new Vector2(1.0f * i / numVertices, 1);
                uvs[i + numVertices] = new Vector2(1.0f * i / numVertices, 0);

                if (outside && inside)
                {
                    // vertices and uvs are identical on inside and outside, so just copy
                    vertices[i + 2 * numVertices] = vertices[i];
                    vertices[i + 3 * numVertices] = vertices[i + numVertices];
                    uvs[i + 2 * numVertices] = uvs[i];
                    uvs[i + 3 * numVertices] = uvs[i + numVertices];
                }
                if (inside)
                {
                    // invert normals
                    normals[i + offset] = -normals[i];
                    normals[i + numVertices + offset] = -normals[i + numVertices];
                }
            }
            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.uv = uvs;

            // create triangles
            // here we need to take care of point order, depending on inside and outside
            int cnt = 0;
            if (radiusTop == 0)
            {
                // top cone
                tris = new int[numVertices * 3 * multiplier];
                if (outside)
                    for (i = 0; i < numVertices; i++)
                    {
                        tris[cnt++] = i + numVertices;
                        tris[cnt++] = i;
                        if (i == numVertices - 1)
                            tris[cnt++] = numVertices;
                        else
                            tris[cnt++] = i + 1 + numVertices;
                    }
                if (inside)
                    for (i = offset; i < numVertices + offset; i++)
                    {
                        tris[cnt++] = i;
                        tris[cnt++] = i + numVertices;
                        if (i == numVertices - 1 + offset)
                            tris[cnt++] = numVertices + offset;
                        else
                            tris[cnt++] = i + 1 + numVertices;
                    }
            }
            else if (radiusBottom == 0)
            {
                // bottom cone
                tris = new int[numVertices * 3 * multiplier];
                if (outside)
                    for (i = 0; i < numVertices; i++)
                    {
                        tris[cnt++] = i;
                        if (i == numVertices - 1)
                            tris[cnt++] = 0;
                        else
                            tris[cnt++] = i + 1;
                        tris[cnt++] = i + numVertices;
                    }
                if (inside)
                    for (i = offset; i < numVertices + offset; i++)
                    {
                        if (i == numVertices - 1 + offset)
                            tris[cnt++] = offset;
                        else
                            tris[cnt++] = i + 1;
                        tris[cnt++] = i;
                        tris[cnt++] = i + numVertices;
                    }
            }
            else
            {
                // truncated cone
                tris = new int[numVertices * 6 * multiplier];
                if (outside)
                    for (i = 0; i < numVertices; i++)
                    {
                        int ip1 = i + 1;
                        if (ip1 == numVertices)
                            ip1 = 0;

                        tris[cnt++] = i;
                        tris[cnt++] = ip1;
                        tris[cnt++] = i + numVertices;

                        tris[cnt++] = ip1 + numVertices;
                        tris[cnt++] = i + numVertices;
                        tris[cnt++] = ip1;
                    }
                if (inside)
                    for (i = offset; i < numVertices + offset; i++)
                    {
                        int ip1 = i + 1;
                        if (ip1 == numVertices + offset)
                            ip1 = offset;

                        tris[cnt++] = ip1;
                        tris[cnt++] = i;
                        tris[cnt++] = i + numVertices;

                        tris[cnt++] = i + numVertices;
                        tris[cnt++] = ip1 + numVertices;
                        tris[cnt++] = ip1;
                    }
            }
            mesh.triangles = tris;

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            //mesh.Optimize();
            return mesh;
        }
        public static GameObject Create_Cone_Object(string name, int numVertices = 10,
                float radiusTop = 0f,
                float radiusBottom = 1f,
                float length = 1f,
                float openingAngle = 0f,
                bool outside = true,
                bool inside = false,
                 int colliderIndex = 1)
        {
            name = (name != "") ? name : "kPolyCube";
            Mesh m = Create_Cone(name, numVertices, radiusTop, radiusBottom, length, openingAngle, outside, inside);
            GameObject quad = new GameObject(name);
            ((MeshFilter)quad.AddComponent<MeshFilter>()).sharedMesh = m;
            quad.AddComponent<MeshRenderer>();

            Add_Collider(colliderIndex, quad, m);
            Debug.Log(m.subMeshCount);
            return quad;
        }
        #endregion
        #region CUBE CREATION
        public static Mesh Create_Cube(string name,
            int xseg, int yseg, int zseg,
            float width, float height, float depth,
            int _windingIndex = 0, int _pivotIndex = 0, int _colliderIndex = 1)
        {
            Mesh m = new Mesh();
            List<Vector3> verts = new List<Vector3>();
            List<int> trias = new List<int>();
            List<Vector2> uvs = new List<Vector2>();
            Mesh dm = null;
            for (int fi = 0; fi < 6; fi++)
            {
                //if (dm != null) dm.Clear();
                //m = new Mesh();
                switch (fi)
                {
                    case 0:
                        //vertices[index] = new Vector3(dx, 0.0f, dy);
                        dm = Create_Mesh(name, xseg, zseg, width, depth, fi);
                        break;
                    case 1:
                        //vertices[index] = new Vector3(dx, 0.0f, -dy);
                        dm = Create_Mesh(name, xseg, zseg, width, depth, fi);
                        break;

                    case 2:
                        dm = Create_Mesh(name, xseg, yseg, width, height, fi);
                        //vertices[index] = new Vector3(0.0f, dy, dx);
                        break;
                    case 3:
                        dm = Create_Mesh(name, xseg, yseg, width, height, fi);
                        //vertices[index] = new Vector3(0.0f, dy, -dx);
                        break;
                    case 4:
                        dm = Create_Mesh(name, zseg, yseg, depth, height, fi);
                        //vertices[index] = new Vector3(dx, dy, 0.0f);
                        break;
                    case 5:
                        dm = Create_Mesh(name, zseg, yseg, depth, height, fi);
                        //vertices[index] = new Vector3(-dx, dy, 0.0f);
                        break;
                }

                //dm = Create_Mesh(name, xseg, yseg, width, height, fi);

                verts.AddRange(dm.vertices);
                trias.AddRange(dm.triangles);
                uvs.AddRange(dm.uv);

                //       m.SetTriangles(dm.triangles, fi);
            }

            //m = new Mesh();
            m.name = name;
            m.subMeshCount = 6;
            m.vertices = verts.ToArray();
            m.uv = uvs.ToArray();
            //       m.triangles = trias.ToArray();
            /*if (norm != null)
                m.normals = norm;
            if (color != null)
                m.colors = color;
            */

            for (int fi = 0; fi < 6; fi++)
            {
                Debug.Log("Smc : " + m.subMeshCount);
                List<int> tri = trias.GetRange(fi, 6);
                m.SetTriangles(tri.ToArray(), fi);
                m.SetIndices(m.GetIndices(fi), MeshTopology.Triangles, fi);
            }


            m.RecalculateNormals();
            m.RecalculateBounds();
            //    m.Optimize();

            return m;
        }

        public static GameObject Create_Cube_Object(string name, int xseg, int yseg, int zseg,
                                                        float width, float height, float depth,
                                                        int windingIndex = 0, int pivotIndex = 0, int colliderIndex = 1)
        {
            name = (name != "") ? name : "kPolyCube";
            Mesh m = Create_Cube(name, xseg, yseg, zseg, width, height, depth, windingIndex, pivotIndex, colliderIndex);
            GameObject quad = new GameObject(name);
            ((MeshFilter)quad.AddComponent<MeshFilter>()).sharedMesh = m;
            quad.AddComponent<MeshRenderer>();

            Add_Collider(colliderIndex, quad, m);
            Debug.Log(m.subMeshCount);
            return quad;
        }
        #endregion
        #region PLANE CREATION
        public static Mesh Create_Plane(string name, int xseg, int yseg,
                                        float width, float height,
                                        int facingIndex = 0, int windingIndex = 0, int pivotIndex = 0)
        {
            return Create_Mesh(name, xseg, yseg, width, height, facingIndex, windingIndex, pivotIndex);

        }

        public static GameObject Create_Plane_Object(string name, int xseg, int yseg,
                                        float width, float height,
                                        int facingIndex = 0, int windingIndex = 0, int pivotIndex = 0, int colliderIndex = 1)
        {
            name = (name != "") ? name : "kPolyPlane";
            Mesh m = Create_Plane(name, xseg, yseg, width, height, facingIndex, windingIndex, pivotIndex);
            GameObject quad = new GameObject(name);
            ((MeshFilter)quad.AddComponent<MeshFilter>()).sharedMesh = m;
            quad.AddComponent<MeshRenderer>();

            Add_Collider(colliderIndex, quad, m);

            return quad;
        }
        #endregion
        #region COLLIDER
        //---------------------------------------------------------------------------------- COLLIDER INIT
        /** Add a collider object to the generated gameobject.
         *  @params     int         colIndex    - The index for the collider object [ 1-MeshCollider, 2-BoxCollider]
         *  @params     GameObject  quad        - The target gameobject.
         * 	@params     Mesh        m           - The sharedMesh property for a MeshCollider componete.                       */
        public static void Add_Collider(int colIndex, GameObject quad, Mesh m)
        {
            if (colIndex == 0) return;
            switch (colIndex)
            {
                case 1: // mesh 
                    MeshCollider mc = quad.AddComponent<MeshCollider>();
                    mc.sharedMesh = m;
                    break;
                case 2: // box
                    quad.AddComponent<BoxCollider>();
                    break;
            }
        }
        #endregion
        #region FACING
        //---------------------------------------------------------------------------------- FACE OBJECT

        public static string[] FACING = new string[6] { "TOP", "BUTTOM", "FRONT", "BACK", "LEFT", "RIGHT" };

        public static Vector3 Facing(int facingIndex)
        {
            Vector3 p = Vector3.zero;
            switch (facingIndex)
            {
                case 0:
                    p = Vector3.forward;
                    break;
                case 1:
                    p = -Vector3.forward;
                    break;

                case 2:
                    p = Vector3.up;
                    break;
                case 3:
                    p = -Vector3.up;
                    break;

                case 4:
                    p = Vector3.right;
                    break;
                case 5:
                    p = -Vector3.right;
                    break;
            }

            return p;
        }
        #endregion
        #region PIVOT
        //---------------------------------------------------------------------------------- PIVOT OBJECT
        public static string[] PIVOT3D = new string[27] {    "FTL","FTC","FTR", 
                                                            "FML","FMC","FMR",
                                                            "FBL","FBC","FBR",
                                                            "CTL","CTC","CTR", 
                                                            "CML","CMC","CMR",
                                                            "CBL","CBC","CBR",
                                                            "RTL","RTC","RTR", 
                                                            "RML","RMC","RMR",
                                                            "RBL","RBC","RBR" };

        public static Vector3 Pivot3D(int pivotIndex = 0, float width = 1.0f, float height = 1.0f, float depth = 1.0f)
        {
            Vector3 p = Vector3.zero;
            width = width * .5f;
            height = height * .5f;
            depth = depth * .5f;
            switch (pivotIndex)
            {
                case 0://FRONT.UpperLeft:
                    p = new Vector3(-width, height, depth);
                    break;
                case 1://FRONT.UpperCenter:
                    p = new Vector3(0, height, depth);
                    break;
                case 2://FRONT.UpperRight:
                    p = new Vector3(width, height, depth);
                    break;
                case 3://FRONT.MiddleLeft:
                    p = new Vector3(-width, 0, depth);
                    break;
                case 4://FRONT.MiddleCenter:
                    p = new Vector3(0, 0, depth);
                    break;
                case 5://FRONT.MiddleRight:
                    p = new Vector3(width, 0, depth);
                    break;
                case 6://FRONT.LowerLeft:
                    p = new Vector3(-width, -height, depth);
                    break;
                case 7://FRONT.LowerCenter:
                    p = new Vector3(0, -height, depth);
                    break;
                case 8://FRONT.LowerRight:
                    p = new Vector3(width, -height, depth);
                    break;
                // MIDDLE----------------------------------------
                case 9://MIDDLE.UpperLeft:
                    p = new Vector3(-width, height, 0);
                    break;
                case 10://MIDDLE.UpperCenter:
                    p = new Vector3(0, height, 0);
                    break;
                case 11://MIDDLE.UpperRight:
                    p = new Vector3(width, height, 0);
                    break;
                case 12://MIDDLE.MiddleLeft:
                    p = new Vector3(-width, 0, 0);
                    break;
                case 13://MIDDLE.MiddleCenter:
                    p = Vector3.zero;
                    break;
                case 14://MIDDLE.MiddleRight:
                    p = new Vector3(width, 0, 0);
                    break;
                case 15://MIDDLE.LowerLeft:
                    p = new Vector3(-width, -height, 0);
                    break;
                case 16://MIDDLE.LowerCenter:
                    p = new Vector3(0, -height, 0);
                    break;
                case 17://MIDDLE.LowerRight:
                    p = new Vector3(width, -height, 0);
                    break;
                // REAR----------------------------------------
                case 18://REAR.UpperLeft:
                    p = new Vector3(-width, height, -depth);
                    break;
                case 19://REAR.UpperCenter:
                    p = new Vector3(0, height, -depth);
                    break;
                case 20://REAR.UpperRight:
                    p = new Vector3(width, height, -depth);
                    break;
                case 21://REAR.MiddleLeft:
                    p = new Vector3(-width, 0, -depth);
                    break;
                case 22://REAR.MiddleCenter:
                    p = new Vector3(0, 0, -depth);
                    break;
                case 23://REAR.MiddleRight:
                    p = new Vector3(width, 0, -depth);
                    break;
                case 24://REAR.LowerLeft:
                    p = new Vector3(-width, -height, -depth);
                    break;
                case 25://REAR.LowerCenter:
                    p = new Vector3(0, -height, -depth);
                    break;
                case 26://REAR.LowerRight:
                    p = new Vector3(width, -height, -depth);
                    break;
            }
            return p;
        }
        /*
         TL , TC, TR
         ML , MC, MR
         BL , BC, BR
         * */
        public static string[] PIVOT2D = new string[9] { "TL", "TC", "TR", "ML", "MC", "MR", "BL", "BC", "BR" };

        public static Vector2 Pivot2D(int pivotIndex = 0, float width = 1.0f, float height = 1.0f)
        {
            Vector2 p = Vector2.zero;
            width = width * .5f;
            height = height * .5f;
            switch (pivotIndex)
            {
                case 0://TextAnchor.UpperLeft:
                    p = new Vector2(-width, height);
                    break;
                case 1://TextAnchor.UpperCenter:
                    p = new Vector2(0, height);
                    break;
                case 2://TextAnchor.UpperRight:
                    p = new Vector2(width, height);
                    break;
                case 3://TextAnchor.MiddleLeft:
                    p = new Vector2(-width, 0);
                    break;
                case 4://TextAnchor.MiddleCenter:
                    p = Vector2.zero;
                    break;
                case 5://TextAnchor.MiddleRight:
                    p = new Vector2(width, 0);
                    break;
                case 6://TextAnchor.LowerLeft:
                    p = new Vector2(-width, -height);
                    break;
                case 7://TextAnchor.LowerCenter:
                    p = new Vector2(0, -height);
                    break;
                case 8://TextAnchor.LowerRight:
                    p = new Vector2(width, -height);
                    break;
            }
            return p;
        }
        #endregion
        #region MAKE PLANAR
        public static Mesh Planar(Mesh m, bool x = false, bool y = false, bool z = false)
        {
            Debug.Log("Flatten Mesh : " + m.name);
            Vector3[] verts = m.vertices;
            float dx = 0,
                    dy = 0,
                    dz = 0;
            for (int i = 0; i < m.vertexCount; i++)
            {
                dx += verts[i].x;
                dy += verts[i].y;
                dz += verts[i].z;
            }
            dx = dx / m.vertexCount;
            dy = dy / m.vertexCount;
            dz = dz / m.vertexCount;
            // Debug.Log("Flatten Mesh : " + ((x) ? dx : -1) + " " + ((y) ? dy : -1) + " " + ((z) ? dz : -1));
            for (int i = 0; i < m.vertexCount; i++)
            {
                //Debug.Log("vert "+ i + " Before : " +  m.vertices[i].x + " "+  m.vertices[i].y + " "+  m.vertices[i].z);
                verts[i] = new Vector3(
                    (x) ? dx : verts[i].x,
                    (y) ? dy : verts[i].y,
                    (z) ? dz : verts[i].z);
                //Debug.Log("vert "+ i + "after : " +  m.vertices[i].x + " "+  m.vertices[i].y + " "+  m.vertices[i].z);

            }
            //  if (updateMesh)
            //   {

            //   }
            m.vertices = verts;
            return m;
        }
        #endregion
    }
}
#region HELPERZ
/** TriPoint -----------------------------------------------------------------------------------------
 * 
 * 	Paul Knab - k-lock.de - 08.04.2013
 * 	___________________________________
 * 
 * 	
 * 	Helper to define the data of an pair of triangles.
 * 
 */
public class TriPoint
{
    public int _p1, _p2, _p3 = -1;

    public TriPoint Init(int p1 = -1, int p2 = -1, int p3 = -1)
    {
        if (p1 != -1)
            _p1 = p1;
        if (p2 != -1)
            _p2 = p2;
        if (p3 != -1)
            _p3 = p3;

        return this;
    }
    /** Debug Helper.
     * 
     * 	@returns string - A string containing the values of all points.
     * 
     * */
    public string Trace()
    {
        return this._p1 + " " + this._p2 + " " + this._p3;
    }
}

/** EDITOR - STATE MODES -------------------------------------------------------------------------------
 *
 *	Editor state mode enum struct.
 *
 */
public enum MODE
{
    None,
    Point,
    Edge,
    Triangle,
    Border,
    Quad,
    All

}
#endregion