/** http://www.k-lock.de  | Paul Knab 
 * 	_______________________________________
 * 	
 * 	kPoly | V.1.0.0 | 08.04.2013
 *  ________________________________________
 * 
 * 	Unity Mesh Helper Class 
 * 	 
 */
using UnityEditor;
using UnityEngine;
using System.Collections;

public class kPoly
{
	/** Static helper to create a ray from mouse position in scene view. [ GUIPointToWorldRay ]
	 * 
	 * 	@params returns Ray - */
	private static Ray HitRay()
	{
		Vector2 mp = Event.current.mousePosition;
		return HandleUtility.GUIPointToWorldRay (mp);
	}
	
	/** Returns the complete raycast hit object from the mouse position.
	 * 
	 * 	@returns RaycastHit hit.collider */
	public static RaycastHit HitTriangleObject ()
	{
		Ray r = HitRay();
		RaycastHit hit;
		if (!Physics.Raycast (r, out hit, float.MaxValue)) {
			return hit;
		}
		return hit;
	}
	/** Returns the hitting mesh collider from the mouse position.
	 * 
	 * 	@returns Collider hit.collider */
	public static Collider HitTriangleCollider ()
	{
		Ray r = HitRay();
		RaycastHit hit;
		if (!Physics.Raycast (r, out hit, float.MaxValue)) {
			return null;
		}
		return hit.collider;
	}
	/** Get the mesh triangle index from the mouse position
	 * 	and the current selected mesh componente of the
	 * 	selection.gameobject.
	 * 
	 * 	@returns int hit.triangleIndex */
	public static int HitTriangle ()
	{
		Ray r = HitRay();
		RaycastHit hit;
		if (!Physics.Raycast (r, out hit, float.MaxValue)) {
			return-1;
		}
		return hit.triangleIndex;
	}
	/** Calculate the triangle index neighbour list.
	 * 
	 * 	@returns TriPoint[] - A list containg the data to draw quads.*/
	public static TriPoint[] Neigbours (Mesh m)
	{
		int n = m.triangles.Length;
		TriPoint[] neigbourList = new TriPoint[n];

		for (int i = 0; i < n; i += 3) {

			int p1 = m.triangles [i];
			int p2 = m.triangles [i + 1];
			int p3 = m.triangles [i + 2];
			
			TriPoint tp = (p1 < n && neigbourList [p1] == null) ? new TriPoint () : neigbourList [p1];
			
			if (p2 + 1 != p3) {
				tp._p2 = p2;
				tp._p3 = p3;
			} else {
				tp._p1 = p2;	
			}
			neigbourList [p1] = tp;
		}
		return neigbourList;
	}
}


/** TriPoint -----------------------------------------------------------------------------------------
 * 
 * 	Paul Knab - k-lock.de - 08.04.2013
 * 	___________________________________
 * 
 * 	
 * 	Helper to define the data of an pair of triangles.
 * 
 */
public class TriPoint
{
	public  int _p1, _p2, _p3 = -1;

	public TriPoint Init (int p1=-1, int p2=-1, int p3=-1)
	{
		if (p1 != -1)
			_p1 = p1;
		if (p2 != -1)
			_p2 = p2;
		if (p3 != -1)
			_p3 = p3;
		
		return this;
	}
	/** Debug Helper.
	 * 
	 * 	@returns string - A string containing the values of all points.
	 * 
	 * */
	public string Trace ()
	{
		return this._p1 + " " + this._p2 + " " + this._p3;	
	}
}

/** EDITOR - STATE MODES -------------------------------------------------------------------------------
 *
 *	Editor state mode enum struct.
 *
 */
public enum MODE
{
	None,
	E_Point,
	E_Line,
	E_Quad,
	E_All,

}