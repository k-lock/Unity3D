using UnityEngine;
using UnityEditor;

namespace klock
{
    class kInputs
    {
        // Determine if the left mouse button is pressing
        public static bool LeftMouseDown
        {
            get { return Event.current.type == EventType.MouseDown && Event.current.button == 0; }
        }

        // Determine if the left mouse button is releasing
        public static bool LeftMouseUp
        {
            get { return Event.current.type == EventType.MouseUp && Event.current.button == 0; }
        }

        // Determine if we are pressing the delete key
        public static bool DeleteKeyDown
        {
            get { return KeyDown(KeyCode.Delete); }
        }

        // Determine if we are pressing a key
        public static bool KeyDown(KeyCode keyCode)
        {
            return Event.current.type == EventType.KeyDown && Event.current.keyCode == keyCode;
        }

    }
}
