using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace klock.kEditPoly.style
{
    public class KP_Style
    {
        public static GUIStyle tf_input_center()
        {
            GUIStyle cs = new GUIStyle(EditorStyles.textField);
            cs.alignment = TextAnchor.MiddleCenter;

            return cs;
        }
        public static GUIStyle grid()
        {
            GUIStyle cs = new GUIStyle(GUI.skin.button);
            // cs.fontSize = 9;
            cs.contentOffset = Vector2.zero;
            return cs;
        }

    }
}
