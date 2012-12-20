using System;
using UnityEngine;
using UnityEditor;

namespace klock.drawing
{
    /** Class with static GUI textures.
     * Create the textures and hold it in static objects of type Texture2D */
    public class kGFXTexture
    {
        private static Texture2D triU = null;
        private static Texture2D triD = null;
        private static Texture2D triL = null;
        private static Texture2D triR = null;

        /** Return a texture with a triangle.
         * @param int dir - The direction index for the tiangle.
         * 
         * 0 - Up
         * 1 - Right
         * 2 - Down
         * 3 - Left 
         * 
         * @return Textur2D - The triangle texture. */
        public static Texture2D Triangle( int dir, Color color )
        {
        
            if (dir == 0) { if (triU == null) triU = DrawTriangle( color, 0 ); return triU; }
            if (dir == 2) { if (triD == null) triD = DrawTriangle( color, 2 ); return triD; }
            if (dir == 1) { if (triL == null) triL = DrawTriangle( color, 1 ); return triL; }
            if (dir == 3) { if (triR == null) triR = DrawTriangle( color, 3 ); return triR; }

            return null;
        }
        /** Draw a triangle to a texture and retun it.
        *  @param int size     - The size for texture.// ( default is 6)
        *  @returns Texture2D  - The created texture with the triangle.*/
        public static Texture2D DrawTriangle(Color color, int dir = 0)
        {
            int size = 6; //, 1, 2, 3, 4, 5, 7, 8, 9, 10, 14, 15 };
            Texture2D t = new Texture2D(size, size, TextureFormat.ARGB32, false);

            Vector2[] num = new Vector2[1];
            if (dir == 0) num = new Vector2[] { new Vector2(0, 1), new Vector2(1, 1), new Vector2(2, 1), new Vector2(3, 1), new Vector2(4, 1), new Vector2(5, 1), new Vector2(1, 2), new Vector2(2, 2), new Vector2(3, 2), new Vector2(4, 2), new Vector2(2, 3), new Vector2(3, 3) };
            if (dir == 1) num = new Vector2[] { new Vector2(2, 0), new Vector2(3, 0), new Vector2(1, 1), new Vector2(2, 1), new Vector2(3, 1), new Vector2(4, 1), new Vector2(0, 2), new Vector2(1, 2), new Vector2(2, 2), new Vector2(3, 2), new Vector2(4, 2), new Vector2(5, 2) };
            if (dir == 2) num = new Vector2[] { new Vector2(2, 0), new Vector2(2, 1), new Vector2(2, 2), new Vector2(2, 3), new Vector2(2, 4), new Vector2(2, 5), new Vector2(3, 0), new Vector2(3, 1), new Vector2(3, 2), new Vector2(3, 3), new Vector2(4, 1), new Vector2(4, 2) };
            if (dir == 3) num = new Vector2[] { new Vector2(3, 0), new Vector2(3, 1), new Vector2(3, 2), new Vector2(3, 3), new Vector2(3, 4), new Vector2(3, 5), new Vector2(2, 0), new Vector2(2, 1), new Vector2(2, 2), new Vector2(2, 3), new Vector2(1, 1), new Vector2(1, 2) };

            int n = size;
            for (int j = 0; j < n; j++){
                for (int i = 0; i < n; i++)
                {
                    t.SetPixel(i, j, new Color(1, 1, 1, 0));
                }}

            n = num.Length;
            for (int i = 0; i < n; i++)
            {
                int _x = (int)num[i].x;
                int _y = (int)num[i].y;

                t.SetPixel(_x, _y + 1, color);

            }
            t.Apply();

            return t;
        }
		
		

        /** Create a colored texture and retun it.
		* @param Vector2 size - The size for texture. ( x: Width, y: Height ).
        * @returns Texture2D  - The created texture.*/
		public static Texture2D ColoredTexture( int w, int h, Color color ) 
		{
			if( ColoredEditorTexture != null ) return ColoredEditorTexture;
			//Texture2D t = EditorGUIUtility.whiteTexture;
           Texture2D t = new Texture2D( w, h, TextureFormat.ARGB32, false);

			for (int j = 0; j < w; j++){
            for (int i = 0; i < h; i++)
            {
            	t.SetPixel(i, j, color );
            }}
			
			t.Apply();
			if( ColoredEditorTexture == null ) ColoredEditorTexture = t;

            return t;
		}
		public static Texture2D ColoredEditorTexture = null;
    }
}
