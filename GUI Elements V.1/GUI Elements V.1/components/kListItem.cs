/* klock.components.kListItem V1.1 - 06|2012 - Paul Knab  */
using UnityEngine;
using klock.drawing;

namespace klock.components
{
    /** A GUI element for a list view.*/
    public class kListItem
    {
        /**
         * Drawing the component.
         * @param Rect rect     - The position and size for the toogle.
         * @param bool enabled  - Is the component GUI.enabeld or not.
         * @param bool selected - Is the toggle on or off.  
         * @param Color color   - The color for the selected Item.
         * @returns bool        - Returns true if selected is true else ...*/

        public static bool Draw(Rect rect, bool selected, Color color) //, bool roller = false
        {
            Rect chkRect = new Rect(40, rect.y - 1, rect.width + 2, rect.height + 2);
            Event e = Event.current;
            bool mDown = false;

            if (chkRect.Contains(e.mousePosition))
            {
                if (e.type == EventType.mouseDown)
                {
                    selected = (!selected) ? true : false;
                    mDown = true;
                }
            }

            if (selected)
            {
                kDraw.FillRect(rect, color);
            }

            return mDown;
        }
    }
}

