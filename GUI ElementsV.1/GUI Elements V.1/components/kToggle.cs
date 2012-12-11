/* klock.components.kToggle V1.1 - 06|2012 - Paul Knab  */
using UnityEngine;
using klock.drawing;

namespace klock.components
{
    /** A GUI toogle component.*/
    public class kToggle
    {
        /**
         * Drawing the component.
         * @param Rect rect     - The position and size for the toogle.
         * @param bool enabled  - Is the component GUI.enabeld or not.
         * @param bool selected - Is the toggle on or off.  
         * @returns bool        - Returns true if the toggle is on else return value is false.*/

        public static bool Draw(Rect rect, bool enabled, bool selected)
        {
            Color color = new Color(.5f, .5f, .5f);
            Rect drwRect = rect;
            Rect chkRect = new Rect(drwRect.x + 1, drwRect.y + 1, drwRect.width - 2, drwRect.height - 2);
            Event e = Event.current;

            if (enabled && chkRect.Contains(e.mousePosition))
            {
                if (e.type == EventType.mouseDown)
                {
                    selected = !selected;
                }
            }
            kDraw.OutRect(drwRect, color);

            /*	GUI.color = ( GUI.skin.name == "DarkSkin" ) ? Color.white : new Color( .8f,.8f,.8f, .5f );
                    GUI.Box( drwRect, "");
                GUI.color = Color.white;*/

            if (selected && enabled) kDraw.FillRect(new Rect(drwRect.x + 3, drwRect.y + 3, drwRect.width - 6, drwRect.height - 6), color);

            return selected;
        }
    }
}
