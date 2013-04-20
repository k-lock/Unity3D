//--------------------------------------------------------------------------------------------------------------------------------------//
//                                                                                                                                      //
//     MAIN TOOL BAR                                                                                                                    //
//                                                                                                                                      //
//--------------------------------------------------------------------------------------------------------------------------------------//

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace klock.kEditPoly.panels
{
    public class KP_mainTool
    {

        /*private static Texture2D[] TOOL_BAR_ICONS = new Texture2D[] {   klock.kLibary.LoadBitmap("create", 20,20),
                                                                    klock.kLibary.LoadBitmap("modify", 20,20),
                                                                    klock.kLibary.LoadBitmap("utility", 20,20),
                                                                    klock.kLibary.LoadBitmap("utility", 20,20)};*/
        private static string[] _BAR_ITEMS = new string[] { "CREATE", "EDIT", "INFO", "MAT" };
        public static void DRAW_BAR()
        {
            kPoly2Tool k2p = kPoly2Tool.instance;
            k2p.MAIN_MENU_ID = GUILayout.Toolbar(k2p.MAIN_MENU_ID, _BAR_ITEMS);

        }
    }
}