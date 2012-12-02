using UnityEditor;
using UnityEngine;

namespace klock.snippetz
{
    /** Class to creates a compressed file that contains a collection of assets*/
    public class AssetExport
    {
        /** Builds an asset bundle from the selected objects in the project view. Track dependencies.*/
        [MenuItem("Assets/Build AssetBundle - Track dependencies")]
        public static void ExportAsset()
        {
            string path = EditorUtility.SaveFilePanel("Save object as", "", "Object_", "*.unity3d; *.fbx;");
            if (path.Length != 0)
            {
                Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
                BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets);
                Selection.objects = selection;
            }
        }
        /** Builds an asset bundle from the selected objects in the project view. No dependency tracking. */
        [MenuItem("Assets/Build AssetBundle - No dependency tracking")]
        public static void ExportResourceNoTrack()
        {
            // Bring up save panel
            string path = EditorUtility.SaveFilePanel("Save Resource", "", "New Resource", "*.unity3d; *.fbx;");
            if (path.Length != 0)
            {
                // Build the resource file from the active selection.
                BuildPipeline.BuildAssetBundle(Selection.activeObject, Selection.objects, path);
            }
        }
    }
}
