using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BotMover2 : MonoBehaviour
{
	private MeshBoard	meshboard;
	
	private Vector3 	wayPoint = Vector3.zero;
	public 	bool 		move		= false;

//	private float		health		= 1.0f;
	public 	float		speed		= 2.0f;
//	private float		shield		= 1.0f;
	private float		rotSpeed	= 25.0f;


	private Transform 	location;

	private void Awake()
	{
		location = transform;
	}
	
	
	public void Init()// Vector3 nPos)
	{	
		meshboard = Camera.main.GetComponent<MeshBoard>();

		location  = transform;
		location.position = to3D( new Point( 0,7 ) );	

		wayPoint  = to3D( meshboard.GetNextPoint( to2D() ) );

		location.LookAt( wayPoint );
		location.gameObject.active = true;
		location.GetChild(0).gameObject.active = true;
	
		move = true;
	
	}

	private void Update()
	{
		if( !move ) return;
		MoveToTarget();
	}
	
	/*private void OnDrawGizmos()
	{
		if( move )  Gizmos.DrawSphere( 	wayPoint, 0.2f );
	}*/
	
	private void MoveToTarget()
	{
		if( move ){
			
			float waypointDistance = Vector3.Distance( location.position, wayPoint );
			if( waypointDistance <= .05f ){

				if ( meshboard.IsFinishPoint( to2D() ))
				{
					ClearBot();
					
					return;
					
				}else{
					
					ChangeWayPoint();
				}
			}
			
			RotateTowards();
			MoveForward();	
		
		}
	}
	
	private void ChangeWayPoint()
	{
		if( !move ) return;

		Vector3 pos = to3D( meshboard.GetNextPoint( to2D() ));
		Vector3 temp = location.position;
		
		wayPoint = ( pos != null ) ? pos : temp;
	
	}
	
	private void RotateTowards() 
	{
		Quaternion rotation = Quaternion.LookRotation( wayPoint - location.position );
		location.rotation = Quaternion.Slerp( location.rotation, rotation, Time.deltaTime * rotSpeed );
		
	}		
	private void MoveForward()
	{
		transform.Translate( Vector3.forward * Time.deltaTime * speed );
	}
	
	private void ClearBot()
	{
		move = false;
		speed = 0;
					
		meshboard.RemoveEnemy( this );
					
		Destroy( this.gameObject );
	}

	#region HELPERZ
	
	private Vector3 to3D( Point p )
	{
		if( p == null ) p = to2D ();//p = meshboard.GetNextPoint( to2D());
		return new Vector3( p.x - ( MeshBoard.WIDTH/2 ), 0, p.y - ( MeshBoard.HEIGHT/2 ) );	
	}
	
	private Point to2D() 
	{
		return new Point( Mathf.RoundToInt( location.position.x ) + ( MeshBoard.WIDTH /2 ), Mathf.RoundToInt( location.position.z ) + ( MeshBoard.HEIGHT /2 ) );
	}
	
	#endregion
	
}
	

 