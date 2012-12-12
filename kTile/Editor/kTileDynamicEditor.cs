/* kTileDynamic V.0.1 - 2012 - Paul Knab */
using UnityEngine;
using UnityEditor;
using System.Collections;

namespace klock.kTiles.editors
{
    [System.Serializable]
  //  [CustomEditor(typeof(kTileDynamic)), CanEditMultipleObjects]
    public class kTileDynamicEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            kTileDynamic t = target as kTileDynamic;
            DrawDefaultInspector();

            if (GUI.changed)
            {
                t.MESH_update();
            }

        }

    }
}