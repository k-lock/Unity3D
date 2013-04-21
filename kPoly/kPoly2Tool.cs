using UnityEngine;
using UnityEditor;
using klock.kEditPoly.panels;
using klock.kEditPoly.helper;

namespace klock.kEditPoly
{
    public class kPoly2Tool : EditorWindow
    {
        #region vars
        public static kPoly2Tool instance;
        private SceneView.OnSceneFunc _onSceneGUI_ = null;

        public int MAIN_MENU_ID = 0;


        #endregion
        #region Editor
        /** The Unity EditorWindow start function.*/
        [MenuItem("Window/klock/kMesh/kPoly2Tool %M")]
        public static kPoly2Tool Init()
        {
            instance = (kPoly2Tool)EditorWindow.GetWindow(typeof(kPoly2Tool), false, "Tools2");
            instance.Show();
            // instance.OnEnable();
            instance.position = new Rect(200, 100, 250, 420);
            instance.minSize = new Vector2(200, 300);
            instance.maxSize = new Vector2(1200, 900);

            return instance;
        }
        #endregion
        #region Unity

        private void OnEnable()
        {
            if (instance == null)
            {
                instance = this;
            }

        }

        private void Update()
        {
        }

        private void OnGUI()
        {
            DrawWindow();
        }

        private void OnSelectionChange()
        {
            Repaint();
        }

        public void OnSceneGUI(SceneView sceneView)
        {

        }
        private void OnDisable()
        {
            instance = null;
        }
        private void OnDestroy()
        {
            _onSceneGUI_ = null;
            instance = null;
        }
        #endregion

        private void DrawWindow()
        {
            int controlID = GUIUtility.GetControlID(instance.GetHashCode(), FocusType.Passive);
            Event e = Event.current;
            switch (e.GetTypeForControl(controlID))
            {
                case EventType.mouseDown:
                case EventType.mouseUp:
                case EventType.mouseDrag:
                case EventType.keyUp:
                case EventType.keyDown:
                case EventType.repaint:
                case EventType.layout:
                case EventType.ExecuteCommand:
                case EventType.ValidateCommand:

                    //kPolyGUI.MAIN_TOOLBAR();
                    KP_mainTool.DRAW_BAR();

                    switch (MAIN_MENU_ID)
                    {
                        case 0: KP_create.DRAW_PANEL(); break;
                        case 1: break;
                        case 2: KP_info.DRAW_PANEL(); break;
                        case 3: KP_material.DRAW_PANEL(); break;
                    }

                    break;
            }
        }
    }//class
}//namespace