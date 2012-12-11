/* klock.drawing.kDraw V.3.2 - 06|2012 - Paul Knab */
using UnityEngine;
using UnityEditor;
using System;

namespace klock.drawing
{
	/** Class to draw GUI elements */
	
	public class kDraw
	{
		/** Draw a filled rect at position rectangle.x and rectangle.y with the size of rectangle.width and rectangle.height.
		 *  Using a EditorGUIUtility.whiteTexture to draw.
		 * @param Rect rect The rect with creation values.
		 * @param Color color The wanted color for the fill.*/
		
		public static void FillRect( Rect rect, Color color )
	    {
			Texture2D texture = EditorGUIUtility.whiteTexture;	
			GUI.color = color;
				EditorGUI.DrawPreviewTexture( rect, texture );
			GUI.color = Color.white;
	    }
		/** Draw a filled GUI rect with outlines ( EditorGUIUtility.whiteTexture ) at position rectangle.x and rectangle.y and with the size of rectangle.width and rectangle.height.
		 * Using a EditorGUIUtility.whiteTexture to draw.
		 * @param Rect rect The rect with creation values.
		 * @param Color color The wanted color for the fill.*/

        public static void FillRect(Rect rect, float lineSize, Color rectColor, Color lineColor)
	    {
			FillRect(new Rect( rect.x + lineSize, rect.y + lineSize, rect.width - lineSize*2, rect.height - lineSize*2), rectColor );
			OutRect( rect, lineColor );
	    }
		
		/** Draw the outlines of an GUI rect with the size and position from the parameter rectangle.
		 * Using a EditorGUIUtility.whiteTexture to draw a line .
		 * @param Rect rect The rect with creation values.
		 * @param Color color The wanted color for the line.*/
		public static void OutRect( Rect rect, Color color )
		{
			FillRect(new Rect(  rect.x,  rect.y, 	  rect.width , 1), color );
			FillRect(new Rect(  rect.x,  rect.y+rect.height-1, rect.width , 1), color );
			FillRect(new Rect(  rect.x,  rect.y, 1, rect.height ), color );
			FillRect(new Rect(  rect.x+rect.width-1,  rect.y, 1, rect.height ), color );
		}
		
		/** Draw the outlines of an GUI rect with the size and position from the parameter rectangle.
		 * Using the unity handles component to draw a line.
		 * @param Rect rect The rect with creation values.
		 * @param Color color The wanted color for the line.*/
		public static void OutRect_H( Rect rect, Color color )
	    {
	        Line_H(new Vector2(  rect.x+1,  rect.y+1), new Vector2(  rect.x+1, rect.yMax-1), color );
	        Line_H(new Vector2( rect.xMax,  rect.y+1), new Vector2( rect.xMax, rect.yMax-1), color );
	        Line_H(new Vector2(    rect.x,  rect.y+1), new Vector2( rect.xMax,    rect.y+1), color );
	        Line_H(new Vector2(    rect.x, rect.yMax), new Vector2( rect.xMax,   rect.yMax), color );
	    }
		
		/** Draw a line from Point A to Point B. Using the unity handles component.
		 * @param Vector2 pointA The startpoint of the line.
		 * @param Vector2 pointB The endpoint of the line.
		 * @param Color color The wanted color for the line.*/
		
		public static void Line_H( Vector2 pointA, Vector2 pointB, Color color )
		{
			HandlesColor ( color );
			Handles.DrawLine ( 	new Vector3( pointA.x, pointA.y, 0 ),
								new Vector3( pointB.x, pointB.y, 0 ));
			HandlesColor ( Color.white );
		}
		
		/** Simple setup the unity handles component color 
		 * @param Color color The new color for the component. */
		private static void HandlesColor( Color color ) { Handles.color = color; }

		/** Draw part of texture - WARNING: u2>u1 and v2>v1 */
		public static void DrawTextureClipped( Texture2D texture, Rect rect, float x, float y )
		{
			GUI.BeginGroup( new Rect( x, y, rect.width, rect.height ));
				GUI.Label ( rect, texture );
			GUI.EndGroup ();
			
		}
		
	}
}
		