//--------------------------------------------------------------------------------------------------------------------------------------//
//                                                                                                                                      //
//     PANEL INFO                                                                                                                       //
//                                                                                                                                      //
//--------------------------------------------------------------------------------------------------------------------------------------//

using UnityEditor;
using UnityEngine;
using klock.kEditPoly.helper;

namespace klock.kEditPoly.panels
{
    public class KP_info
    {
        public static bool _SHOW_TRIAS = true;
        public static bool _SHOW_NEIBS = false;
        public static bool _SHOW_DHANS = false;

        public static void DRAW_PANEL()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.BeginVertical();
            GUILayout.Space(5);

            GameObject _selection = kSelect.OBJECT;
            Mesh _selectMesh = kSelect.MESH;
            // current selection idendifier
            GUI.enabled = (_selection != null);

            EditorGUILayout.ObjectField("Selection ", _selection, typeof(GameObject), true);
            EditorGUILayout.LabelField("Mesh Name : " + (_selection != null && _selectMesh != null ? _selectMesh.name : "none"));
            EditorGUI.indentLevel = 1;
            EditorGUILayout.LabelField("Vertecies : " + (_selection != null && _selectMesh != null ? _selectMesh.vertexCount + " " : "0"));
            EditorGUILayout.LabelField("Triangles : " + (_selection != null && _selectMesh != null ? (_selectMesh.triangles.Length / 3) + " " : "0"));
            EditorGUILayout.LabelField("Faces : " + (_selection != null && _selectMesh != null ? (_selectMesh.vertexCount / 6) + " " : "0"));
            EditorGUILayout.LabelField("SubMeshes : " + (_selection != null && _selectMesh != null ? _selectMesh.subMeshCount : 0));
            EditorGUILayout.Space();
            EditorGUI.indentLevel = 0;
            _SHOW_TRIAS = EditorGUILayout.Toggle("Triangles", _SHOW_TRIAS);
            _SHOW_NEIBS = EditorGUILayout.Toggle("Neigbours", _SHOW_NEIBS);
            _SHOW_DHANS = EditorGUILayout.Toggle("Default Handles", _SHOW_DHANS);
            EditorGUILayout.Space();

            EditorGUILayout.EndVertical();
            GUILayout.Space(10);
            EditorGUILayout.EndHorizontal();

        }
    }
}
