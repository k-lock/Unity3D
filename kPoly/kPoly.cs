/**
 * Mesh Creation Helper 
 * V1.0.0
 */
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
        /** Calculate the triangle index neighbour list.
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
        #endregion
        #region MESH INIT
        //---------------------------------------------------------------------------------- MESH 
        public static Mesh Create_Mesh(
                                                string _meshName,
                                                int _uSegments,
                                                int _vSegments,
                                                float _width,
                                                float _height,
                                                int _faceIndex,
                                                int _windinIndex,
                                                int _pivotIndex,
                                                int _colliderIndex)
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

                    float dx = x * xSC - _width * .5f - pivot.x * .5f;
                    float dy = y * ySC - _height * .5f - pivot.y * .5f;

                    vertices[index] = (_faceIndex == 0) ? new Vector3(dx, 0.0f, dy) : new Vector3(dx, dy, 0.0f);
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
        #region CUBE CREATION
        public static Mesh Create_Cube(string name, int xseg, int yseg, int zseg, float width, float height, float depth)
        {
            return new Mesh();
        }
        #endregion
        #region PLANE CREATION
        public static Mesh Create_Plane(string name, int xseg, int yseg,
                                        float width, float height,
                                        int _facingIndex = 0, int _windingIndex = 0, int _pivotIndex = 0, int _colliderIndex = 1)
        {
            return Create_Mesh(name, xseg, yseg, width, height, _facingIndex, _windingIndex, _pivotIndex, _colliderIndex);

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
            if (colIndex > 0)
            {
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
                case 0: p = Vector3.forward; break;
                case 1: p = -Vector3.forward; break;

                case 2: p = Vector3.up; break;
                case 3: p = -Vector3.up; break;

                case 4: p = Vector3.right; break;
                case 5: p = -Vector3.right; break;
            }

            return Vector3.zero;
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
    E_Point,
    E_Line,
    E_Quad,
    E_All,

}
#endregion