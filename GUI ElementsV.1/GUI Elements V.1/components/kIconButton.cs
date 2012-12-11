/* klock.components.kIconButton V1.1 - 06|2012 - Paul Knab  */
using UnityEngine;
using klock.drawing;

namespace klock.components
{
    /** A GUI button component with a picture.*/
    public class kIconButton
    {
        /**
         * Drawing the component.
         * @param Rect rect    - The position and size for the toogle.
         * @param bool hover   - Is the mouseposition over the component.
         * @param bool enabled - Is the component GUI.enabeld or not.  
         * @param Texture2D t  - The picture to draw.
         * @param string tip   - The GUIContent.ToolTip for this component.
         * @returns bool       - Returns true if selected is true else ...*/

        public static bool Draw(Rect rect, bool hover, bool enabled, Texture2D t, string tip)
        {
            bool select = false;
            Event e = Event.current;
            if (rect.Contains(e.mousePosition) && enabled)
            {
                hover = true;
                if (e.isMouse && e.type == EventType.MouseDown && !select) select = true;
            }
            GUI.color = Color.white;
            if (hover)
            {
                kDraw.FillRect(new Rect(rect.x, rect.y - 5, rect.width, rect.height + 10), 1, new Color(1, 1, 1, .3f), new Color(.5f, .5f, .5f, 1));
                GUI.Label(rect, new GUIContent("", tip));
            }
            //      Texture2D t  = IconCatcher.ICON(id);
            //      if (t == null) IconCatcher.ICON_LOAD();

            GUI.color = (hover) ? new Color(.7f, .7f, .7f, 1) : (enabled) ? new Color(.8f, .8f, .8f, 1) : new Color(.9f, .9f, .9f, .5f);
            if (t != null) GUI.DrawTexture(new Rect(rect.x + (25 - t.width * .5f), 0, t.width, t.height), t, ScaleMode.ScaleToFit);
            GUI.color = Color.white;

            return select;

        }
    }
}