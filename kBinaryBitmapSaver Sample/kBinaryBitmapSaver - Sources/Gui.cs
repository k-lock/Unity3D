/* Gui V.1 - 2012 - Paul Knab */
using UnityEngine;
using UnityEditor;

namespace klock.GuiDraw
{
	/** Class to draw simple non filled rectangles */
	
	public class Gui
	{
		/** Draw a GUI rectangle at position rectangle.x and rectangle.y with the size of rectangle.width and rectangle.height.
		 * @param Rect rect The rect with creation values.
		 * @param Vector2 pointB The endpoint of the line.
		 * @param Color color The wanted color for the line.*/
		
		public static void DrawRect( Rect rect, Color color )
	    {
	        DrawLine(new Vector2(    rect.x,    rect.y), new Vector2(    rect.x, rect.yMax), color );
	        DrawLine(new Vector2( rect.xMax,    rect.y), new Vector2( rect.xMax, rect.yMax), color );
	        DrawLine(new Vector2(    rect.x,    rect.y), new Vector2( rect.xMax,    rect.y), color );
	        DrawLine(new Vector2(    rect.x, rect.yMax), new Vector2( rect.xMax, rect.yMax), color );
	    }
		
		/** Draw a GUI line from Point A to Point B.
		 * @param Vector2 pointA The startpoint of the line.
		 * @param Vector2 pointB The endpoint of the line.
		 * @param Color color The wanted color for the line.*/
		
		public static void DrawLine( Vector2 pointA, Vector2 pointB, Color color )
		{
			HandlesColor ( color );
			Handles.DrawLine ( 	new Vector3( pointA.x, pointA.y, 0 ),
								new Vector3( pointB.x, pointB.y, 0 ));
			HandlesColor ( Color.white );
		}
		
		/** Simple setup the unity handles component color 
		 * @param Color color The new color for the component. */
		
		private static void HandlesColor( Color color )
		{
			Handles.color = color;
		}
	}	
}