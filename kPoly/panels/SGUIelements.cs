//--------------------------------------------------------------------------------------------------------------------------------------//
//                                                                                                                                      //
//     SGUIelements                                                                                                                      //
//                                                                                                                                      //
//--------------------------------------------------------------------------------------------------------------------------------------//

using UnityEditor;
using UnityEngine;


namespace klock.kEditPoly.panels
{
    class SGUIelements
    {
        public static bool _toolPreview = false;

        /**--------------------------------------------------------------------------------------
         * 
         *  Weld Tool
         * 
         * --------------------------------------------------------------------------------------
         */

        public static float _toolRadius = .5f;
        //private static float _mintoolRadius = .015f;

        public static void Tool_weld()
        {
            Rect size = new Rect(0, 0, 150, 80);
            string toolName = " Welding Tool";// +KP_edit._editorMode + "s";

            Handles.BeginGUI();
            GUI.enabled = KP_edit.curPointIndex.Count > 0;
            //  Handles.BeginGUI(new Rect(Screen.width - size.width - 10, Screen.height - size.height - 10, size.width, size.height));

            GUI.BeginGroup(new Rect(Screen.width - size.width - 10, Screen.height - size.height - 50, size.width, size.height));
            GUI.Box(size, toolName);

            _toolRadius = Mathf.Clamp(EditorGUILayout.FloatField("Collaps Radius ", _toolRadius), 0, 10);
            if (GUILayout.Button("Apply", GUILayout.Width(140)))
            {
                _toolPreview = true;
            }
            GUI.EndGroup();
            Handles.EndGUI();
            GUI.enabled = true;
            //      if (_toolRadius < 0) { _toolRadius = _mintoolRadius; return; }
        }
        /**--------------------------------------------------------------------------------------
         * 
         *  Weld Tool
         * 
         * --------------------------------------------------------------------------------------
         */
        public static bool _conPre = true;
        public static int _connex = 1;
        public static float _conPad = 0;
        public static void Tool_connect()
        {
            bool guiTemp = GUI.enabled;
            Rect size = new Rect(0, 0, 150, 120);
           
            string toolName = "Edge Connect Tool";// +KP_edit._editorMode + "s";

            Handles.BeginGUI();
            GUI.enabled = KP_edit.curPointIndex.Count > 0;
            //  Handles.BeginGUI(new Rect(Screen.width - size.width - 10, Screen.height - size.height - 10, size.width, size.height));

            GUI.BeginGroup(new Rect(Screen.width - size.width - 10, Screen.height - size.height - 50, size.width, size.height));
            GUI.Box(size, toolName);

            _connex = Mathf.Clamp(EditorGUILayout.IntField("Connections", _connex), 1, 10);
            // GUI.enabled = (_connex > 1);
            float tcp = _conPad;
            _conPad = Mathf.Clamp(EditorGUILayout.FloatField("Con Padding", _conPad), -1, 1);
            if (_conPad != tcp)
            {
                /* 
                 KP_edit.nVerts[0] = KP_edit.nVerts[0] + new Vector3(0, 0, SGUIelements._conPad);
                 KP_edit.nVerts[1] = KP_edit.nVerts[1] + new Vector3(0, 0, SGUIelements._conPad);
                 */
            }
            //  GUI.enabled = guiTemp;
            GUI.color = (_conPre) ? Color.grey : Color.white;
            if (GUILayout.Button("Preview", GUILayout.Width(140)))
            {
                _conPre = !_conPre;
            }
            GUI.color = Color.white;
            if (GUILayout.Button("Apply", GUILayout.Width(140)))
            {
                KP_edit.TOOL_INDEX = -1;
                _toolPreview = true;
                KP_edit.EdgeConnect_Preview(true);
            }
            GUI.EndGroup();
            Handles.EndGUI();
            GUI.enabled = guiTemp;
            //      if (_toolRadius < 0) { _toolRadius = _mintoolRadius; return; }
        }
    }
}
