using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshBoard : MonoBehaviour {
	
	public  Transform MESH;
	public  Transform ENEMEY;
	
	public static int WIDTH  = 21;
	public static int HEIGHT = 15;

	
	private List<BotMover2>  Enemys;
	public void RemoveEnemy( BotMover2 enemy )  { Enemys.Remove( enemy ); }
	
	private int	enemysComplete = 0;
	public int  EnemysComplete	{ get { return enemysComplete; } }
	
	private GridManager GRIDMANAGER;
	
	public int DOLLARS = 1000;
	public int POINTS  = 0;
	
	

	private void Awake()
	{
		Enemys = new List<BotMover2>();
		
		GridManager_INIT();
	
	}
	
	
	
	private void OnGUI()
	{
		if(GUI.Button( new Rect( 0, Screen.height - 30, 200, 20), "Spawn Enemy")){

			Transform e = Instantiate( ENEMEY ) as Transform;
			e.gameObject.AddComponent<SphereCollider>();
			BotMover2 bm = e.gameObject.AddComponent<BotMover2>();

			bm.Init();

			Enemys.Add( bm );
			enemysComplete++;
		}
	
	}

	
	
	public void RecalcPath()
	{
		GRIDMANAGER.ClearLogic();
      	GRIDMANAGER.Pathfind();
    // 	GRIDMANAGER.HighlightPath();
        
	//	GRIDMANAGER.DrawBoard( this );
	
	}
	
	private void GridManager_INIT()
	{
		GRIDMANAGER = new GridManager( this, 4 );
		GRIDMANAGER.Squares = new Tile[ WIDTH, HEIGHT ];	
		
		GRIDMANAGER.ClearSquares();
		
		GRIDMANAGER.Squares[  0, 7 ].ContentCode = TileContent.Start;
		GRIDMANAGER.Squares[ 20, 7 ].ContentCode = TileContent.Finish;	
		
		GRIDMANAGER.ClearLogic();
		GRIDMANAGER.Pathfind();
	//	GRIDMANAGER.HighlightPath();
		
	}
	
	public bool IsFinishPoint( Point p ) 
	{
		if( GRIDMANAGER.Squares[ p.x, p.y ].ContentCode == TileContent.Finish ) return true;
		return false;
	}
	
	public Point GetNextPoint( Point p ) 
	{
		return GRIDMANAGER.GetNextPoint( p );
	}
	
	public int PathLength()
	{
		
		return ( GRIDMANAGER.Route != null )? GRIDMANAGER.Route.Count : 0;
		
	}
	
	/*public bool isPath( Point index )
	{
		Debug.Log("--> " + index.Debuger() );
		GRIDMANAGER.Squares[ index.x + (WIDTH/2), index.y+(HEIGHT/2) ].ContentCode = TileContent.Wall;
		
		bool b = GRIDMANAGER.ExistPath();
		
		GRIDMANAGER.Squares[ index.x + (WIDTH/2), index.y+(HEIGHT/2) ].ContentCode = TileContent.Empty;
		
		Debug.Log( (index.x +  (WIDTH/2)) + " | " +  (index.y+(HEIGHT/2)) + " -> " + b);
		
		return b;
	}*/
	
	/*public bool isPath( Point index )
	{
	//	Debug.Log("--> " + index.Debuger() + GRIDMANAGER.Squares[index.x,index.y].Index);// + " " + GRIDMANAGER.Squares[ index.x + (WIDTH/2), index.y+(HEIGHT/2) ].ContentCode);
		bool returner = true;
		
		GRIDMANAGER.Squares[ index.x, index.y ].ContentCode = TileContent.Wall;
		
		GRIDMANAGER.Pathfind();
		GRIDMANAGER.HighlightPath();
		
		List<Point> path = GRIDMANAGER.Route;
	
		if( path.Count>0 ) {
			foreach( Point p in path )
			{
				if( p.x == index.x && p.y == index.y ){
					Debug.Log(p.Debuger() + " " + index.Debuger() );
					returner = false;
				}
			}
		}
		
		GRIDMANAGER.Squares[ index.x, index.y ].ContentCode = TileContent.Empty;
		GRIDMANAGER.Pathfind();
		GRIDMANAGER.HighlightPath();
	
		return returner;
	}*/
	
	public void SetTileColor( Color color, int x, int y )
	{
		GRIDMANAGER.Squares[ x, y ].SetColor( color );
	}
	
}
