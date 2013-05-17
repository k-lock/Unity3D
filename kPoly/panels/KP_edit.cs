//--------------------------------------------------------------------------------------------------------------------------------------//
//                                                                                                                                      //
//     PANEL EDIT                                                                                                                       //
//                                                                                                                                      //
//--------------------------------------------------------------------------------------------------------------------------------------//
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;


using klock.kEditPoly.helper;

namespace klock.kEditPoly.panels
{
    public class KP_edit
    {

        static bool FOLD_selection = true;
        //    static bool FOLD_object = true;
        static bool FOLD_verts = true;
        static bool FOLD_geome = true;
        public static GameObject _selection = null;
        public static bool _freeze = false;
        public static bool ANY_KEY = false;
        public static bool _dragCreate = false;

        public static PLANAR_HELPER planarHelp = new PLANAR_HELPER();
        public static MODE _editorMode = MODE.None;

        //  static int triIndex = -1;
        public static List<int> curPointIndex = new List<int>();

        static Vector3[] verts;
        public static int TOOL_INDEX = -1;

        public static Face[] faces = null;
        public static Edge[] edges = null;
        static Mesh _selectMesh = null;
        static List<int> toolVerts = new List<int>();

        private static GUIStyle selection_Style = null;
        private static void Selection_Style()
        {
            selection_Style = new GUIStyle();
            selection_Style.normal.background = EditorGUIUtility.whiteTexture;
            //   selection_Style.active.background = GUI.skin.box.active.background;
            selection_Style.contentOffset = new Vector2(7, 0);
        }
        public static void DRAW_PANEL()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.BeginVertical();
            GUILayout.Space(5);

            Color editorGUIback = new Color(.76f, .76f, .76f);
            if (selection_Style == null) Selection_Style();
            if (_selection == null) _selection = kSelect.OBJECT;
            Mesh _selectMesh = kSelect.MESH;
            //MODE modeTemp = _editorMode;
            GUI.enabled = (_selection != null && _selectMesh != null);
            //-------------------------------------------------------------------
            //  SELECTION
            float bw = 50,
                    bh = 30;
            FOLD_selection = EditorGUILayout.Foldout(FOLD_selection, "Selection " + _editorMode.ToString());// + ((_selection != null) ? "[ " + _selection.name + " ]" : ""));
            if (FOLD_selection)
            {
                bool pressed = false;
                EditorGUILayout.BeginVertical();
                GUILayout.Space(2);
                GUILayout.BeginHorizontal();
                GUI.color = (_editorMode == MODE.Point) ? Color.yellow : editorGUIback;
                if (GUILayout.Button(new GUIContent(kLibary.LoadBitmap("points", 25, 25)), selection_Style, GUILayout.Width(bw), GUILayout.Height(bh)))
                {
                    _editorMode = (_editorMode == MODE.Point) ? MODE.None : MODE.Point;
                    pressed = true;
                }
                GUI.color = Color.white;
                GUI.color = (_editorMode == MODE.Edge) ? Color.yellow : editorGUIback;
                if (GUILayout.Button(new GUIContent(kLibary.LoadBitmap("edge", 25, 25)), selection_Style, GUILayout.Width(bw), GUILayout.Height(bh)))
                {
                    _editorMode = (_editorMode == MODE.Edge) ? MODE.None : MODE.Edge;
                    pressed = true;
                }
                GUI.color = Color.white;
                GUI.color = (_editorMode == MODE.Triangle) ? Color.yellow : editorGUIback;
                if (GUILayout.Button(new GUIContent(kLibary.LoadBitmap("tri", 25, 25)), selection_Style, GUILayout.Width(bw), GUILayout.Height(bh)))
                {
                    _editorMode = (_editorMode == MODE.Triangle) ? MODE.None : MODE.Triangle;
                    pressed = true;
                }
                GUI.color = Color.white;
                GUI.color = (_editorMode == MODE.Quad) ? Color.yellow : editorGUIback;
                if (GUILayout.Button(new GUIContent(kLibary.LoadBitmap("quad", 25, 25)), selection_Style, GUILayout.Width(bw), GUILayout.Height(bh)))
                {
                    _editorMode = (_editorMode == MODE.Quad) ? MODE.None : MODE.Quad;
                    pressed = true;
                }
                GUI.color = Color.white;
                GUI.color = (_editorMode == MODE.All) ? Color.yellow : editorGUIback;
                if (GUILayout.Button(new GUIContent(kLibary.LoadBitmap("sub", 25, 25)), selection_Style, GUILayout.Width(bw), GUILayout.Height(bh)))
                {
                    _editorMode = (_editorMode == MODE.All) ? MODE.None : MODE.All;
                    pressed = true;
                }
                GUI.color = Color.white;
                GUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                if (pressed)
                {
                    // if (_selection != null )
                    // {
                    _selection = kSelect.OBJECT;
                    _freeze = (_editorMode != MODE.None) ? true : false;

                    if (_editorMode != MODE.None)
                    {
                        _selection.hideFlags |= HideFlags.NotEditable;
                        kPoly2Tool.SceneEvent(true);
                    }
                    else
                    {
                        _selection.hideFlags = 0;
                        kPoly2Tool.SceneEvent(false);
                    }
                    curPointIndex.Clear();
                    TOOL_INDEX = -1;
                    // if (toolVerts.Count > 0) 
                    //       toolVerts.Clear();
                    //  }
                }

                //EditorGUILayout.BeginHorizontal();
                //EditorGUILayout.PrefixLabel("" + _editorMode.ToString());
                // EditorGUILayout.LabelField(" " + (_editorMode != MODE.None && curPointIndex.Count > 0 ? curPointIndex[0] + " " : ""), GUILayout.ExpandWidth(true));
                //EditorGUILayout.EndHorizontal();
            }

            //-------------------------------------------------------------------
            //  VERTS 
            FOLD_verts = EditorGUILayout.Foldout(FOLD_verts, "Edit Vertices");
            if (FOLD_verts)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button(new GUIContent("Remove"), EditorStyles.toolbarButton)  )
                {
                    VerticesRemover();
                }
                if (GUILayout.Button(new GUIContent("Break"), EditorStyles.toolbarButton)) kPoly.VerticesBreak(_selectMesh, curPointIndex.ToArray());
                if (GUILayout.Button(new GUIContent("Turn"), EditorStyles.toolbarButton)) kPoly.TriangleTurn(_selectMesh, curPointIndex);
                //GUILayout.Space(10);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Button(new GUIContent("Extrude"), EditorStyles.toolbarButton);
                GUILayout.Button(new GUIContent("P"), EditorStyles.toolbarButton, GUILayout.Width(25));
                GUILayout.Space(5);
                if (GUILayout.Button(new GUIContent("Weld"), EditorStyles.toolbarButton))
                {
                    TOOL_INDEX = 1;

                    ModifiVerticies_points(true);

                    TOOL_INDEX = -1;
                }
                GUI.color = (TOOL_INDEX == 1) ? new Color(0, .5f, 1, .7f) : Color.white;
                if (GUILayout.Button(new GUIContent("P"), EditorStyles.toolbarButton, GUILayout.Width(25)))
                {
                    if (TOOL_INDEX == -1)
                        TOOL_INDEX = 1;
                    else
                        TOOL_INDEX = -1;

                    SceneView.currentDrawingSceneView.Repaint();
                }
                GUI.color = Color.white;
                if (_editorMode == MODE.Edge)
                {
                    if (GUILayout.Button(new GUIContent("Connect"), EditorStyles.toolbarButton))
                    {
                        kPoly.EdgeConnect_Preview(_selectMesh, curPointIndex, edges);
                        //_selectMesh, curPointIndex, edges, SGUIelements._connex, SGUIelements._conPad, true);
                        // ModifiVerticies_edgeConnect();
                    }
                    GUI.color = (TOOL_INDEX == 2) ? new Color(0, .5f, 1, .7f) : Color.white;
                    if (GUILayout.Button(new GUIContent("P"), EditorStyles.toolbarButton, GUILayout.Width(25)))
                    {
                        if (TOOL_INDEX == -1)
                            TOOL_INDEX = 2;
                        else
                            TOOL_INDEX = -1;

                        SceneView.currentDrawingSceneView.Repaint();
                    }
                    GUI.color = Color.white;
                }
                // GUILayout.Space(10);
                GUILayout.EndHorizontal();
            }
            //-------------------------------------------------------------------
            //  GEOMETRY
            FOLD_geome = EditorGUILayout.Foldout(FOLD_geome, "Edit Geometry");
            if (FOLD_geome)
            {
                //EditorGUILayout.Toggle(false, "Preserve UVs");
                GUILayout.BeginHorizontal();

                if (GUILayout.Button(new GUIContent("Make Planar"), EditorStyles.toolbarButton))
                {
                    if (_selection != null && _selectMesh != null)
                    {
                        kPoly.VerticesFlatten(_selectMesh, curPointIndex, _editorMode, planarHelp, edges, faces);
                        curPointIndex.Clear();
                    }
                }
                GUILayout.Space(5);
                GUI.color = Color.white;
                GUI.color = (planarHelp.x_Axis) ? Color.grey : Color.white;
                if (GUILayout.Button(new GUIContent("X"), EditorStyles.toolbarButton)) { planarHelp.x_Axis = !planarHelp.x_Axis; }
                GUI.color = Color.white;
                GUI.color = (planarHelp.y_Axis) ? Color.grey : Color.white;
                if (GUILayout.Button(new GUIContent("Y"), EditorStyles.toolbarButton)) { planarHelp.y_Axis = !planarHelp.y_Axis; }
                GUI.color = Color.white;
                GUI.color = (planarHelp.z_Axis) ? Color.grey : Color.white;
                if (GUILayout.Button(new GUIContent("Z"), EditorStyles.toolbarButton)) { planarHelp.z_Axis = !planarHelp.z_Axis; }
                GUI.color = Color.white;

                GUILayout.EndHorizontal();
            }
            //   EditorGUILayout.LabelField("Active Object : " + _selection.name + " Mesh : " + _selectMesh.name);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent("Clear Verts"), EditorStyles.toolbarButton)) verts = null;
      //      if (GUILayout.Button(new GUIContent("Clear Tris"), EditorStyles.toolbarButton)) tris = null;
            if (GUILayout.Button(new GUIContent("Clear Edges"), EditorStyles.toolbarButton)) edges = null;
            if (GUILayout.Button(new GUIContent("Clear Faces"), EditorStyles.toolbarButton)) faces = null;
            GUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            GUILayout.Space(10);
            EditorGUILayout.EndHorizontal();
        }

        public static void Draw_Handles()
        {
            _selection = kSelect.OBJECT;
            Mesh _selectMesh = kSelect.MESH;
            if (_selectMesh == null || _selection == null)
            {
                return;
            }

            Undo.SetSnapshotTarget(_selectMesh, "MeshEdit");

            verts = _selectMesh.vertices;
            Transform root = _selection.transform;

            /*     int controlIDBeforeHandle = -1,
                     controlIDAfterHandle = -1;
                 bool isEventUsedByHandle = false,
                     isEventUsedBeforeHandle = false,
                     setDirty = false,
                     isDrawn = false;*/
            bool setDirty = false;

            // Vert Labels
            if (KP_info._SHOW_TRIAS)
                for (int i = 0; i < verts.Length; i++)
                {
                    Vector3 v1 = root.TransformPoint(verts[i]);
                    Handles.Label(v1, new GUIContent("" + i));
                }
            //     int someHashCode = kPoly2Tool.instance.GetHashCode();
            switch (_editorMode)
            {
                case MODE.Point: setDirty = ModifiVerticies_points(); break;
                case MODE.Edge: setDirty = ModifiVerticies_edges(); break;
                case MODE.Triangle: setDirty = ModifiVerticies_tris(); break;
                case MODE.Quad: setDirty = ModifiVerticies_quad(); break;
                case MODE.All: setDirty = ModifiVerticies_all(); break;
            }


            if (setDirty && _selection != null)
            {
                Undo.IncrementCurrentEventIndex();

                _selectMesh.vertices = verts;

                _selectMesh.RecalculateNormals();
                _selectMesh.RecalculateBounds();

                Undo.IncrementCurrentEventIndex();

                _selection.GetComponent<MeshCollider>().sharedMesh = null;
                _selection.GetComponent<MeshCollider>().sharedMesh = _selectMesh;

                kPoly2Tool.instance.Repaint();
                SceneView.RepaintAll();
            }
        }



        private static bool ModifiVerticies_quad() // -------------------------------------------- TRANSFORM quadz
        {
            if (_selectMesh == null || _selectMesh.vertexCount != kSelect.MESH.vertexCount || faces == null || edges == null)
            {
                _selectMesh = kSelect.MESH;
                edges = null;
                edges = kPoly.BuildEdges(_selectMesh.vertexCount, _selectMesh.triangles);
                faces = kPoly.BuildFaces(_selectMesh.vertexCount, _selectMesh, edges);
            }

            bool setDirty = false; ;
            Vector3 dhp = Vector3.zero;
            Vector3 scale = new Vector3(1, 1, 1);
            Vector3 mpos = Vector3.zero;
            Quaternion mrot = Quaternion.identity;
            Transform root = _selection.transform;
            //int eIndex = 0;
            List<Vector3> vlist = new List<Vector3>(_selectMesh.vertices);
            List<int> tlist = new List<int>(_selectMesh.triangles);

            if (KP_info._SHOW_TRIAS)
            {
                /* int id = 0;
                 foreach (Edge e in edges)
                 {
                     if (tlist[edges[id].faceIndex[0]] != tlist[edges[id].faceIndex[1]] )
                     {
                         GUIStyle style = new GUIStyle();
                         Debug.Log(id );
                        // if (tlist[edges[id].faceIndex[0]] == tlist[edges[id].faceIndex[1]])
                        //     style.normal.textColor = Color.blue;
                        // else
                             style.normal.textColor = Color.white;

                         Vector3 d = (vlist[edges[id].vertexIndex[0]] + vlist[edges[id].vertexIndex[1]]) / 2;
                         Handles.Label(_selection.transform.TransformPoint(d), id + " " + " \n" +
                             //    tlist[edges[id].faceIndex[0] * 3] + " " + tlist[edges[id].faceIndex[1] * 3]+ " \n" +
                             edges[id].faceIndex[0] + " " + edges[id].faceIndex[1] + " \n"
                             // +  edges[id].faceIndex[0] % 3 + " " + edges[id].faceIndex[1] % 3
                         , style);
                     }
                     id++;
                 }*/
                for (int i = 0, n = faces.Length; i < n; i++)
                {
                    Face f = faces[i];
                    GUIStyle style = new GUIStyle();
                    style.normal.textColor = Color.white;

                    Handles.Label(_selection.transform.TransformPoint(f.middle), i + " " + " \n" +
                        f.triIndex[0] + " " + f.triIndex[1] //+ " \n"
                        // +  edges[id].faceIndex[0] % 3 + " " + edges[id].faceIndex[1] % 3
                    , style);

                }
            }
            for (int i = 0, n = faces.Length; i < n; i++)
            {
                if (curPointIndex.Contains(i))
                    Handles.color = new Color(Color.red.r, Color.red.g, Color.red.b, .85f);
                else
                    Handles.color = new Color(Color.green.r, Color.green.g, Color.green.b, .85f);

                float cubeSize = HandleUtility.GetHandleSize(faces[i].middle) * .04f;

                if (curPointIndex.Contains(i))
                {

                    Vector3[] vs = new Vector3[4];
                    for (int id = 0, n2 = 4; id < n2; id++)
                    {
                        //   Debug.Log(faces[i].vertexIndex[id]);
                        vs[id] = root.TransformPoint(vlist[faces[i].vertexIndex[id]]);
                    }
                    Handles.DrawPolyLine(vs);
                }
                if (Handles.Button(root.TransformPoint(faces[i].middle), Quaternion.identity, cubeSize, cubeSize, Handles.CubeCap))
                {
                    POINT_SELECTION(i);
                }

            }
            foreach (int id in curPointIndex)
            {

                dhp += root.TransformPoint(faces[curPointIndex[0]].middle);
            }
            if ((mpos = dhp = dhp / curPointIndex.Count) != Vector3.zero)
            {
                switch (Tools.current)
                {
                    case Tool.Move:
                        mpos = Handles.PositionHandle(mpos, mrot);
                        break;
                    case Tool.Rotate:
                        mrot = Handles.RotationHandle(mrot, dhp);
                        break;
                    case Tool.Scale:
                        scale = Handles.ScaleHandle(scale, dhp, mrot, 1);
                        break;
                }
                if (mpos != dhp || mrot != Quaternion.identity || scale != new Vector3(1, 1, 1))
                {
                    Vector3 d = mpos - dhp;
                    if (mrot != Quaternion.identity || scale != new Vector3(1, 1, 1)) d = Vector3.zero;
                    Matrix4x4 m = Matrix4x4.TRS(d, mrot, scale);

                    foreach (int id in curPointIndex)
                    {
                        Face f = faces[id];

                        int d1 = f.vertexIndex[0];
                        int d2 = f.vertexIndex[1];
                        int d3 = f.vertexIndex[2];
                        int d4 = f.vertexIndex[3];

                        verts[d1] = m.MultiplyPoint3x4(_selectMesh.vertices[d1]);
                        verts[d2] = m.MultiplyPoint3x4(_selectMesh.vertices[d2]);
                        verts[d3] = m.MultiplyPoint3x4(_selectMesh.vertices[d3]);
                        verts[d4] = m.MultiplyPoint3x4(_selectMesh.vertices[d4]);

                        f.middle = (verts[d1] + verts[d2] + verts[d3] + verts[d4]) / 4;

                    }

                    foreach (Face f in faces)
                    {
                        int d1 = f.vertexIndex[0];
                        int d2 = f.vertexIndex[1];
                        int d3 = f.vertexIndex[2];
                        int d4 = f.vertexIndex[3];
                        f.middle = (verts[d1] + verts[d2] + verts[d3] + verts[d4]) / 4;
                    }

                    setDirty = true;
                }
            }
            return setDirty;

        }
        private static bool ModifiVerticies_tris() // -------------------------------------------- TRANSFORM triangles
        {
            if (_selectMesh == null)
            {
                _selectMesh = kSelect.MESH;
            }
            bool setDirty = false;
            //   isDrawn = false;

            Vector3 dhp = Vector3.zero;
            Vector3 scale = new Vector3(1, 1, 1);
            Vector3 mpos = Vector3.zero;
            Quaternion mrot = Quaternion.identity;
            Transform root = _selection.transform;

            //     int someHashCode = kPoly2Tool.instance.GetHashCode();
            int[] tris = _selectMesh.triangles;

            //float cubeSize = .1f;
            //Vector3 dv = Vector3.zero;
            for (int i = 0, n = tris.Length / 3; i < n; i++)
            {

                //     Debug.Log(n + " " + i * 3 + " " + verts[tris[i * 3]] + "| " + (i * 3 + 1) + " " + verts[tris[i * 3+1]] + "| " + (i * 3 + 2)+ " "+ verts[tris[i * 3+2]] + "| " );

                Vector3 v1 = root.TransformPoint(verts[tris[i * 3]]);
                Vector3 v2 = root.TransformPoint(verts[tris[i * 3 + 1]]);
                Vector3 v3 = root.TransformPoint(verts[tris[i * 3 + 2]]);
                Vector3 dv = (v1 + v2 + v3) / 3;

                //          int controlIDBeforeHandle = GUIUtility.GetControlID(someHashCode, FocusType.Passive);
                //         bool isEventUsedBeforeHandle = (Event.current.type == EventType.used);

                if (curPointIndex.Contains(i))
                    Handles.color = new Color(Color.red.r, Color.red.g, Color.red.b, .85f);
                else
                    Handles.color = new Color(Color.green.r, Color.green.g, Color.green.b, .85f);

                float cubeSize = HandleUtility.GetHandleSize(v1) * .04f;

                Handles.DrawPolyLine(new Vector3[4] { v1, v2, v3, v1 });


                //         Handles.color = new Color(Color.cyan.r, Color.cyan.g, Color.cyan.b, .85f);
                // friend triangle
                /*          Vector3 f1 = root.TransformPoint(verts[tris[i * 3]]);
                          Vector3 f2 = root.TransformPoint(verts[tris[i * 3 + 1]]);
                          Vector3 f3 = root.TransformPoint(verts[tris[i * 3 + 2]]);
                        
                          Vector3 fv = (f1 + f2 + f3) / 3;
                        
                          Handles.DrawPolyLine(new Vector3[4] { f1, f2, f3, f1 });
  */
                //    Debug.Log((tris[i]) % 3);

                /* Handles.Label(v1+new Vector3(0,1), new GUIContent("" + verts[tris[i * 3 + 0]]));
                 Handles.Label(v2 + new Vector3(0, 1), new GUIContent("" + verts[tris[i * 3 + 1]]));
                 Handles.Label(v3 + new Vector3(0, 1), new GUIContent("" + verts[tris[i * 3 + 2]]));
                */

                if (Handles.Button(dv, Quaternion.identity, cubeSize, cubeSize, Handles.CubeCap))
                {
                    POINT_SELECTION(i);
                }
            }
            foreach (int id in curPointIndex)
            {
                int d1 = tris[id * 3];
                int d2 = tris[id * 3 + 1];
                int d3 = tris[id * 3 + 2];

                Vector3 pd1 = verts[d1];
                Vector3 pd2 = verts[d2];
                Vector3 pd3 = verts[d3];

                dhp += root.TransformPoint((pd1 + pd2 + pd3) / 3);

            }
            if ((mpos = dhp = dhp / curPointIndex.Count) != Vector3.zero)
            {
                switch (Tools.current)
                {
                    case Tool.Move:
                        mpos = Handles.PositionHandle(mpos, mrot);
                        break;
                    case Tool.Rotate:
                        mrot = Handles.RotationHandle(mrot, dhp);
                        break;
                    case Tool.Scale:
                        scale = Handles.ScaleHandle(scale, dhp, mrot, 1);
                        break;
                }
                if (mpos != dhp || mrot != Quaternion.identity || scale != new Vector3(1, 1, 1))
                {
                    Vector3 d = mpos - dhp;
                    if (mrot != Quaternion.identity || scale != new Vector3(1, 1, 1)) d = Vector3.zero;
                    Matrix4x4 m = Matrix4x4.TRS(d, mrot, scale);

                    foreach (int id in curPointIndex)
                    {
                        int d1 = tris[id * 3];
                        int d2 = tris[id * 3 + 1];
                        int d3 = tris[id * 3 + 2];

                        verts[d1] = m.MultiplyPoint3x4(_selectMesh.vertices[d1]);
                        verts[d2] = m.MultiplyPoint3x4(_selectMesh.vertices[d2]);
                        verts[d3] = m.MultiplyPoint3x4(_selectMesh.vertices[d3]);

                    }
                    setDirty = true;
                }
            }
            return setDirty;
        }



        private static bool ModifiVerticies_edges()// -------------------------------------------- TRANSFORM edges
        {
            if (_selectMesh == null ||
                _selectMesh.vertexCount != kSelect.MESH.vertexCount || 
                edges == null
                )
            {
                _selectMesh = kSelect.MESH;
                edges = null;
                //edges = BuildEdges(_selectMesh.vertexCount, _selectMesh.triangles);
            }
            bool setDirty = false;
            //    isDrawn = false;
            Vector3 dhp = Vector3.zero;
            Vector3 scale = new Vector3(1, 1, 1);
            Vector3 mpos = Vector3.zero;
            Quaternion mrot = Quaternion.identity;
            Transform root = _selection.transform;
            if (edges == null) edges = kPoly.BuildEdges(_selectMesh.vertexCount, _selectMesh.triangles);
            int eIndex = 0;
            List<Vector3> nverts = new List<Vector3>(_selectMesh.vertices);
            foreach (Edge edge in edges)
            {
                //   if (edge.faceIndex[0] == edge.faceIndex[1])
                //     {

                int t1 = edge.vertexIndex[0];
                int t2 = edge.vertexIndex[1];
                if (t1 < 0 || t2 < 0 || t1 > edges.Length || t2 > edges.Length)
                {
                    edges = null;
                    edges = kPoly.BuildEdges(_selectMesh.vertexCount, _selectMesh.triangles);
                    return false;
                }
                Vector3 p1 = _selectMesh.vertices[t1];
                Vector3 p2 = _selectMesh.vertices[t2];
                Vector3 dv = root.TransformPoint((p1 + p2) / 2);



                float cubeSize = HandleUtility.GetHandleSize(dv) * .04f;
                if (curPointIndex.Contains(eIndex))//&& curPointIndex.Contains(t2))
                    Handles.color = new Color(Color.red.r, Color.red.g, Color.red.b, .85f);
                else
                {
                    if (edge.faceIndex[0] != edge.faceIndex[1])
                        Handles.color = new Color(Color.green.r, Color.green.g, Color.green.b, 0);//.85f);.85f);
                    else
                        Handles.color = new Color(Color.cyan.r, Color.cyan.g, Color.cyan.b, 0.5f);
                }
                //Handles.Label(dv, t1 + "->" + t2);
                if (Handles.Button(dv, Quaternion.identity, cubeSize, cubeSize, Handles.SphereCap))
                {
                    if (!ANY_KEY) curPointIndex.Clear();
                    curPointIndex.Add(eIndex);
                    //      Debug.Log("Select Edge : " + eIndex + " T1 : " + t1 + " T2 : " + t2);
                }
                eIndex++;
            }
            foreach (int id in curPointIndex)
            {
                int d1 = edges[id].vertexIndex[0];
                int d2 = edges[id].vertexIndex[1];
                Vector3 pd1 = verts[d1];
                Vector3 pd2 = verts[d2];

                dhp += root.TransformPoint((pd1 + pd2) / 2);

            }
            if ((mpos = dhp = dhp / curPointIndex.Count) != Vector3.zero && TOOL_INDEX != 2)
            {
                switch (Tools.current)
                {
                    case Tool.Move:
                        mpos = Handles.PositionHandle(mpos, mrot);
                        break;
                    case Tool.Rotate:
                        mrot = Handles.RotationHandle(mrot, dhp);
                        break;
                    case Tool.Scale:
                        scale = Handles.ScaleHandle(scale, dhp, mrot, 1);
                        break;
                }
                if (mpos != dhp || mrot != Quaternion.identity || scale != new Vector3(1, 1, 1))
                {
                    Vector3 d = mpos - dhp;
                    if (mrot != Quaternion.identity || scale != new Vector3(1, 1, 1)) d = Vector3.zero;
                    Matrix4x4 m = Matrix4x4.TRS(d, mrot, scale);
                    //        Debug.Log(_dragCreate + " " + ANY_KEY);
                    if (!ANY_KEY)
                    {
                        foreach (int id in curPointIndex)
                        {

                            int d1 = edges[id].vertexIndex[0];
                            int d2 = edges[id].vertexIndex[1];
                            verts[d1] = m.MultiplyPoint3x4(_selectMesh.vertices[d1]);
                            verts[d2] = m.MultiplyPoint3x4(_selectMesh.vertices[d2]);
                        }
                        _dragCreate = false;
                    }
                    else
                    {
                        /// create two new verts
                        /// create edge

                        if (!_dragCreate)
                        {

                            int io1 = edges[curPointIndex[0]].vertexIndex[0];
                            int io2 = edges[curPointIndex[0]].vertexIndex[1];

                            List<Vector3> vlist = new List<Vector3>(_selectMesh.vertices);
                            vlist.AddRange(new List<Vector3>() { m.MultiplyPoint3x4(_selectMesh.vertices[io1]), m.MultiplyPoint3x4(_selectMesh.vertices[io2]) });

                            int in1 = _selectMesh.vertexCount;
                            int in2 = _selectMesh.vertexCount + 1;

                            List<int> tlist = new List<int>(_selectMesh.triangles);
                            tlist.AddRange(new List<int>() { io1, in1, in2, io1, in2, io2 });

                            //     foreach (int i in tlist.GetRange(_selectMesh.triangles.Length, 6)) Debug.Log("->" + i);

                            _selectMesh.vertices = vlist.ToArray();
                            _selectMesh.triangles = tlist.ToArray();

                            edges = null;
                            edges = kPoly.BuildEdges(_selectMesh.vertexCount, _selectMesh.triangles);

                            /// get the index of new created edge
                            /// set curPoint id -> the new one
                            _dragCreate = true;

                            int nei = kPoly.EdgeIndex(in1, in2, edges);
                            //   edges[curPointIndex[0]].faceIndex[0];
                            //

                            curPointIndex.Clear();
                            curPointIndex.Add(nei);


                            edges = null;
                            faces = null;
                            //  Debug.Log("Create " + in1 + " " + in2 + " ->" + (edges[nei]));

                            // return false;
                        }
                    }


                    setDirty = true;
                    //    isDrawn = true;
                }
            }

            return setDirty;
        }
        public static void VerticesRemover()
        {
           _selectMesh =  VerticesRemover( ((_selectMesh != null )? _selectMesh : kSelect.MESH), curPointIndex, _editorMode, edges, faces);
            curPointIndex.Clear();
            switch (_editorMode)
            {
                case MODE.Edge: edges = null; edges = kPoly.BuildEdges(_selectMesh.vertexCount, _selectMesh.triangles); break;
            }
                
            faces = null;
            //verts = null;
        }
        private static Mesh VerticesRemover(Mesh _selectMesh, List<int> curPointIndex, MODE _editorMode, Edge[] edges = null, Face[] faces = null)
        {
           return kPoly.VerticesRemover(_selectMesh, curPointIndex, _editorMode, edges, faces);
        }
        
        private static bool ModifiVerticies_points(bool now = false)// -------------------------------------------- TRANSFORM verticies
        {
            bool setDirty = false,
                isDrawn = false;
            Vector3 dhp = Vector3.zero;
            Vector3 scale = new Vector3(1, 1, 1);
            Vector3 mpos = Vector3.zero;
            Quaternion mrot = Quaternion.identity;
            Transform root = _selection.transform;

            for (int i = 0; i < verts.Length; i++)
            {
                Vector3 v1 = root.TransformPoint(verts[i]);

                if (curPointIndex.Contains(i))
                    Handles.color = new Color(Color.red.r, Color.red.g, Color.red.b, .85f);
                else
                    Handles.color = new Color(Color.green.r, Color.green.g, Color.green.b, .85f);

                float cubeSize = HandleUtility.GetHandleSize(v1) * .04f;

                if (Handles.Button(v1, Quaternion.identity, cubeSize, cubeSize, Handles.CubeCap))
                {
                    POINT_SELECTION(i);
                }

                // Debug.Log( Tools.pivotMode );

                /*   if (curPointIndex.Contains(i))
                   {
                       Vector3 dv = Vector3.zero;
                       Vector3 mv = Vector3.zero;

                       foreach (int id in curPointIndex) dv += root.TransformPoint(verts[id]);

                       if ((dv = dv / curPointIndex.Count) != Vector3.zero)
                       {
                           mv = Handles.PositionHandle(dv, Quaternion.identity);
                           if (mv != dv && !setDirty)
                           {
                               Vector3 d = mv - dv;
                               foreach (int id in curPointIndex) verts[id] += d;
                               setDirty = true;
                           }
                       }
                   }*/
                if (curPointIndex.Contains(i) && !isDrawn)
                {
                    foreach (int id in curPointIndex) dhp += root.TransformPoint(verts[id]);
                    if ((mpos = dhp = dhp / curPointIndex.Count) != Vector3.zero)
                    {
                        switch (Tools.current)
                        {
                            case Tool.Move:
                                mpos = Handles.PositionHandle(mpos, (Tools.pivotMode == PivotMode.Center ? root.localRotation : Quaternion.identity));
                                break;
                            case Tool.Rotate:
                                mrot = Handles.RotationHandle(mrot, dhp);
                                break;
                            case Tool.Scale:
                                scale = Handles.ScaleHandle(scale, dhp, mrot, 1);
                                break;
                        }
                        isDrawn = true;
                        if (mpos != dhp || mrot != Quaternion.identity || scale != new Vector3(1, 1, 1))//&& !setDirty)
                        {
                            Vector3 d = mpos - dhp;
                            if (mrot != Quaternion.identity || scale != new Vector3(1, 1, 1)) d = Vector3.zero;
                            Matrix4x4 m = Matrix4x4.TRS(d, mrot, scale);
                            foreach (int id in curPointIndex) verts[id] = m.MultiplyPoint3x4(verts[id]);

                            setDirty = true;
                        }
                    }
                }
            }


            if (_selection != null && TOOL_INDEX != -1 && curPointIndex.Count > 0 && !setDirty)
            {
                //  int[] tris = _selectMesh.triangles;
                int sp = curPointIndex[0];
                Vector3 tv = root.TransformPoint(verts[sp]);
                toolVerts = new List<int>();
                isDrawn = false;
                // Find verts intersects
                for (int i = 0; i < verts.Length; i++)
                {
                    Vector3 tiv = root.TransformPoint(verts[i]);
                    float dist = Vector3.Distance(tv, tiv);
                    if (dist < SGUIelements._toolRadius && dist > 0.00000000001f)
                    {
                        //Debug.Log("Add Vert : " + i + " sp  " + sp);
                        toolVerts.Add(i);
                    }
                }
                //---------------------------------------------------------------DRAW HANDLES
                if (toolVerts.Count < 1)
                {
                    Handles.color = new Color(Color.red.r, Color.red.g, Color.red.b, .85f);
                    toolVerts = null;
                }
                else
                    Handles.color = new Color(Color.green.r, Color.green.g, Color.green.b, .85f);

                Handles.DrawWireDisc(tv, Vector3.up, SGUIelements._toolRadius);

                //-------------------------------------------------------------------------TOOL method

                if (SGUIelements._toolPreview || now)
                {
                    bool change = false;
                    // If found verts in circle | 
                    if (toolVerts != null && toolVerts.Count != 0 && isDrawn == false)
                    {
                        if (_selectMesh == null) _selectMesh = kSelect.MESH;

                        List<int> tris = new List<int>(_selectMesh.triangles);
                        List<int> ntris = new List<int>();
                        //List<int> rtris = new List<int>();
                        //  List<Vector3> verti = new List<Vector3>(verts);

                        if (isDrawn == false)
                        {
                            foreach (int cp in toolVerts)
                            {
                                for (int i = 0, n = tris.Count; i < n; i += 3)
                                {
                                    List<int> mlist = tris.GetRange(i, 3);
                                    bool hasCP = mlist.Contains(cp);
                                    bool hasSP = mlist.Contains(sp);

                                    if (!hasCP || hasCP && !hasSP)
                                    {
                                        //   Debug.Log(i + "  -> " + tris[i] + " " + tris[i + 1] + " " + tris[i + 2]);
                                        //    cTris.Add(i);
                                        if (hasCP && !hasSP)
                                        {
                                            // rename mlist
                                            int nr = mlist.IndexOf(cp);
                                            if (nr > -1) mlist[nr] = sp;
                                        }
                                        ntris.AddRange(mlist);
                                        change = true;
                                    }
                                }
                            }
                        }

                        if (change)
                        {

                            //      VERT_remove(toolVerts[0]);
                            /*       _selectMesh.vertices = verti.ToArray();
                                   _selectMesh.RecalculateBounds();*/
                        //    Debug.Log("---------------------------------------------------------------");

                            _selectMesh.triangles = ntris.ToArray();
                            curPointIndex.Clear();

                            SceneView.RepaintAll();
                            SGUIelements._toolPreview = false;
                            setDirty = isDrawn = true;

                        }
                    }
                }
            }

            return setDirty;
        }

        private static bool ModifiVerticies_all()// -------------------------------------------- TRANSFORM complete MeshObject
        {
            bool setDirty = false;
            Vector3 dhp = Vector3.zero;
            Vector3 scale = new Vector3(1, 1, 1);
            Vector3 mpos = Vector3.zero;
            Quaternion mrot = Quaternion.identity;

            foreach (Vector3 v in verts)
            {
                dhp += _selection.transform.TransformPoint(v);
            }
            mpos = dhp = dhp / verts.Length;

            float cubeSize = HandleUtility.GetHandleSize(dhp) * .04f;
            Handles.color = new Color(Color.red.r, Color.red.g, Color.red.b, .85f);
            Handles.Button(dhp, Quaternion.identity, cubeSize, cubeSize, Handles.CubeCap);
            switch (Tools.current)
            {
                case Tool.Move:
                    mpos = Handles.PositionHandle(dhp, mrot);
                    break;
                case Tool.Rotate:
                    mrot = Handles.RotationHandle(mrot, dhp);
                    break;
                case Tool.Scale:
                    scale = Handles.ScaleHandle(scale, dhp, mrot, 1);
                    break;
            }
            if (!setDirty)
            {
                Vector3 d = mpos - dhp;
                Matrix4x4 m = Matrix4x4.TRS(d, mrot, scale);
                for (int i = 0; i < verts.Length; i++)
                {
                    verts[i] = m.MultiplyPoint3x4(verts[i]);
                }
                setDirty = true;
            }
            return setDirty;
        }
        private static void POINT_SELECTION(int i)
        {


            if (!ANY_KEY && curPointIndex.Count > 0) curPointIndex.Clear();
            if (!curPointIndex.Contains(i))
            {
                curPointIndex.Add(i);
                kPoly2Tool.instance.Repaint();
            }
            else
                curPointIndex.Remove(i);
        }
        public static void EdgeConnect_Preview()
        {
            kPoly.EdgeConnect_Preview(_selectMesh, curPointIndex, edges);
        }

        /* private static int GetEdge(int v1, int v2)
         {
             int index = 0;
             foreach (Edge e in edges)
             {
                 if (e.vertexIndex[0] == v1 && e.vertexIndex[1] == v2 ||
                     e.vertexIndex[1] == v1 && e.vertexIndex[0] == v2)
                 {
                     return index;
                 }
                 index++;
             }
             return -1;
         }*/
        /*     int tc = tlist.Count;
             int qc = (tc/2)/3;

             for( int i=0;i<qc;i++)
             {
                 Face f = new Face();

                 int edgeID = 0;
                 foreach (Edge e in edges)
                 {
                    // Debug.Log("? " + i + " " + edgeID + " " + tlist[e.faceIndex[0]*3]);
                     if (tlist[e.faceIndex[0] * 3] == i)
                     {
                        
                         if (!f.edgeIndex.Contains(edgeID))
                         {
                            Debug.Log("Add Edge " + edgeID);
                             f.edgeIndex.Add(edgeID);
                         }
                     }
                     edgeID++;
                 }

                 faceArray.Add(f);
             }

              for (int i = 0, n = maxCount; i < n; i += 3)
                         {
                             List<int> mlist = tlist.GetRange(i, 3);
                             Tri tri = new Tri();
                             tri.vertexIndex = mlist.ToArray();
                             tri.triIndex = i;
                             // tri.neiIndex = -1;

                             if (!triArray.Contains(tri)) triArray.Add(tri);

                           ///  Vector3 d = (vlist[tlist[i]] +
                           ///                  vlist[tlist[i + 1]] +
                           ////                  vlist[tlist[i + 2]]) / 3;
                           //  GUIStyle style = new GUIStyle();
                           //  style.normal.textColor = Color.green;
                           //        Handles.Label(_selection.transform.TransformPoint(d), i / 3 + " ", style);
                         }
              * 
              * 
                         int qi = 0;
                         foreach (Face f in faceArray)
                         {
                             Debug.Log("Face " + qi);
                
                             id=0;
                             foreach (int eid in f.edgeIndex)
                             {
                                 Edge e = edges[eid];
                                 int id2 =0;

                                 Debug.Log("Face With :  ");
                                 Debug.Log(e.vertexIndex[0] + " " + e.vertexIndex[1] );
                                 //foreach (int fi in e.faceIndex)
                                 //{
                                 //    Debug.Log("Faceindex  " + id2 + " [ " + fi + " ]");
                                 //    id2++;
                                 //}
                                 id++;
                             }
                             qi++;

                         }*/
        /*
        int m = tlist[edges[edges.Length - 1].faceIndex[0] * 3];
        List<List<int>> lo = new List<List<int>>();

      List<Edge> ed = new List<Edge>(edges);
        for (int i = 0; i <= m; i++)
        {
            lo.Add(new List<int>());
            int iid = 0;
            foreach (Edge e in edges)
            {
                //Debug.Log(i + " " + tlist[e.faceIndex[0] * 3]);
                if (tlist[e.faceIndex[0] * 3] == i)
                {
                    int enr = ed.IndexOf(e);
                    lo[i].Add(enr);//enr);
                }
                iid++;
            }
        }*/
        /*
                     for (int i = 0; i < m; i++)
                     {

                         Debug.Log(i  + " " + lo[i].Count);


                         Vector3 d = (vlist[tlist[lo[i][0]]] +
                                        vlist[tlist[lo[i][1]]] +
                                        vlist[tlist[lo[i][2]]] +
                                        vlist[tlist[lo[i][3]]]) / 4;
                         GUIStyle style = new GUIStyle();
                         style.normal.textColor = Color.green;
                         Handles.Label(_selection.transform.TransformPoint(d), i + " ", style);

                     }
                    */

    }//kP_edit end



}//namespace end
/*  switch (Tools.current)
  {
      case Tool.Move:
          verts[i] += d;
          break;
      case Tool.Rotate:
          verts[i] = msav * verts[i];
          break;
      case Tool.Scale:
          Debug.Log(verts[i]);
          //verts[i] = verts[i] + mdav;
          break;
  }
 
 
    int triIndex = -1;
    for (int i = 0, n = nlist.Count; i < n; i += 3)
    {
        bool i1 = nInde.Contains(nlist[i]);
        bool i2 = nInde.Contains(nlist[i+1]);
        bool i3 = nInde.Contains(nlist[i+2]);
        //Debug.Log("? " + i + " / " + i1 + " " + i2 + " " + i3 + " " + nInde[0] + "  " + nInde[1] + " " + nInde[2]);
                
        if (i1 && i2 && i3)
        {
            Debug.Log("? " + i);
            triIndex = i;
        }
    }
 
 
 
 
 
 
 
 
 
 
 
 
 */
