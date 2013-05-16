using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using klock.kEditPoly.prefs;
using klock.kEditPoly.style;

namespace klock.kEditPoly.panels
{
    public class KP_create
    {
        public static void DRAW_PANEL()
        {
            bool GUI_TEMP = GUI.enabled;
            EditorGUILayout.BeginVertical();
            GUILayout.Space(2);

            // OBJECT CART 
            KP.MESH_CART_INDEX = EditorGUILayout.Popup(KP.MESH_CART_INDEX, KP.MESH_CART);
            // OBJECT TYPE 
            KP.FOLD_object = EditorGUILayout.Foldout(KP.FOLD_object, "Object Types");
            if (KP.FOLD_object)
            {
                int objectTemp = KP.MESH_TYPE_INDEX;
                GUILayout.BeginHorizontal();
                GUILayout.Space(10);
                KP.MESH_TYPE_INDEX = GUILayout.SelectionGrid(KP.MESH_TYPE_INDEX, (KP.MESH_CART_INDEX == 0 ? KP.MESH_TYPE_a : KP.MESH_TYPE_b), 2);
                GUILayout.Space(10);
                GUILayout.EndHorizontal();

                if (objectTemp != KP.MESH_TYPE_INDEX)
                {
                    KP.Reset_create();
                }
            }
            // OBJECT NAME
            KP.FOLD_name = EditorGUILayout.Foldout(KP.FOLD_name, "Object Name");
            if (KP.FOLD_name)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(10);
                GUI.enabled = KP.MESH_TYPE_INDEX != -1;
                KP._meshName = EditorGUILayout.TextField(KP._meshName, KP_Style.tf_input_center());
                GUI.enabled = GUI_TEMP;
                if (GUILayout.Button("ID", GUILayout.Width(24))) 
                { 
                    KP._meshName = "kPoly " + UnityEngine.Random.Range(1, 99); 
                }
                GUILayout.Space(10);
                GUILayout.EndHorizontal();
            }
            // OBJECT PARAMETERS
            KP.FOLD_para = EditorGUILayout.Foldout(KP.FOLD_para, "Parameters");
            if (KP.FOLD_para)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(10);
                GUI.enabled = KP.MESH_TYPE_INDEX != -1;
                if (KP.MESH_CART_INDEX == 0)
                {
                    switch (KP.MESH_TYPE_INDEX)
                    {
                        case 0:
                            CREATE_cube();
                            break;
                        case 1:
                            CREATE_plane();
                            break;
                        case 2:
                            CREATE_cone();
                            break;
                    }
                }
                else
                {
                    EditorGUILayout.LabelField("No editabled properties.", EditorStyles.boldLabel);
                }
                GUILayout.Space(10);
                GUILayout.EndHorizontal();
            }
            // OBJECT CREATION TYPE 
            KP.FOLD_create = EditorGUILayout.Foldout(KP.FOLD_create, "Creation Types");//, folderSkin());
            if (KP.FOLD_create)
            {
                // Editor Button for start mesh creation
                if (GUILayout.Button(new GUIContent("GameObject [ Scene ]")))
                {
                    MeshCreator_gameObject();
                }
                GUILayout.Button(new GUIContent("Mesh Asset [ DataBase ]"));
                GUILayout.BeginHorizontal();
                GUILayout.Button(new GUIContent("Export File [ Folder ]"));
                GUILayout.Button(new GUIContent(".."), GUILayout.Width(18));
                GUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();
            GUI.enabled = GUI_TEMP;
        }
        private static void MeshCreator_gameObject()
        {
            GameObject go = null;
            switch (KP.MESH_CART_INDEX)
            {
                case 0:
                    // "Cube", "Plane", "Cone", "Cylinder", "Sphere", "Box"
                    switch (KP.MESH_TYPE_INDEX)
                    {
                        case 0:
                            go = kPoly.Create_Cube_Object(KP._meshName, KP._uSegments, KP._vSegments, KP._zSegments, KP._width, KP._height, KP._depth);
                            break;
                        case 1:
                            go = kPoly.Create_Plane_Object(KP._meshName, KP._uSegments, KP._vSegments, KP._width, KP._height, KP._faceIndex, KP._windinIndex, KP._pivotIndex, KP._colliderIndex);
                            break;
                        case 2:
                            go = kPoly.Create_Cone_Object(KP._meshName, KP._uSegments, KP._width, KP._depth, KP._height, KP.openingAngle, KP.outside, KP.inside);
                            break;
                    }
                    break;
                case 1:
                    //"Cube", "Sphere", "Capsule", "Cylinder", "Plane"
                    switch (KP.MESH_TYPE_INDEX)
                    {
                        case 0:
                            go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            break;
                        case 1:
                            go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                            break;
                        case 2:
                            go = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                            break;
                        case 3:
                            go = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                            break;
                        case 4:
                            go = GameObject.CreatePrimitive(PrimitiveType.Plane);
                            break;
                    }
                    break;
            }
            if (go != null) Selection.activeGameObject = go;
        }
        private static void CREATE_cone()
        {
            EditorGUILayout.BeginVertical();
            // Editor value reset button
            if (GUILayout.Button(new GUIContent("Reset Editor"), EditorStyles.miniButton))
            {
                KP.Reset_create();
            }
            // if openingAngle>0, create a cone with this angle by setting radiusTop to 0, and adjust radiusBottom according to length;
            EditorGUILayout.Space();
            KP._uSegments = EditorGUILayout.IntField("Segments", KP._uSegments); // numVertices
            EditorGUILayout.Space();
            KP._width = EditorGUILayout.FloatField("Radius Top", KP._width); //radiusTop
            KP._depth = EditorGUILayout.FloatField("Radius Bottom", KP._depth);//radiusBottom
            KP._height = EditorGUILayout.FloatField("Height", KP._height);//length
            EditorGUILayout.Space();
            KP.openingAngle = EditorGUILayout.FloatField("Open Angle", KP.openingAngle);
            EditorGUILayout.Space();
            KP.outside = EditorGUILayout.Toggle("Outside", KP.outside);
            KP.inside = EditorGUILayout.Toggle("Inside", KP.inside);
            EditorGUILayout.Space();

            EditorGUILayout.EndVertical();
        }
        private static void CREATE_cube()
        {
            EditorGUILayout.BeginVertical();
            // Editor value reset button
            if (GUILayout.Button(new GUIContent("Reset Editor"), EditorStyles.miniButton))
            {
                KP.Reset_create();
            }
            EditorGUILayout.Space();
            // Editor value for width and height of the created mesh [ float ]
            KP._width = EditorGUILayout.FloatField("Width", KP._width);
            KP._height = EditorGUILayout.FloatField("Height", KP._height);
            KP._depth = EditorGUILayout.FloatField("Depth", KP._depth);
            EditorGUILayout.Space();
            // Editor value for width and height segments of the created mesh [ int ]
            KP._uSegments = EditorGUILayout.IntField("uSegments", KP._uSegments);
            KP._vSegments = EditorGUILayout.IntField("vSegments", KP._vSegments);
            KP._zSegments = EditorGUILayout.IntField("zSegments", KP._zSegments);
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
        }
        private static void CREATE_plane()
        {
            EditorGUILayout.BeginVertical();
            EditorGUI.BeginChangeCheck();
            GUILayout.BeginHorizontal();
            // Editor value reset button
            if (GUILayout.Button(new GUIContent("Reset Editor"), EditorStyles.miniButton))
            {
                KP.Reset_create();
            }
            GUILayout.EndHorizontal();
            EditorGUILayout.Space();
            // Editor value for width and height of the created mesh [ float ]
            KP._width = EditorGUILayout.FloatField("Width", KP._width);
            KP._height = EditorGUILayout.FloatField("Height", KP._height);
            EditorGUILayout.Space();
            // Editor value for width and height segments of the created mesh [ int ]
            KP._uSegments = EditorGUILayout.IntField("uSegments", KP._uSegments);
            KP._vSegments = EditorGUILayout.IntField("vSegments", KP._vSegments);
            EditorGUILayout.Space();
            GUILayout.BeginHorizontal();
            // Editor value for the pivot point of the created mesh Unity.TextAnchor
            GUILayout.Label("Pivot ");
            GUILayout.Space(18);
            KP._pivotIndex = EditorGUILayout.Popup(KP._pivotIndex, KP._pivotLabels);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            // Editor value for the mesh face direction FACING.XZ
            GUILayout.Label("Facing ");
            GUILayout.Space(10);
            KP._faceIndex = EditorGUILayout.Popup(KP._faceIndex, kPoly.FACING);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            // Editor value for triangle winding order
            GUILayout.Label("Winding");
            GUILayout.Space(2);
            KP._windinIndex = EditorGUILayout.Popup(KP._windinIndex, KP._windinLabels);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            // Editor value for collider export
            GUILayout.Label("Collider ");
            GUILayout.Space(3);
            KP._colliderIndex = EditorGUILayout.Popup(KP._colliderIndex, KP._colliderLabels);
            GUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            // Starting GUI changes check
            if (EditorGUI.EndChangeCheck())
            {
                KP._width = Mathf.Clamp(KP._width, 0, int.MaxValue);
                KP._height = Mathf.Clamp(KP._height, 0, int.MaxValue);
                KP._uSegments = Mathf.Clamp(KP._uSegments, 1, int.MaxValue);
                KP._vSegments = Mathf.Clamp(KP._vSegments, 1, int.MaxValue);
                //   Debug.Log("Change Editor");
            }
            EditorGUILayout.EndVertical();
        }
    }
}
