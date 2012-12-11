using UnityEngine;
using System;
using System.Collections.Generic;


public class GridManager {
	
	private MeshBoard meshBoard = null;
	
	private Tile[,] squares;
    public  Tile[,] Squares  { get { return squares; } set { squares = value; } }
	
	private List<Point> path;
	public  List<Point> Route{ get { return path; } } 
		
	
	private Point[] MOVEMENTS = null;
	
	
	public GridManager( MeshBoard meshboard, int typ )
    {
		meshBoard = meshboard;
			
        InitMovements( typ );
    }

	
	public void Pathfind()
    {
        // Find path from hero to monster. First, get coordinates of hero.
       
		Point startingPoint = FindCode( TileContent.Finish );
       
		int pointX = startingPoint.x;
        int pointY = startingPoint.y;
        if (pointX == -1 || pointY == -1) return;
     
        //starts at distance of 0.
         
        squares[ pointX, pointY ].DistanceSteps = 0;

        while (true)
        {
            bool madeProgress = false;

            // Look at each square on the board.
            
            foreach ( Point mainPoint in AllSquares() )
            {
                int x = mainPoint.x;
                int y = mainPoint.y;

                // If the square is open, look through valid moves given the coordinates of that square.
                 
                if ( SquareOpen(x, y) )
                {
                    int passHere = squares[ x, y ].DistanceSteps;

                    foreach ( Point movePoint in ValidMoves( x, y ))
                    {
                        int newX = movePoint.x;
                        int newY = movePoint.y;
                        int newPass = passHere + 1;

                        if( squares[newX, newY].DistanceSteps > newPass )
                        {
                            squares[newX, newY].DistanceSteps = newPass;
                            madeProgress = true;
                        }
                    }
                }
            }
            if (!madeProgress)
            {
                break;
            }
        }
    }	
	
	public void HighlightPath()
    {

		path = new List<Point>();
		
        Point startingPoint = FindCode( TileContent.Start );
        int pointX = startingPoint.x;
        int pointY = startingPoint.y;
        if (pointX == -1 && pointY == -1) return;
		
		while (true)
        {
            // Look through each direction and find the square with the lowest number of steps marked.
          
            Point lowestPoint = new Point(0,0);
            int lowest = 10000;

            foreach ( Point movePoint in ValidMoves( pointX, pointY ) )
            {
                int count = squares[ movePoint.x, movePoint.y ].DistanceSteps;
                if (count < lowest){
                 
					lowest = count;
                    lowestPoint.x = movePoint.x;
                    lowestPoint.y = movePoint.y;
                }
            }
            if (lowest != 10000){
                // Mark the square as part of the path if it is the lowest number. 
				// Set the current position as the square with that number of steps.
				
                squares[ lowestPoint.x, lowestPoint.y ].IsPath = true;
                pointX = lowestPoint.x;
                pointY = lowestPoint.y;
				
				path.Add(  new Point( pointX, pointY ) );
				squares[ pointX, pointY ].SetColor( Color.cyan );
            }else{
                break;
            }

            if ( squares[ pointX, pointY ].ContentCode == TileContent.Finish )
            {
                //We went from monster to hero, so we're finished.
               
                break;
            }
        }
	
	//	if( path != null ) Route = path;
    }
	
	
	public Point GetNextPoint( Point point )
	{
		Point startingPoint = point;
        int pointX = startingPoint.x;
        int pointY = startingPoint.y;
        if ( pointX == -1 && pointY == -1 ) return null;
		
	//	Debug.Log("GetNextPoint From >" + startingPoint.Debuger() );
		
		Point lowestPoint = new Point(0,0);
        int lowest = 10000;
		
		foreach (Point movePoint in ValidMoves( pointX, pointY ))
        {
			int count = squares[ movePoint.x, movePoint.y ].DistanceSteps;
            if (count < lowest){
             
				lowest = count;
                lowestPoint.x = movePoint.x;
                lowestPoint.y = movePoint.y;
            }
        }
        if (lowest != 10000){
            // Mark the square as part of the path if it is the lowest number. 
			// Set the current position as the square with that number of steps.
			
            squares[ lowestPoint.x, lowestPoint.y ].IsPath = true;
            pointX = lowestPoint.x;
            pointY = lowestPoint.y;
			
		//	Debug.Log( " RISE TO " + pointX + " "+ pointY );
			
			squares[ pointX, pointY ].SetColor( Color.cyan );
			
			return new Point( pointX, pointY );
			
			
			
        }else{
           // break;
        }

        if ( squares[pointX, pointY].ContentCode == TileContent.Finish )
        {
           Debug.Log(" //We went from monster to hero, so we're finished.");
           return new Point( 20, 7 );
          
			// break;
        }
		
		
		return null;
	}
	
	public bool ExistPath()
    {
        Debug.Log(" // Mark the path from monster to hero.");

		path = new List<Point>();
		
        Point startingPoint = FindCode( TileContent.Start );
        int pointX = startingPoint.x;
        int pointY = startingPoint.y;
//        if (pointX == -1 && pointY == -1) return null;
		
		while (true)
        {
            // Look through each direction and find the square with the lowest number of steps marked.
          
            Point lowestPoint = new Point(0,0);
            int lowest = 10000;

            foreach ( Point movePoint in ValidMoves( pointX, pointY ) )
            {
                int count = squares[ movePoint.x, movePoint.y ].DistanceSteps;
                if (count < lowest){
                 
					lowest = count;
                    lowestPoint.x = movePoint.x;
                    lowestPoint.y = movePoint.y;
                }
            }
            if ( lowest != 10000 ){
                // Mark the square as part of the path if it is the lowest number. 
				// Set the current position as the square with that number of steps.
				
                squares[ lowestPoint.x, lowestPoint.y ].IsPath = true;
                pointX = lowestPoint.x;
                pointY = lowestPoint.y;
				
				path.Add(  new Point( pointX, pointY ) );
			
				meshBoard.SetTileColor( Color.cyan, pointX, pointY);
				
            }else{
                break;
            }

            if ( squares[ pointX, pointY ].ContentCode == TileContent.Finish) 
            {
               Debug.Log(" //We went from monster to hero, so we're finished.");
               
                break;
            }
        }
		
		Debug.Log("PathCount : " + path.Count);
		
		if( path != null ) return true;
		
		return false;
    }
	
	
	public void ClearLogic() 	// Reset some information about the squares.
    {
       foreach ( Point point in AllSquares() )
        {
			int x = point.x;
            int y = point.y;
            squares[ x, y ].DistanceSteps = 10000;
            squares[ x, y ].IsPath = false;
			squares[ x, y ].SetColor( (squares[ x, y ].ContentCode == TileContent.Wall ) ? Color.red : Color.white );
			
        }
    }
	
	public void ClearSquares() 	// Reset every square.
    {

        foreach ( Point point in AllSquares() )
        {
            squares[ point.x, point.y ] = GenerateTile( point.x, point.y );			//new Tile();
        }
    }

	private Tile GenerateTile( int _x, int _y )
	{
		Transform e = MeshBoard.Instantiate( meshBoard.MESH )as Transform;
		e.position = new Vector3( _x - (MeshBoard.WIDTH/2), -.8f, _y - (MeshBoard.HEIGHT/2));
		
		Tile tile = e.GetComponent<Tile>();
		     tile.Init( meshBoard, _y + _x * MeshBoard.HEIGHT);

		return tile;
	}
	
	
	
	#region MOVEMENTS
	
    public void InitMovements( int movementCount )
    { 

        if (movementCount == 4) {
            MOVEMENTS = new Point[]
            {
                new Point( 0, -1),
                new Point( 1,  0),
                new Point( 0,  1),
                new Point(-1,  0)
            };
        }else{
            MOVEMENTS = new Point[]
            {
                new Point(-1, -1),
                new Point( 0, -1),
                new Point( 1, -1),
                new Point( 1,  0),
                new Point( 1,  1),
                new Point( 0,  1),
                new Point(-1,  1),
                new Point(-1,  0)
            };
        }
    }
	
	#endregion
	#region HELPERZ
	
	static private bool ValidCoordinates( int x, int y )
    {
        // Our coordinates are constrained between 0 and 14.
    
        if ( x < 0 )  { return false; }
        if ( y < 0 )  { return false; }
        if ( x > MeshBoard.WIDTH-1  ) { return false; }
        if ( y > MeshBoard.HEIGHT-1 ) { return false; }
        
		return true;
    }

	private static IEnumerable<Point> AllSquares()
    {
        // Return every point on the board in order.
        
        for ( int x = 0; x < MeshBoard.WIDTH; x++ )
        {
            for ( int y = 0; y < MeshBoard.HEIGHT; y++ )
            {
				yield return new Point( x, y );
            }
        }
		
    }
	
	private IEnumerable<Point> ValidMoves( int x, int y )
    {
        // Return each valid square we can move to.
         
        foreach (Point movePoint in MOVEMENTS)
        {
            int newX = x + movePoint.x;
            int newY = y + movePoint.y;

            if ( ValidCoordinates( newX, newY ) && SquareOpen( newX, newY ))
            {
                yield return new Point( newX, newY );
            }
        }
		
    }
	
	private bool SquareOpen( int x, int y )
    {
        // A square is open if it is not a wall.
   
        switch ( squares[ x, y ].ContentCode )
        {
            case TileContent.Empty:
                return true;
            case TileContent.Finish:
                return true;
            case TileContent.Start:
                return true;
            case TileContent.Wall:
            default:
                return false;
        }
		
    }
	
	private Point FindCode( TileContent contentIn )
    {
        // Find the requested code and return the point.
   
        foreach ( Point point in AllSquares() )
        {
            if ( squares[ point.x, point.y ].ContentCode == contentIn)
            {
                return new Point( point.x, point.y );
            }
        }
        return new Point(-1, -1);
    }
	
	
	#endregion
}
