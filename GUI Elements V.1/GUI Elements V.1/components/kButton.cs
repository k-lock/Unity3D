/* klock.components.kButton V1.1 - 06|2012 - Paul Knab */
using UnityEngine;
using klock.drawing;

namespace klock.components
{
    /** A GUI button component.*/
    public class kButton
    {
        /** Color for pressing Button.*/
        public static Color _select = new Color(1, 0.5f, 0, 0.6f);
        /** Color for rolling over Button.*/
        public static Color _over = new Color(1, 0.5f, 0, 0.3f);
        /** Color for rolling out Button.*/
        public static Color _out = new Color(.8f, 0.8f, 0.8f, 0.3f);
        /** Color for enabled mode.*/
        public static Color _enable = new Color(0, 0, 0, 0.3f);

        /**
         * Drawing the component.
         * @param Rect rect         - The position and size for the toogle.
         * @param string labelText  - The displayed label text.
         * @param bool enabled      - Is the component GUI.enabeld or not.  
         * @param bool selected     - Is the button pressed or not.  
         * @param string tip        - The GUIContent.ToolTip for this component.
         * @param bool roller       - Has the component Mouse Roll Over/Out Events.  
         * @returns bool            - Returns true if selected is true else ...*/

        public static bool Draw( Rect rect, string labelText, bool enabled, bool selected, string tip = "", bool roller = false )
        {
           Rect chkRect = new Rect(rect.x + 1, rect.y + 1, rect.width - 2, rect.height - 2);
            Event e = Event.current;

            if (enabled && chkRect.Contains(e.mousePosition))
            {
                if (e.type == EventType.mouseDown)
                {
                    selected = (!selected) ? true : false;
                }
            }

            GUI.Box(rect, "");

            if (selected && enabled) kDraw.FillRect(rect, _select);
            if (roller && enabled && !selected)
            {
                if (chkRect.Contains(e.mousePosition))
                {
                    kDraw.FillRect(rect, _over);
                }
                else
                {
                    kDraw.FillRect(rect, _out);
                }
            }
            else
            {
                if (!enabled) kDraw.FillRect(rect, _enable);
            }

            GUI.Label(new Rect(chkRect.x, chkRect.y - 1, chkRect.width, chkRect.height), new GUIContent(labelText, (enabled && tip != "") ? tip : null));

            return selected;
        }
    }
}

