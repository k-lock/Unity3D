using UnityEngine;
using System.Collections;

public class Point {

	public int x = 0;
	public int y = 0;
	
	public Point( int _x, int _y )
	{
		x = _x;
		y = _y;		
	}
	
	public string Debuger()
	{
		return "Point [ x : " + x + " y : " + y +" ]"; 	
	}
}
