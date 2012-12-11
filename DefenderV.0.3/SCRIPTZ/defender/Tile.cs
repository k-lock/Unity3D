using UnityEngine;
using System.Collections;

public enum TileContent
{
    Empty,
    Start,	
    Finish,
    Wall
};

public class Tile : MonoBehaviour {

	private MeshBoard meshBoard = null;
	
	private int INDEX = -1;
	public  int Index 				{ get { return INDEX; 		  } set { INDEX = value; } 			}
		
	private TileContent CONTENTCODE;
    public  TileContent ContentCode { get { return CONTENTCODE;   } set { CONTENTCODE = value;   }	}

    private int DISTANCESTEPS = 10000;
    public  int DistanceSteps 		{ get { return DISTANCESTEPS; } set { DISTANCESTEPS = value; } 	}

    private bool ISPATH = false;
    public  bool IsPath  			{ get { return ISPATH;        } set { ISPATH = value; } 		}

	
	public void Init( MeshBoard meshboard, int index )
	{
		meshBoard = meshboard;
		
		Index = index;
		CONTENTCODE = TileContent.Empty;
		
		SetColor( Color.white );
	}
	

		
	private bool CheckIf( bool test )	
	{	

		if( ContentCode != TileContent.Start && ContentCode != TileContent.Finish ){
			if( ContentCode == TileContent.Empty ){
			
				if( !ISPATH ){
			
	            	SetColor( Color.red );   
					
					if( test ) {		
						ContentCode = TileContent.Empty;
					}else{
						ContentCode = TileContent.Wall;
						meshBoard.RecalcPath();
					}				
					return true;
						
				}else{
		
					ContentCode = TileContent.Wall;
					meshBoard.RecalcPath();
					
					SetColor( Color.red );   
					
					if(meshBoard.PathLength()>0){
						if( test ) {		
							ContentCode = TileContent.Empty;
							meshBoard.RecalcPath();
						}else{
							ContentCode = TileContent.Wall;
							meshBoard.RecalcPath();
						}
						SetColor( Color.red );   
						
						return true;
					}else{
						
						ContentCode = TileContent.Empty;
						meshBoard.RecalcPath();
							
						return false;
					}		
						
				}
			}else{
				if( !test ) {	
					SetColor( Color.white ); 
					ContentCode = TileContent.Empty; 
				}		
			}
		}
		
		return false;
	}
	
	private void OnMouseDown()
	{ 
		CheckIf( false ); 
		
	}
	private void OnMouseOver()
	{
		if( ContentCode == TileContent.Empty ) CheckIf( true );
	}
	private void OnMouseExit()
	{
		if( ContentCode == TileContent.Empty )  SetColor( ( ISPATH ) ? Color.cyan : Color.white ); 		
	}
	
	public void SetColor( Color color )
	{
		color.a = ( INDEX%2 == 1 ) ?  .3f : .2f;

		Material _material = new Material( Shader.Find( "Transparent/Diffuse" ) );
		renderer.sharedMaterial = _material;
		renderer.sharedMaterial.color = color;
		
	}
	
}
