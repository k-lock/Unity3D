using UnityEngine;
using UnityEditor;
using klock.kEditPoly.panels;
using klock.kEditPoly;
namespace klock.kEditPoly
{
    public class kPoly2Tool : EditorWindow
    {
        #region vars
        public static kPoly2Tool instance;
        private static SceneView.OnSceneFunc _onSceneGUI_ = null;

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
            /*  if (KP_edit._editorMode != MODE.None && KP_edit._freeze)
              {
                  if (Selection.activeGameObject != KP_edit._selection)
                  {
                      Selection.activeGameObject = KP_edit._selection;
                  }
              }*/
        }

        private void OnGUI()
        {
            DrawWindow();
        }

        private void OnSelectionChange()
        {

            if (KP_edit.E_MODE == MODE.None && KP_edit.selection == null)
            {
                KP_edit.selection = kSelect.OBJECT;
                Repaint();
                SceneView.RepaintAll();
            }

        }

        public void OnSceneGUI(SceneView sceneView)
        {
            if (KP_edit.selection == null) return;
            /*Renderer renderer = KP_edit._selection.renderer;
            if (renderer)
                EditorUtility.SetSelectedWireframeHidden(renderer, KP_edit._editorMode != MODE.None && KP.hideWireframe);

            if (renderer && renderer.sharedMaterial == null)
            {
                renderer.sharedMaterial = (Material)AssetDatabase.LoadAssetAtPath("Assets\\klock\\kPoly\\ExampleMaterial.mat", typeof(Material));
            }*/

            // If we are normal, exit now
            if (KP_edit.E_MODE == MODE.None)
                return;

            // Draw SceneGUI Tool elements
            if (KP_edit.TOOL_INDEX != -1)
            {
                switch (KP_edit.TOOL_INDEX)
                {
                    case 1:
                        SGUIelements.Tool_weld();
                        break;
                    case 2:
                        SGUIelements.Tool_connect();
                        KP_edit.EdgeConnect_Preview( );
                        break;
                }
            }

            // This prevents us from selecting other objects in the scene
            int controlID = GUIUtility.GetControlID(FocusType.Passive);
            HandleUtility.AddDefaultControl(controlID);

            /* // Hide and show wireframe if we press control and W
             if (Event.current.control)
             {
                 if (kInputs.KeyDown(KeyCode.W))
                 {
                     Event.current.Use();
                     KP.hideWireframe = !KP.hideWireframe;
                     SceneView.RepaintAll();
                 }
             }*/
            // If we are holding alt, allow normal controls to happen
            if (Event.current.alt)
                return;
            // If we are deleting, call methode and return
            if (kInputs.DeleteKeyDown)
            {
                KP_edit.VerticesRemover();
                return;
            }


            if (KP_edit.FREEZE)
            {
                bool c = false;
                if (kInputs.KeyDown(KeyCode.F1)) { KP_edit.E_MODE = MODE.Point; c = true; }
                if (kInputs.KeyDown(KeyCode.F2)) { KP_edit.E_MODE = MODE.Edge; c = true; }
                if (kInputs.KeyDown(KeyCode.F3)) { KP_edit.E_MODE = MODE.Triangle; c = true; }
                if (kInputs.KeyDown(KeyCode.F4)) { KP_edit.E_MODE = MODE.Quad; c = true; }
                if (kInputs.KeyDown(KeyCode.F5)) { KP_edit.E_MODE = MODE.All; c = true; }
                if (c) { instance.Repaint(); return; }

                KP_edit.Draw_Handles();
                SceneView.currentDrawingSceneView.Repaint();

            }


            if (Event.current.type == EventType.KeyUp) KP_edit.ANY_KEY = false;
            if (Event.current.type == EventType.KeyDown) KP_edit.ANY_KEY = true;

            if (!KP_edit.ANY_KEY && KP_edit.curPointIndex.Count > 0 && kInputs.LeftMouseDown)
            {
                if (!KP_edit.DRAG_CREATE) KP_edit.curPointIndex.Clear();
                if (KP_edit.DRAG_CREATE) KP_edit.DRAG_CREATE = false;
            }

        }

        private void OnDisable()
        {
            instance = null;
            // _onSceneGUI_ = null;
            KP_material.ME_LIST = null;
        }
        private void OnDestroy()
        {
            // _onSceneGUI_ = null;
            instance = null;
        }
        #endregion

        private void DrawWindow()
        {
            /* int controlID = GUIUtility.GetControlID(instance.GetHashCode(), FocusType.Passive);
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
                 case EventType.ValidateCommand:*/

            //kPolyGUI.MAIN_TOOLBAR();
            KP_mainTool.DRAW_BAR();

            switch (MAIN_MENU_ID)
            {
                case 0: KP_create.DRAW_PANEL(); break;
                case 1: KP_edit.DRAW_PANEL(); break;
                case 2: KP_info.DRAW_PANEL(); break;
                case 3: KP_material.DRAW_PANEL(); break;
            }

            /*       break;
           }*/
        }

        public static void SceneEvent(bool state)
        {
            if (state)
            {
                if (_onSceneGUI_ == null)
                {
                    _onSceneGUI_ = new SceneView.OnSceneFunc(instance.OnSceneGUI);
                    SceneView.onSceneGUIDelegate += _onSceneGUI_;
                }
            }
            else
            {
                SceneView.onSceneGUIDelegate -= _onSceneGUI_;
                _onSceneGUI_ = null;

            }
            SceneView.RepaintAll();

        }
    }//class
}//namespace