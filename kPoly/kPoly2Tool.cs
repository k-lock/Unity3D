﻿using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using klock.kEditPoly.panels;
using klock.kEditPoly.helper;

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
            _onSceneGUI_ = new SceneView.OnSceneFunc(OnSceneGUI);
        }

        private void Update()
        {
            if (KP_edit._editorMode != MODE.None)
            {
                if (KP_edit._freeze && Selection.activeGameObject != KP_edit._selection)
                {
                    Selection.activeGameObject = KP_edit._selection;
                }
            }
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
            if (KP_edit._editorMode != MODE.None && KP_edit._freeze)
            {
                KP_edit.Draw_Handles();
            }
        }
        private void OnDisable()
        {
            instance = null;
            _onSceneGUI_ = null;
            KP_material.ME_LIST = null;
        }
        private void OnDestroy()
        {
            _onSceneGUI_ = null;
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
                case 1: KP_edit.DRAW_PANEL(); KP_edit.CHECK_USER_INPUT(); break;
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
                SceneView.onSceneGUIDelegate += _onSceneGUI_;
            }
            else
            {
                SceneView.onSceneGUIDelegate -= _onSceneGUI_;
            }
        }
    }//class
}//namespace