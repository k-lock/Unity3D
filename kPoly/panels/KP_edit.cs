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
                if (GUILayout.Button(new GUIContent("Complete")))
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
                GUILayout.Button(new GUIContent("Remove"));
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

            verts = _selectMesh.vertices;
            Transform root = _selection.transform;
            float cubeSize = .3f;
            int controlIDBeforeHandle = -1,
                controlIDAfterHandle = -1;
            bool isEventUsedByHandle = false,
                isEventUsedBeforeHandle = false,
                setDirty = false;

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

                    for (int i = 0; i < verts.Length; i++)
                    {
                        Vector3 v1 = root.TransformPoint(verts[i]);

                        controlIDBeforeHandle = GUIUtility.GetControlID(someHashCode, FocusType.Passive);
                        isEventUsedBeforeHandle = (Event.current.type == EventType.used);

                        if (curPointIndex.Contains(i))
                            Handles.color = new Color(Color.red.r, Color.red.g, Color.red.b, .85f);
                        else
                            Handles.color = new Color(Color.green.r, Color.green.g, Color.green.b, .85f);
                        cubeSize = HandleUtility.GetHandleSize(v1) * .3f;
                        Handles.ScaleValueHandle(0, v1, Quaternion.identity, cubeSize, Handles.CubeCap, 0);
                        if (curPointIndex.Contains(i))
                        {
                            verts[i] = root.InverseTransformPoint(Handles.PositionHandle(v1, Quaternion.identity));
                            if (v1 != verts[i] && !setDirty) setDirty = true;
                        }

                        controlIDAfterHandle = GUIUtility.GetControlID(someHashCode, FocusType.Native);
                        isEventUsedByHandle = !isEventUsedBeforeHandle && (Event.current.type == EventType.used);

                        if ((controlIDBeforeHandle < GUIUtility.hotControl && GUIUtility.hotControl < controlIDAfterHandle) || isEventUsedByHandle)
                        {
                            POINT_SELECTION(i);
                        }
                    }
                    break;
                case MODE.Line:


                    break;
                case MODE.Tri:

                    for (int i = 0; i < verts.Length; i++)
                    {
                        Vector3 v1 = root.TransformPoint(verts[_selectMesh.triangles[i * 3]]);
                        Vector3 v2 = root.TransformPoint(verts[_selectMesh.triangles[i * 3 + 1]]);
                        Vector3 v3 = root.TransformPoint(verts[_selectMesh.triangles[i * 3 + 2]]);
                        Vector3 dv = (v1 + v2 + v3) / 3;

                        controlIDBeforeHandle = GUIUtility.GetControlID(someHashCode, FocusType.Passive);
                        isEventUsedBeforeHandle = (Event.current.type == EventType.used);

                        if (curPointIndex.Contains(i))
                            Handles.color = new Color(Color.red.r, Color.red.g, Color.red.b, .85f);
                        else
                            Handles.color = new Color(Color.green.r, Color.green.g, Color.green.b, .85f);

                        cubeSize = HandleUtility.GetHandleSize(v1) * .3f;

                        Handles.DrawPolyLine(new Vector3[4] { v1, v2, v3, v1 });

                       
                        Handles.ScaleValueHandle(0, dv, Quaternion.identity, cubeSize, Handles.DotCap, 0);
                        if (curPointIndex.Contains(i))
                        {

                            Vector3 dm = Handles.PositionHandle(dv, Quaternion.identity);
                            if (dm != dv && !isEventUsedBeforeHandle)
                            {

                                Vector3 d = dm - dv;
                                verts[_selectMesh.triangles[i * 3]] += d;
                                verts[_selectMesh.triangles[i * 3 + 1]] += d;
                                verts[_selectMesh.triangles[i * 3 + 2]] += d;

                                setDirty = true;
                            }
                        }
                       
                        controlIDAfterHandle = GUIUtility.GetControlID(someHashCode, FocusType.Native);
                        isEventUsedByHandle = !isEventUsedBeforeHandle && (Event.current.type == EventType.used);

                        if ((controlIDBeforeHandle < GUIUtility.hotControl && GUIUtility.hotControl < controlIDAfterHandle) || isEventUsedByHandle)
                        {
                            POINT_SELECTION(i);
                        }
                    }
                    break;
                case MODE.All:

                    Vector3 dav = Vector3.zero;
                    for (int i = 0; i < verts.Length; i++)
                    {
                        Vector3 v1 = root.TransformPoint(verts[i]);
                        dav += v1;
                    }
                    dav = dav / verts.Length;
                    cubeSize = HandleUtility.GetHandleSize(dav) * .8f;
                    Handles.ScaleValueHandle(0, dav, Quaternion.identity, cubeSize, Handles.CubeCap, 0);
                    Vector3 mdav = root.InverseTransformPoint(Handles.PositionHandle(dav, Quaternion.identity));
                    if (mdav != dav && !setDirty)
                    {
                        Vector3 d = mdav - dav;
                        for (int i = 0; i < verts.Length; i++)
                        {
                            verts[i] += d;
                        }
                        setDirty = true;
                    }


                    break;
            }


            if (setDirty)
            {
                _selectMesh.vertices = verts;
                _selectMesh.RecalculateNormals();
                _selectMesh.RecalculateBounds();
                _selection.GetComponent<MeshCollider>().sharedMesh = null;
                _selection.GetComponent<MeshCollider>().sharedMesh = _selectMesh;
            }
          
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

    }
    public class PLANAR_HELPER
    {
        public bool
            x_Axis = false,
            y_Axis = false,
            z_Axis = false;
    }
}
