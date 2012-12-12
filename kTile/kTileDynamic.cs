/* kTileDynamic V.0.3 - 2012 - Paul Knab */
using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class kTileDynamic : MonoBehaviour
{

	[HideInInspector]
	public     Rect 			uvRect = new Rect(0,0,100,100);	

	public 		float 			_width  = 1;		
	public 		float 			_height = 1;

	public 	int 				_currentFrame = 0;
	public 	int 				_lastFrame    = 0;
	
	public 		float 			_frameTick = 0.05f;	
	public 		Rect[] 			_frameRects;
	private 	float 			_frameTime = 0.0f;
	private   	int 			_frameTemp = 0;	

	private 	Vector3[] 		verts  = new Vector3[4];
	private 	Vector2[] 		uvs    = new Vector2[4];
	private 	int[]			trias  = new int[6];
//	private 	Color[] 		colors = new Color[4];
	
	[HideInInspector]
	public 		Mesh 			mesh;
	[HideInInspector]
	public 		MeshFilter 		meshFilter;
	[HideInInspector]
	public 		MeshRenderer 	meshRenderer;
	
	//[HideInInspector]
	public 		bool 			playAnimation = false;
	//[HideInInspector]
	public 		bool 			looping = false;
//	[HideInInspector]
//	public 		bool 			reverse	= false;
	
	public Mesh MESH_init()
	{
		meshFilter = (MeshFilter)gameObject.GetComponent (typeof(MeshFilter));
		if (meshFilter == null) meshFilter = (MeshFilter)gameObject.AddComponent (typeof(MeshFilter));

		meshRenderer = (MeshRenderer)gameObject.GetComponent (typeof(MeshRenderer));
		if (meshRenderer == null) meshRenderer = (MeshRenderer)gameObject.AddComponent (typeof(MeshRenderer));
		
		Mesh m = new Mesh ();
		m.name = "kTileDynamic";
		m.vertices = VERTS;
		m.uv = UVS;
		m.triangles = TRIAS;	
		m.RecalculateNormals ();
	
		meshFilter.sharedMesh = new Mesh();
		mesh = meshFilter.mesh = m;
		
		m.RecalculateBounds ();	
	
		return m;
	}
	
	public void MESH_update()
	{
	
		SetupArrays( _width, _height, FrameRect);
			
		Mesh m =  ( mesh != null ) ? mesh : MESH_init(); 

		m.Clear();
		m.vertices = VERTS;
		m.uv = UVS;
		m.triangles = TRIAS;
       
		m.RecalculateNormals ();
	//	m.RecalculateBounds (); //-  Assigning triangles will automatically Recalculate the bounding	

	}
	
	private void Awake ()
	{

	}

	private void Update ()
	{
		if( playAnimation && Application.isPlaying )  Frame_check();
	}
	
	/*
	
	V1
	
	public void Awake()
	{
			
		meshFilter   = (MeshFilter)	 gameObject.GetComponent(typeof(MeshFilter));
		meshRenderer = (MeshRenderer)gameObject.GetComponent(typeof(MeshRenderer));
		
		if (meshFilter   == null) meshFilter   = (MeshFilter)  gameObject.AddComponent(typeof(MeshFilter));
		if (meshRenderer == null) meshRenderer = (MeshRenderer)gameObject.AddComponent(typeof(MeshRenderer));

		m = new Mesh();
        m.name = "";
		m.vertices  = VERTS;
		m.uv 	    = UVS;
    	m.triangles = TRIAS;
		m.RecalculateNormals();
	
		meshFilter.sharedMesh = m;
       
		m.RecalculateBounds();	

		if ( meshRenderer.sharedMaterial == null)
		{  
			meshRenderer.sharedMaterial = new Material (Shader.Find("Transparent/Diffuse"));
			meshRenderer.sharedMaterial.mainTexture = UnityEditor.EditorGUIUtility.whiteTexture;
		}
		
	}
	private void Update()
	{
		RUpdate();
	}
	
	private void LateUpdate()
	{
		RLateUpdate();
		
	}
	*/
	
	//int i = 0;
	private void Frame_check()
	{
		_frameTemp = _currentFrame;
		
		Frame_calc();
		
	//	Debug.Log ("-> "+( _frameTemp != _currentFrame ) + " - " + i++);
	
		if( _frameTemp != _currentFrame )
		{
			
			Frame_draw();
			
			//i=0;
		}	
	}
		
	private void Frame_draw()
	{
		if(_currentFrame < 0 )return;

		SetupArrays( _width, _height, FrameRect);
		
		Mesh m =  ( mesh != null ) ? mesh : MESH_init (); 
		
		m.Clear();
		m.vertices  = VERTS;
		m.uv 	    = UVS;
	    m.triangles = TRIAS;
	       
		m.RecalculateNormals();

	}

	private void Frame_calc()
	{
		
		if( playAnimation )
		{
			if( _frameTime < 1.0f )
			{
				_frameTime += _frameTick/10;
			
			}else{
		
				if( _currentFrame == _lastFrame )
				{
					// Animation Ends
					if( looping ) 
					{
						_currentFrame = -1;
						
					}else{
						// stop animation
						playAnimation = false;
					}
					
					return;
					
				}else{
					// Frame reached Step 2 the Next 
					_frameTime = 0.0f;
		
					/*if( reverse )
					{
						_currentFrame = ( _currentFrame > 0 ) ? --_currentFrame :  0;
						
						if( _currentFrame == 0 ) reverse = false;	
		
					}else{
					*/	
						_currentFrame = ( _currentFrame < _lastFrame ) ? ++_currentFrame :  _lastFrame;	
						
					//}
					if( _currentFrame < 0)_currentFrame = 0;
					if( _currentFrame > _lastFrame)_currentFrame = _lastFrame;
				}
			}
		}
	}
	
	public void FrameSetup()
	{
	
		Rect frameRect = FrameRect;

		_width  = frameRect.width/50.0f;
		_height = frameRect.height/50.0f;
			
		uvRect  = frameRect;
			
		SetupArrays( _width, _height, frameRect);

	}
	
	public void SetupArrays( float width, float height, Rect UVrect )
	{
		_width = UVrect.width/100;
		_height = UVrect.height/100;
		
		uvRect = UVrect;
		
		SetupArrays();

	}
	
	private void SetupArrays()
	{
		verts = new Vector3[4]{	new Vector3(0, 0, 0), new Vector3(0, _height, 0), new Vector3(_width, _height, 0), new Vector3(_width, 0, 0) };
		uvs   = new Vector2[4]{ new Vector2(0, 0),    new Vector2(0, 1),          new Vector2(1, 1),               new Vector2(1, 0) };
		
		if( meshRenderer != null )
		{
		
			Vector2 lowerLeftUV  = PixelCoordToUVCoord( new Vector2( uvRect.x, uvRect.y) );
			Vector2 UVDimensions = PixelSpaceToUVSpace( new Vector2( uvRect.width, -uvRect.height) );
	
			uvs[0] = lowerLeftUV + Vector2.up * UVDimensions.y;		 		
			uvs[1] = lowerLeftUV;	 										
			uvs[2] = lowerLeftUV + Vector2.right * UVDimensions.x;			
			uvs[3] = lowerLeftUV + UVDimensions;
			
		}

        trias = new int[6] {0, 1, 3, 3, 1, 2};

	}

	public Vector3[] VERTS 
	{
		get{ return verts; }	
	}
	
	public Vector2[] UVS 
	{
		get{ return uvs; }	
	}
	
	public int[] TRIAS 
	{
		get{ return trias; }	
	}

	public Rect FrameRect
	{
		get{
			Rect r = _frameRects[ _currentFrame ];
			return r;
		}		
	}
	
	public Rect[] FrameRects
	{
		set{ _frameRects = value;}
	}
	
	public void ResetFrameTime()
	{
		_frameTime = 0f;
	}
	
	#region HELPERZ
	
	public Vector2 PixelSpaceToUVSpace(Vector2 xy)
	{
		Texture t = meshRenderer.sharedMaterial.mainTexture;

		return new Vector2(xy.x / ((float)t.width), xy.y / ((float)t.height));
	}
	
	public Vector2 PixelCoordToUVCoord(Vector2 xy)
	{
		Vector2 p = PixelSpaceToUVSpace(xy);
		p.y = 1.0f - p.y;
		
		return p;
	}
	
	#endregion
	
}

