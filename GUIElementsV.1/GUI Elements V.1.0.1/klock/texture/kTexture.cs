/* klock.texture.kTexture V.1.3 - 06|2012 - Paul Knab */
using UnityEngine;
using System;

namespace klock.texture
{
    /** Class to handle with Textures */

    public class kTexture
    {
        /** Creates a clipped copy of the given Texture2D and returns it.
         *
         * @param Texture2D texture - The source texture.
         * @param Rect rect         - The rectangle to show. */
        public static Texture2D Clip(Texture2D texture, Rect rect)
        {
            if (texture == null) return texture;

            int width  = Convert.ToInt16(rect.width);
            int height = Convert.ToInt16(rect.height);
            Texture2D tex = new Texture2D(width, height);//, TextureFormat.RGB24, false);

            float _y = texture.height - rect.height - rect.y;
            //	_y = _y > texture.height ? texture.height : _y;
            //	float _x = rect.x+ width > texture.width ? rect.x = texture.width-width : rect.x;

            //	Debug.Log ( rect.x + " " + (int)_y + " " + width + " " + height);

            Color[] colors = texture.GetPixels((int)rect.x, (int)_y, width, height);

            tex.SetPixels(0, 0, width, height, colors);
            tex.Apply();

            return tex;

        }

        /** Creates a copy of the given Texture2D then coloring and returns it .
         *
         * @param Texture2D texture - The source texture.
         * @param Color color           - The color to tint.
         * @param TextureFormat format  - The format for the returning texture. [ ARG32 - default ] */
        public static Texture2D Tint(Texture2D texture, Color color, TextureFormat format = TextureFormat.ARGB32)
        {
            if (texture == null) return texture;

            int width = Convert.ToInt32(texture.width);
            int height = Convert.ToInt32(texture.height);
            Texture2D tex = new Texture2D(width, height, format, false);

            Color[] colors = texture.GetPixels(0, 0, width, height);

            int n = colors.Length; for (int i = 0; i < n; i++) { colors[i] = color; }

            tex.SetPixels(0, 0, width, height, colors);
            tex.Apply();

            return tex;

        }

        /** Search in the Unity Database Assets for the name string.
         * If one found - the texture is returned.
         * If no one found - null is returning.
         * 
         * @param string name - The for the searching Texture. */
        public static Texture2D FindTexture(string name)
        {
	//		UnityEditor.AssetDatabase.Refresh();
			UnityEditor.EditorUtility.DisplayProgressBar("Search AssetDatabase", name, 100);
			
        //    Texture2D rt = new Texture2D(1024, 1024);
            Texture2D[] ta = Resources.FindObjectsOfTypeAll(typeof(Texture2D)) as Texture2D[];
			
			foreach (Texture2D t in ta)
            {
                if (t.name == name)
                {		
       //    			UnityEditor.EditorUtility.DisplayProgressBar("Refresh AssetDatabase", name, 100);
       //     		UnityEditor.EditorUtility.ClearProgressBar();
					 
					//Debug.Log("Texture" + name + " Found ");
					UnityEditor.EditorUtility.ClearProgressBar();
					return t;   
                }
            }

            UnityEditor.EditorUtility.ClearProgressBar();
			
            return null;
        }

    }
}