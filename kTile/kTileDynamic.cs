/* kTileDynamic V.0.3 - 2012 - Paul Knab */
using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class kTileDynamic : MonoBehaviour
{

	[HideInInspector]
	public  Rect 			uvRect = new Rect(0,0,100,100);	

	public 	float 			_width  = 1;		
	public 	float 			_height = 1;

	public 	int 			_currentFrame = 0;
	public 	int 			_lastFrame    = 0;
	
	public 	float 			_frameTick = 0.05f;	
	public 	Rect[] 			_frameRects;
	private float 			_frameTime = 0.0f;
	private int 			_frameTemp = 0;	

	private Vector3[] 		verts  = new Vector3[4];
	private Vector2[] 		uvs    = new Vector2[4];
	private int[]			trias  = new int[6];
//	private 	Color[] 		colors = new Color[4];
	
	[HideInInspector]
	public 	Mesh 			mesh;
	[HideInInspector]
	public 	MeshFilter 		meshFilter;
	[HideInInspector]
	public 	MeshRenderer 	meshRenderer;
	
	//[HideInInspector]
	public 	bool 			playAnimation = false;
	//[HideInInspector]
	public 	bool 			looping = false;
//	[HideInInspector]
//	public 		bool 		reverse	= false;
	
	#region UNITY
	#if UNITY_EDITOR
	
	public void Awake ()
	{
		if( mesh != null) mesh = null;
	}

	void Update ()
	{
		if( playAnimation && Application.isPlaying )  Frame_check();
	}

    void OnEnable ()
	{
		
		MESH_refresh ();
		
	}
	
	void OnDisable ()
	{
		GetComponent<MeshFilter> ().mesh = mesh = null;
		DestroyImmediate (mesh);

	}

	#endif
	#endregion
	public void MESH_refresh ()
	{

		Components ();
		MeshUpdate ();
		
	}
	public void Components ()
	{
		meshRenderer = GetComponent<MeshRenderer> ();
		if (meshRenderer == null) {
			meshRenderer = gameObject.AddComponent<MeshRenderer> ();
		}
		MeshFilter meshFilter = GetComponent<MeshFilter> ();
		if (meshFilter == null) {
			meshFilter = gameObject.AddComponent<MeshFilter> ();
			//meshRenderer.material = mat;
		}
		if (mesh == null) {
			GetComponent<MeshFilter> ().mesh = mesh = new Mesh ();
			mesh.name = "kMesh";
			mesh.hideFlags = HideFlags.HideAndDontSave;
		}
		/*if (mat == null) {
			GetComponent<MeshRenderer> ().sharedMaterial = mat = new Material (Shader.Find ("Transparent/Diffuse"));
		}*/
		//meshFilter.mesh = mesh;
	}
	
    void MeshUpdate ()
	{
		if(meshRenderer.sharedMaterial==null)return; 
	
		
		Mesh m = (mesh == null ) ? GetComponent<MeshFilter> ().sharedMesh : mesh ;
		
		Rect fRect 	= FrameRect;
		float width = (fRect.width * _width/100);
		float height = (fRect.height * _height/100);

		// Setup mesh
		m.Clear ();
		m.vertices = verts = new Vector3[4] 
		{ 
			new Vector3 (0, 0, 0), 
			new Vector3 (0, height, 0), 
			new Vector3 (width, height, 0), 
			new Vector3 (width, 0, 0) 
		};
		m.triangles = trias = new int[6] { 0, 1, 3, 3, 1, 2 };
		m.uv = uvs = UV_setup ();

		// Update mesh
		m.RecalculateBounds ();
		m.RecalculateNormals ();
	}

	public void MESH_setup (Rect UVrect)
	{
		MESH_setup (UVrect.width, UVrect.height, UVrect);
	}

	public void MESH_setup (float width, float height, Rect UVrect)
	{
		_width = UVrect.width / 100;
		_height = UVrect.height / 100;

		uvRect = UVrect;

		MESH_refresh ();

	}

	#region Animation 
	
	//int i = 0;
	public void Frame_check()
	{
		_frameTemp = _currentFrame;
		
		Frame_calc();
		
	//	Debug.Log ("-> "+( _frameTemp != _currentFrame ) + " - " + i++);
	
		if( _frameTemp != _currentFrame )
		{
			Frame_draw();
		}	
	}
		
	public void Frame_draw()
	{
		if(_currentFrame < 0 )return;
		if( mesh == null )
		{
			Components();
			return;
		}
		
		Mesh m = GetComponent<MeshFilter> ().mesh = mesh;
		
		UV_setup();
		
		m.Clear();
		m.vertices  = VERTS;
		m.uv 	    = UVS;
	    m.triangles = TRIAS;
	       
		m.RecalculateBounds ();
		m.RecalculateNormals ();

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
					if( _currentFrame > _lastFrame-1)_currentFrame = _lastFrame;
				}
			}
		}
	}

	#endregion 
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
           // Debug.Log(_currentFrame + " " + _lastFrame);
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
	private Vector2[] UV_setup ()
	{
		if (meshRenderer != null) {
			Vector2 lLeftUV = PixelCoordToUVCoord (new Vector2 (FrameRect.x, FrameRect.y));
			Vector2 UVDimen = PixelSpaceToUVSpace (new Vector2 (FrameRect.width, -FrameRect.height));

			uvs [0] = lLeftUV + Vector2.up * UVDimen.y;
			uvs [1] = lLeftUV;
			uvs [2] = lLeftUV + Vector2.right * UVDimen.x;
			uvs [3] = lLeftUV + UVDimen;
	//		Debug.Log (meshRenderer.sharedMaterial.mainTexture);
		}
		
		return uvs;
	}
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

