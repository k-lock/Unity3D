//--------------------------------------------------------------------------------------------------------------------------------------//
//                                                                                                                                      //
//     PANEL EDIT                                                                                                                       //
//                                                                                                                                      //
//--------------------------------------------------------------------------------------------------------------------------------------//

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

using klock.kEditPoly.helper;
using klock.geometry;

namespace klock.kEditPoly.panels
{
    public class KP_edit
    {
        static bool FOLD_selection = true;
        static bool FOLD_object = true;
        static bool FOLD_verts = true;
        static bool FOLD_geome = true;
        public static GameObject _selection = null;
        public static bool _freeze = false;
        public static bool ANY_KEY = false;

        static PLANAR_HELPER planarHelp = new PLANAR_HELPER();
        public static MODE _editorMode = MODE.None;

        static int triIndex = -1;
        static List<int> curPointIndex = new List<int>();
        static Vector3[] verts;

        public static void DRAW_PANEL()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.BeginVertical();
            GUILayout.Space(5);

            if (_selection == null) _selection = kSelect.OBJECT;
            Mesh _selectMesh = kSelect.MESH;
            MODE modeTemp = _editorMode;
            GUI.enabled = (_selection != null && _selectMesh != null);
            //-------------------------------------------------------------------
            //  SELECTION
            FOLD_selection = EditorGUILayout.Foldout(FOLD_selection, "Selection " + ANY_KEY);// + ((_selection != null) ? "[ " + _selection.name + " ]" : ""));
            if (FOLD_selection)
            {
                bool pressed = false;

                GUILayout.BeginHorizontal();
                GUI.color = (_editorMode == MODE.Point) ? new Color(0, .5f, 1, .7f) : Color.white;
                if (GUILayout.Button(new GUIContent("Point")))
                {
                    _editorMode = (_editorMode == MODE.Point) ? MODE.None : MODE.Point;
                    pressed = true;
                }
                GUI.color = Color.white;
                GUI.color = (_editorMode == MODE.Line) ? new Color(0, .5f, 1, .7f) : Color.white;
                if (GUILayout.Button(new GUIContent("Line")))
                {
                    _editorMode = (_editorMode == MODE.Line) ? MODE.None : MODE.Line;
                    pressed = true;
                }
                GUI.color = Color.white;
                GUI.color = (_editorMode == MODE.Tri) ? new Color(0, .5f, 1, .7f) : Color.white;
                if (GUILayout.Button(new GUIContent("Tri")))
                {
                    _editorMode = (_editorMode == MODE.Tri) ? MODE.None : MODE.Tri;
                    pressed = true;
                }
                GUI.color = Color.white;
                GUI.color = (_editorMode == MODE.Quad) ? new Color(0, .5f, 1, .7f) : Color.white;
                if (GUILayout.Button(new GUIContent("Quad")))
                {
                    _editorMode = (_editorMode == MODE.Quad) ? MODE.None : MODE.Quad;
                    pressed = true;
                }
                GUI.color = Color.white;
                GUI.color = (_editorMode == MODE.All) ? new Color(0, .5f, 1, .7f) : Color.white;
                if (GUILayout.Button(new GUIContent("All")))
                {
                    _editorMode = (_editorMode == MODE.All) ? MODE.None : MODE.All;
                    pressed = true;
                }
                GUI.color = Color.white;
                GUILayout.EndHorizontal();

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
                    //  }
                }

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Vertex ");
                EditorGUILayout.LabelField((_editorMode != MODE.None && curPointIndex.Count > 0) ? _editorMode.ToString() + " " + curPointIndex[0] : "", GUILayout.ExpandWidth(true));
                EditorGUILayout.EndHorizontal();
            }

            //-------------------------------------------------------------------
            //  VERTS
            FOLD_verts = EditorGUILayout.Foldout(FOLD_verts, "Edit Vertices");
            if (FOLD_verts)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button(new GUIContent("Remove"))) VERT_remove();
                GUILayout.Button(new GUIContent("Break"));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Button(new GUIContent("Extrude")); GUILayout.Button(new GUIContent("E"));
                GUILayout.Button(new GUIContent("Weld")); GUILayout.Button(new GUIContent("E"));
                GUILayout.EndHorizontal();
            }
            //-------------------------------------------------------------------
            //  GEOMETRY
            FOLD_geome = EditorGUILayout.Foldout(FOLD_geome, "Edit Geometry");
            if (FOLD_geome)
            {
                //EditorGUILayout.Toggle(false, "Preserve UVs");
                GUILayout.BeginHorizontal();
                if (GUILayout.Button(new GUIContent("Make Planar")))
                {
                    if (_selection != null && _selectMesh != null)
                    {
                        _selectMesh = klock.geometry.kPoly.Planar(_selectMesh, planarHelp.x_Axis, planarHelp.y_Axis, planarHelp.z_Axis);
                        _selectMesh.RecalculateBounds();
                        _selectMesh.RecalculateNormals();
                        Debug.Log("Refresh MEsh ");
                    }
                }
                GUI.color = Color.white;
                GUI.color = (planarHelp.x_Axis) ? Color.grey : Color.white;
                if (GUILayout.Button(new GUIContent("X"))) { planarHelp.x_Axis = !planarHelp.x_Axis; }
                GUI.color = Color.white;
                GUI.color = (planarHelp.y_Axis) ? Color.grey : Color.white;
                if (GUILayout.Button(new GUIContent("Y"))) { planarHelp.y_Axis = !planarHelp.y_Axis; }
                GUI.color = Color.white;
                GUI.color = (planarHelp.z_Axis) ? Color.grey : Color.white;
                if (GUILayout.Button(new GUIContent("Z"))) { planarHelp.z_Axis = !planarHelp.z_Axis; }
                GUI.color = Color.white;
                GUILayout.EndHorizontal();
            }
            //   EditorGUILayout.LabelField("Active Object : " + _selection.name + " Mesh : " + _selectMesh.name);
            EditorGUILayout.EndVertical();

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

            Vector3 trans = Vector3.zero;
            Vector3 eulerAngles = Vector3.zero;
            Vector3 scale = new Vector3(1, 1, 1);
            verts = _selectMesh.vertices;
            Transform root = _selection.transform;
            float cubeSize = .3f;
            int controlIDBeforeHandle = -1,
                controlIDAfterHandle = -1;
            bool isEventUsedByHandle = false,
                isEventUsedBeforeHandle = false,
                setDirty = false,
                isDrawn = false;

            // Point Mode 
            for (int i = 0; i < verts.Length; i++)
            {
                Vector3 v1 = root.TransformPoint(verts[i]);
                if (KP_info._SHOW_TRIAS) Handles.Label(v1, new GUIContent("" + i));
            }
            int someHashCode = kPoly2Tool.instance.GetHashCode();
            switch (_editorMode)
            {

                case MODE.Point:
                    setDirty = ModifiVerticies_points();
                    break;
                case MODE.Line:


                    break;
                case MODE.Tri:

                    int[] tris = _selectMesh.triangles;
                    int n = tris.Length / 3;

                    for (int i = 0; i < n; i++)
                    {

                        //   Debug.Log(n + " " + i * 3 + " " + verts[tris[i * 3]] + "| " + (i * 3 + 1) + " " + verts[tris[i * 3+1]] + "| " + (i * 3 + 2)+ " "+ verts[tris[i * 3+2]] + "| " );

                        Vector3 v1 = root.TransformPoint(verts[tris[i * 3]]);
                        Vector3 v2 = root.TransformPoint(verts[tris[i * 3 + 1]]);
                        Vector3 v3 = root.TransformPoint(verts[tris[i * 3 + 2]]);
                        Vector3 dv = (v1 + v2 + v3) / 3;

                        controlIDBeforeHandle = GUIUtility.GetControlID(someHashCode, FocusType.Passive);
                        isEventUsedBeforeHandle = (Event.current.type == EventType.used);

                        if (curPointIndex.Contains(i))
                            Handles.color = new Color(Color.red.r, Color.red.g, Color.red.b, .85f);
                        else
                            Handles.color = new Color(Color.green.r, Color.green.g, Color.green.b, .85f);

                        cubeSize = HandleUtility.GetHandleSize(v1) * .1f;

                        Handles.DrawPolyLine(new Vector3[4] { v1, v2, v3, v1 });

                        /* Handles.Label(v1+new Vector3(0,1), new GUIContent("" + verts[tris[i * 3 + 0]]));
                         Handles.Label(v2 + new Vector3(0, 1), new GUIContent("" + verts[tris[i * 3 + 1]]));
                         Handles.Label(v3 + new Vector3(0, 1), new GUIContent("" + verts[tris[i * 3 + 2]]));
                        */
                        if (Handles.Button(dv, Quaternion.identity, cubeSize, cubeSize, Handles.CubeCap))
                        {
                            POINT_SELECTION(i);
                        }

                        if (curPointIndex.Contains(i))
                        {

                            if (curPointIndex.Count == 1)
                            {
                                Vector3 dm = Handles.PositionHandle(dv, Quaternion.identity);
                                if (dm != dv && !isEventUsedBeforeHandle)
                                {

                                    Vector3 d = dm - dv;
                                    verts[tris[i * 3]] += d;
                                    verts[tris[i * 3 + 1]] += d;
                                    verts[tris[i * 3 + 2]] += d;

                                    setDirty = true;
                                }
                            }
                            else
                            {
                                if (!isDrawn)
                                {
                                    Vector3 dv2 = Vector3.zero;
                                    foreach (int id in curPointIndex)
                                    {
                                        Vector3 t1 = root.TransformPoint(verts[tris[id * 3]]);
                                        Vector3 t2 = root.TransformPoint(verts[tris[id * 3 + 1]]);
                                        Vector3 t3 = root.TransformPoint(verts[tris[id * 3 + 2]]);
                                        Vector3 dt = (t1 + t2 + t3) / 3;
                                        dv2 += dt;
                                    }

                                    dv2 = dv2 / curPointIndex.Count;

                                    if (dv2 != Vector3.zero)
                                    {
                                        Vector3 mv = Handles.PositionHandle(dv2, Quaternion.identity);
                                        if (mv != dv2 && !setDirty)
                                        {
                                            Vector3 d = mv - dv2;
                                            foreach (int id in curPointIndex)
                                            {
                                                verts[tris[id * 3]] += d;
                                                verts[tris[id * 3 + 1]] += d;
                                                verts[tris[id * 3 + 2]] += d;
                                            }
                                            setDirty = true;
                                        }
                                    }
                                    isDrawn = true;
                                }
                            }
                        }

                        /* controlIDAfterHandle = GUIUtility.GetControlID(someHashCode, FocusType.Native);
                         isEventUsedByHandle = !isEventUsedBeforeHandle && (Event.current.type == EventType.used);

                         if ((controlIDBeforeHandle < GUIUtility.hotControl && GUIUtility.hotControl < controlIDAfterHandle) || isEventUsedByHandle)
                         {
                             POINT_SELECTION(i);
                         }*/
                    }
                    break;
                case MODE.All:
                    setDirty = ModifiVerticies_all();
                    break;
            }


            if (setDirty)
            {
                _selectMesh.vertices = verts;
                _selectMesh.RecalculateNormals();
                _selectMesh.RecalculateBounds();

                _selection.GetComponent<MeshCollider>().sharedMesh = null;
                _selection.GetComponent<MeshCollider>().sharedMesh = _selectMesh;

                kPoly2Tool.instance.Repaint();
                SceneView.RepaintAll();
            }
        }
        private static bool ModifiVerticies_points()
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

                float cubeSize = HandleUtility.GetHandleSize(v1) * .1f;

                if (Handles.Button(v1, Quaternion.identity, cubeSize, cubeSize, Handles.CubeCap))
                {
                    POINT_SELECTION(i);
                }

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
                                mpos = Handles.PositionHandle(mpos, mrot);
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
            return setDirty;
        }
        private static bool ModifiVerticies_all()
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

            float cubeSize = HandleUtility.GetHandleSize(dhp) * .1f;
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
        private static void VERT_remove()
        {

            if (curPointIndex.Count == 0) return;
            Transform root = _selection.transform;
            Mesh _selectMesh = kSelect.MESH;
            int killID = curPointIndex[0];
            if (_selectMesh == null && killID != -1) return;

            Undo.SetSnapshotTarget(_selectMesh, "MeshVertexRemove");

            List<Vector3> nVertices = new List<Vector3>(_selectMesh.vertices);
            nVertices.RemoveAt(killID);

            int lastVert = _selectMesh.triangles[_selectMesh.triangles.Length - 1];
            List<int> removeList = new List<int>(); //RemoveTriangle(killID, new List<int>(_selectMesh.triangles));

            for (int i = 0, n = _selectMesh.triangles.Length; i < n; i += 3)
            {
                int i1 = _selectMesh.triangles[i];
                int i2 = _selectMesh.triangles[i + 1];
                int i3 = _selectMesh.triangles[i + 2];

                if (killID == 0)
                {
                    if (killID != i1 && killID != i2 && killID != i3)
                        removeList.AddRange(new List<int> { _selectMesh.triangles[i] - 1, _selectMesh.triangles[i + 1] - 1, _selectMesh.triangles[i + 2] - 1 });
                }
                else if (killID == lastVert)
                {
                    if (killID != i1 && killID != i2 && killID != i3)
                        removeList.AddRange(new List<int> { _selectMesh.triangles[i], _selectMesh.triangles[i + 1], _selectMesh.triangles[i + 2] });
                }
                else
                {
                    if (killID != i && killID != i + 1 && killID != i + 2)
                    {
                        removeList.AddRange(new List<int> { 
                            _selectMesh.triangles[i]-1, 
                            _selectMesh.triangles[i + 1]-1, 
                            _selectMesh.triangles[i + 2] -1
                        });

                        /*if (removeList[i] < 0) removeList[i] = 0;
                        if (removeList[i + 1] < 0) removeList[i + 1] = 0;
                        if (removeList[i + 2] < 0) removeList[i + 2] = 0;*/
                    }
                //    Debug.Log((_selectMesh.triangles[i] - 1) + " " + (_selectMesh.triangles[i + 1] - 1) + " " + (_selectMesh.triangles[i + 2] - 1));
                }
            }
            for (int i = 0, n = removeList.Count; i < n; i ++)
                if (removeList[i] < 0) removeList[i] = 0;

            _selectMesh.Clear();
            _selectMesh.vertices = nVertices.ToArray();
            _selectMesh.triangles = removeList.ToArray();
        }

        private static List<int> RemoveTriangle(int tindex, List<int> triangles)
        {
            List<int> removeList = new List<int>();
            for (int i = 0, n = triangles.Count; i < n; i += 3)
            {
                if (triangles[i] != tindex &&
                    triangles[i + 1] != tindex &&
                    triangles[i + 2] != tindex)
                {
                    removeList.Add(i);
                }
            }
            return removeList;
        }
        private static bool HasNeighbours(int id, Mesh mesh)
        {
            int t1 = id;
            int t2 = id + 1;
            int t3 = id + 2;

            for (int i = 0, n = mesh.triangles.Length; i < n; i += 3)
            {
                if (id != i)
                {
                    int v1 = i;
                    int v2 = i + 1;
                    int v3 = i + 2;
                    if (v1 == t1 || v2 == t2 || v3 == t3)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static void VERT_remove_2()
        {
            if (curPointIndex.Count > 0)
            {
                // _selection = kSelect.OBJECT;
                Transform root = _selection.transform;
                Mesh _selectMesh = kSelect.MESH;
                int killID = curPointIndex[0];

                if (_selectMesh == null && killID != -1)
                {
                    return;
                }
                Undo.SetSnapshotTarget(_selectMesh, "MeshVertexRemove");

                List<Vector3> removeListV = new List<Vector3>();
                List<int> removeList = new List<int>();

                List<Vector3> nVertices = new List<Vector3>();
                List<Vector2> nUvs = new List<Vector2>();
                List<int> nTrias = new List<int>();


                for (int i = 0, n = _selectMesh.triangles.Length; i < n; i += 3)
                {

                    if (_selectMesh.triangles[i] == killID ||
                        _selectMesh.triangles[i + 1] == killID ||
                        _selectMesh.triangles[i + 2] == killID)
                    {
                        removeList.Add(i);
                        // Debug.Log("kill tri " + i +" "  );

                        /*  if (_selectMesh.triangles[i] == killID)
                          {
                           
                              removeListV.Add(_selectMesh.vertices[_selectMesh.triangles[i]]);
                              //if (_selectMesh.triangles[i + 1] == killID) 
                              removeListV.Add(_selectMesh.vertices[_selectMesh.triangles[i + 1]]);
                             // if (_selectMesh.triangles[i + 2] == killID) 
                              removeListV.Add(_selectMesh.vertices[_selectMesh.triangles[i + 2]]);
                          }*/
                    }
                }
                int inn = 0;
                foreach (int vi in removeList)
                {
                    Debug.Log(inn + "  [ " + _selectMesh.triangles[vi] + ", " + _selectMesh.triangles[vi + 1] + ", " + _selectMesh.triangles[vi + 2] + " ]");
                    inn++;
                }

                inn = 0;
                /*  for (int i = 0, n = _selectMesh.triangles.Length; i < n; i+=3)
                  {
                    
                     // Debug.Log(" Check if  " + i + " is odd : " + _selectMesh.triangles[i * 3] + " ? " + removeList.Contains(_selectMesh.triangles[i * 3])); 
                      if (!removeList.Contains(_selectMesh.triangles[i * 3])||
                          !removeList.Contains(_selectMesh.triangles[i * 3 +1 ]) ||
                          !removeList.Contains(_selectMesh.triangles[i * 3 +2]))
                      {
                          Debug.Log("Add old " + i);
                          nVertices.Add(_selectMesh.vertices[i]);
                      }
                  }
                  nVertices = new List<Vector3>(_selectMesh.vertices);
                  foreach( Vector3 v in removeListV ){
                      nVertices.Remove(v);
                  }
                  int inn = 0;
                  foreach (Vector3 v in nVertices)
                  {
                      Debug.Log(inn + " " + v);
                      inn++;
                  }
                  int ia = 0;

                  for (int i = 1, n = nVertices.Count; i < n; i++)
                  {
                      Debug.DrawLine(nVertices[i], nVertices[i - 1]);
                      Debug.Log("nVerts " + i + " " + nVertices[i - 1] + " - " + nVertices[i]);
                  }*/

                // curPointIndex.RemoveAt(0);
                nVertices.Add(_selectMesh.vertices[1]);
                nVertices.Add(_selectMesh.vertices[2]);
                nVertices.Add(_selectMesh.vertices[3]);
                nVertices.Add(_selectMesh.vertices[5]);
                nVertices.Add(_selectMesh.vertices[6]);
                nVertices.Add(_selectMesh.vertices[7]);


                Vector2[] uvs = new Vector2[nVertices.Count];
                inn = 0;
                foreach (Vector3 v in nVertices)
                {
                    uvs[inn] = new Vector2(v.x, v.z);
                    inn++;
                }
                _selectMesh.Clear();
                _selectMesh.vertices = nVertices.ToArray();
                _selectMesh.uv = uvs;
                _selectMesh.triangles = new int[6] { 0, 1, 3, 2, 4, 5 };

                _selectMesh.RecalculateNormals();
                _selectMesh.RecalculateBounds();


            }
        }




        private static void VERT_remove_old()
        {
            if (curPointIndex.Count > 0)
            {
                // _selection = kSelect.OBJECT;
                Transform root = _selection.transform;
                Mesh _selectMesh = kSelect.MESH;
                if (_selectMesh == null)
                {
                    return;
                }

                Undo.SetSnapshotTarget(_selectMesh, "MeshVertexRemove");

                //
                int killID = curPointIndex[0];
                List<int> nList = new List<int>();
                List<int> oList = new List<int>();
                // find the neighboure tris
                for (int i = 0, n = _selectMesh.triangles.Length; i < n; i += 3)
                {

                    //             Debug.Log(i+ " --- " + _selectMesh.triangles[i] + " " + _selectMesh.triangles[i+1] + " " + _selectMesh.triangles[i+2]);
                    if (_selectMesh.triangles[i] == killID ||
                        _selectMesh.triangles[i + 1] == killID ||
                        _selectMesh.triangles[i + 2] == killID)
                    {
                        Debug.Log("Add " + i + " / " + _selectMesh.triangles[i]);
                        nList.Add(i);
                    }
                    else
                    {
                        oList.Add(i);
                    }
                }

                List<Vector2> verts2D = new List<Vector2>();
                foreach (int k in nList)
                {
                    Vector3[] pLine = new Vector3[]{  
                        root.TransformPoint( _selectMesh.vertices[_selectMesh.triangles[k]] ),
                        root.TransformPoint( _selectMesh.vertices[_selectMesh.triangles[k+1] ] ),
                        root.TransformPoint( _selectMesh.vertices[_selectMesh.triangles[k+2] ]) };

                    if (k != killID) verts2D.Add(new Vector2(_selectMesh.vertices[_selectMesh.triangles[k]].x, _selectMesh.vertices[_selectMesh.triangles[k]].z));
                    if (k + 1 != killID) verts2D.Add(new Vector2(_selectMesh.vertices[_selectMesh.triangles[k + 1]].x, _selectMesh.vertices[_selectMesh.triangles[k + 1]].z));
                    if (k + 2 != killID) verts2D.Add(new Vector2(_selectMesh.vertices[_selectMesh.triangles[k + 2]].x, _selectMesh.vertices[_selectMesh.triangles[k + 2]].z));

                    // Debug.Log("Draw : " + pLine[0] + " " +pLine[1] + " " +pLine[2]);
                    /*     Debug.DrawLine(pLine[0], pLine[1]);
                         Debug.DrawLine(pLine[2], pLine[1]);
                         Debug.DrawLine(pLine[0], pLine[2]);

     */
                    // Handles.DrawAAPolyLine(pLine);
                }
                foreach (int k in oList)
                {
                    Vector3[] pLine = new Vector3[]{  
                        root.TransformPoint( _selectMesh.vertices[_selectMesh.triangles[k]] ),
                        root.TransformPoint( _selectMesh.vertices[_selectMesh.triangles[k+1] ] ),
                        root.TransformPoint( _selectMesh.vertices[_selectMesh.triangles[k+2] ]) };

                    // Debug.Log("Draw : " + pLine[0] + " " +pLine[1] + " " +pLine[2]);
                    /*     Debug.DrawLine(pLine[0], pLine[1], Color.yellow);
                         Debug.DrawLine(pLine[2], pLine[1], Color.yellow);
                         Debug.DrawLine(pLine[0], pLine[2], Color.yellow);*/

                    if (!verts2D.Contains(new Vector2(_selectMesh.vertices[_selectMesh.triangles[k]].x, _selectMesh.vertices[_selectMesh.triangles[k]].z)))
                    {
                        verts2D.Add(new Vector2(_selectMesh.vertices[_selectMesh.triangles[k]].x, _selectMesh.vertices[_selectMesh.triangles[k]].z));
                    }
                    if (!verts2D.Contains(new Vector2(_selectMesh.vertices[_selectMesh.triangles[k + 1]].x, _selectMesh.vertices[_selectMesh.triangles[k + 1]].z)))
                    {
                        verts2D.Add(new Vector2(_selectMesh.vertices[_selectMesh.triangles[k + 1]].x, _selectMesh.vertices[_selectMesh.triangles[k + 1]].z));
                    }
                    if (!verts2D.Contains(new Vector2(_selectMesh.vertices[_selectMesh.triangles[k + 2]].x, _selectMesh.vertices[_selectMesh.triangles[k + 2]].z)))
                    {
                        verts2D.Add(new Vector2(_selectMesh.vertices[_selectMesh.triangles[k + 2]].x, _selectMesh.vertices[_selectMesh.triangles[k + 2]].z));
                    }
                }



                // create 2D vert list to triangulate
                Triangulator tr = new Triangulator(verts2D.ToArray());
                int[] indices = tr.Triangulate();
                Vector3[] vertices = new Vector3[verts2D.Count];
                for (int i = 0; i < vertices.Length; i++)
                {

                    vertices[i] = new Vector3(verts2D[i].x, 0, verts2D[i].y);
                    if (i > 0) Debug.DrawLine(vertices[i], vertices[i - 1], Color.yellow);
                }

                /*

                //      _selectMesh.Clear();
                      _selectMesh.vertices = vertices;
                      _selectMesh.triangles = indices;
                      _selectMesh.RecalculateNormals();
                      _selectMesh.RecalculateBounds();

                      _selection.GetComponent<MeshCollider>().sharedMesh = null;
                      _selection.GetComponent<MeshCollider>().sharedMesh = _selectMesh;
                   */


                /*       List<Vector3> lVerts = new List<Vector3>(_selectMesh.vertices);
                       // delete verts
                       foreach (int id in curPointIndex) lVerts.RemoveAt(id);
                       // rebuild triangles and uvs
                       List<int> lTris = new List<int>(lVerts.Count * 3);
                       int index = 0;
       */

                /*          _selectMesh.Clear();
                          _selectMesh.vertices = verts = vertices;
                          _selectMesh.triangles = indices;
                
                           _selectMesh.RecalculateNormals();
                           _selectMesh.RecalculateBounds();

                           _selection.GetComponent<MeshCollider>().sharedMesh = null;
                           _selection.GetComponent<MeshCollider>().sharedMesh = _selectMesh;*/

            }
        }



    }//kP_edit end


    public class PLANAR_HELPER
    {
        public bool
            x_Axis = false,
            y_Axis = false,
            z_Axis = false;
    }
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
  }*/