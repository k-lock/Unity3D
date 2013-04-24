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

        static List<int> curPointIndex = new List<int>();
        static Vector3[] verts;

        public static void DRAW_PANEL()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.BeginVertical();
            GUILayout.Space(5);

            _selection = kSelect.OBJECT;
            Mesh _selectMesh = kSelect.MESH;
            GUI.enabled = (_selection != null && _selectMesh != null);
            //-------------------------------------------------------------------
            //  SELECTION
            FOLD_selection = EditorGUILayout.Foldout(FOLD_selection, "Selection ");// + ((_selection != null) ? "[ " + _selection.name + " ]" : ""));
            if (FOLD_selection)
            {
                GUILayout.BeginHorizontal();
                GUI.color = (_editorMode == MODE.E_Point) ? new Color(0, .5f, 1, .7f) : Color.white;
                if (GUILayout.Button(new GUIContent("Point")))
                {
                    _editorMode = (_editorMode == MODE.E_Point) ? MODE.None : MODE.E_Point;

                    if (_selection != null)
                    {
                        _freeze = !_freeze;

                        if (_editorMode == MODE.E_Point)
                        {
                            _selection.hideFlags |= HideFlags.NotEditable;
                            kPoly2Tool.SceneEvent(true);
                        }
                        else
                        {
                            _selection.hideFlags = 0;
                            kPoly2Tool.SceneEvent(false);
                        }
                    }
                }
                GUI.color = Color.white;
                GUI.color = (_editorMode == MODE.E_Line) ? new Color(0, .5f, 1, .7f) : Color.white;
                if (GUILayout.Button(new GUIContent("Line")))
                {
                    _editorMode = (_editorMode == MODE.E_Line) ? MODE.None : MODE.E_Line;

                    if (_selection != null)
                    {
                        _freeze = !_freeze;

                        if (_editorMode == MODE.E_Line)
                        {
                            _selection.hideFlags |= HideFlags.NotEditable;
                            kPoly2Tool.SceneEvent(true);
                        }
                        else
                        {
                            _selection.hideFlags = 0;
                            kPoly2Tool.SceneEvent(false);
                        }
                    }
                }
                GUI.color = Color.white;
                GUI.color = (_editorMode == MODE.E_Quad) ? new Color(0, .5f, 1, .7f) : Color.white;
                if (GUILayout.Button(new GUIContent("Segment")))
                    _editorMode = (_editorMode == MODE.E_Quad) ? MODE.None : MODE.E_Quad;
                GUI.color = Color.white;
                GUI.color = (_editorMode == MODE.E_All) ? new Color(0, .5f, 1, .7f) : Color.white;
                if (GUILayout.Button(new GUIContent("Complete")))
                    _editorMode = (_editorMode == MODE.E_All) ? MODE.None : MODE.E_All;
                GUI.color = Color.white;
                GUILayout.EndHorizontal();
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
            bool setDirty = false;

            // Point Mode 

            switch (_editorMode)
            {

                case MODE.E_Point:
                    int someHashCode = kPoly2Tool.instance.GetHashCode();
                    for (int i = 0; i < verts.Length; i++)
                    {
                        Vector3 v1 = root.TransformPoint(verts[i]);
                        float cubeSize = HandleUtility.GetHandleSize(v1) * .3f;
                        int controlIDBeforeHandle = GUIUtility.GetControlID(someHashCode, FocusType.Passive);
                        bool isEventUsedBeforeHandle = (Event.current.type == EventType.used);

                        if (curPointIndex.Contains(i))
                            Handles.color = new Color(Color.red.r, Color.red.g, Color.red.b, .85f);
                        else
                            Handles.color = new Color(Color.green.r, Color.green.g, Color.green.b, .85f);

                        Handles.ScaleValueHandle(0, v1, Quaternion.identity, cubeSize, Handles.CubeCap, 0);
                        if (curPointIndex.Contains(i))
                        {
                            verts[i] = root.InverseTransformPoint(Handles.PositionHandle(v1, Quaternion.identity));
                            if (v1 != verts[i] && !setDirty) setDirty = true;
                        }

                        int controlIDAfterHandle = GUIUtility.GetControlID(someHashCode, FocusType.Native);
                        bool isEventUsedByHandle = !isEventUsedBeforeHandle && (Event.current.type == EventType.used);

                        if ((controlIDBeforeHandle < GUIUtility.hotControl && GUIUtility.hotControl < controlIDAfterHandle) || isEventUsedByHandle)
                        {
                            POINT_SELECTION(i);
                        }
                    }
                    break;
                case MODE.E_Line:
                   

                    break;
            }


            if (setDirty)
            {
                _selectMesh.vertices = verts;
                _selectMesh.RecalculateNormals();
                _selectMesh.RecalculateBounds();
            }
        }
        private static void POINT_SELECTION(int i)
        {
            if (!ANY_KEY && curPointIndex.Count > 0) curPointIndex.Clear();
            if (!curPointIndex.Contains(i))
                curPointIndex.Add(i);
            else
                curPointIndex.Remove(i);
        }
        public static void CHECK_USER_INPUT()
        {
            Event e = Event.current;
            ANY_KEY = false;
            if (e.control && e.isKey)
            {
                ANY_KEY = true;
                e.Use();
            }
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
