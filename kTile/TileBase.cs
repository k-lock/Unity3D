/* 

kTile Base  V.0.1 - 2013 - Paul Knab 
____________________________________

Description : Sprite Sheets/ TextureAtlas/ Tile 
		
*/
using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class TileBase : MonoBehaviour
{
    public Rect uvRect = new Rect(0, 0, 50, 50);
    public float _width = 1;
    public float _height = 1;
    protected Vector3[] verts = new Vector3[4];
    protected Vector2[] uvs = new Vector2[4];
    protected int[] trias = new int[6];
    protected Color[] colors = new Color[4];
    [HideInInspector]
    protected Mesh mesh = null;
    [HideInInspector]
    protected MeshFilter meshFilter = null;
    [HideInInspector]
    protected MeshRenderer meshRenderer;


//#if UNITY_EDITOR
	
	protected virtual void Awake ()
	{
	//	if( mesh != null) mesh = null;
	}

	protected virtual void Update ()
	{

	}

    protected virtual void OnEnable ()
	{
		MESH_refresh ();
	}
	
	protected virtual void OnDisable ()
	{
		//GetComponent<MeshFilter> ().mesh = mesh = null;
		//DestroyImmediate (mesh);
	}

//#endif
    /** Main method to setup the Texture Coordinate Rectangle.
	 * 	@params Rectangle UVrect - The rect for displaying Texture. */
    public void MESH_setup(Rect UVrect)
    {
        _width = UVrect.width / 100;
        _height = UVrect.height / 100;

        uvRect = UVrect;

        MESH_refresh();

    }
    /** Main method for force to update Mesh and Material Texture Coordinates.*/
    public void MESH_refresh()
    {

        Components();
        MeshUpdate();

    }
    /** Recalculate the mesh and texture uv coordinates.*/
    protected virtual void MeshUpdate()
    {
        if (meshRenderer.sharedMaterial == null)
            return;

        // uvRect Setup
        uvRect = UV_rect();

        // Setup mesh
        mesh.Clear();
        mesh.vertices = verts = new Vector3[4] { new Vector3(0, 0, 0), new Vector3(0, _height, 0), new Vector3(_width, _height, 0), new Vector3(_width, 0, 0) };
        mesh.triangles = trias = new int[6] { 0, 1, 3, 3, 1, 2 };
        mesh.uv = uvs = UV_setup();

        // Update mesh
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }
    /** Needed Unity Components Checker
     * 	Check for - MeshRender, MeshFilter and Mesh */
    protected void Components()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
        }
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            meshFilter = gameObject.AddComponent<MeshFilter>();
            //meshRenderer.material = mat;
        }
        if (mesh == null)
        {
            GetComponent<MeshFilter>().mesh = mesh = new Mesh();
            mesh.name = "kMesh";
            //mesh.hideFlags = HideFlags.HideAndDontSave;
        }
        /*if (mat == null) {
            GetComponent<MeshRenderer> ().sharedMaterial = mat = new Material (Shader.Find ("Transparent/Diffuse"));
        }*/
        //meshFilter.mesh = mesh;
    }
    #region HELPER
    /** Helper method to the current uvRect object. 
	 * 	@params returns Rect - The Rectangle object of uvRect.*/
    protected virtual Rect UV_rect()
    {
        return uvRect;
    }
    /** Helper to calculate the uv texure coordinates for the current uvRect.*/
    private Vector2[] UV_setup()
    {
        if (GetComponent<MeshFilter>() != null)
        {
            Vector2 lLeftUV = PixelCoordToUVCoord(new Vector2(uvRect.x, uvRect.y));
            Vector2 UVDimen = PixelSpaceToUVSpace(new Vector2(uvRect.width, -uvRect.height));

            uvs[0] = lLeftUV + Vector2.up * UVDimen.y;
            uvs[1] = lLeftUV;
            uvs[2] = lLeftUV + Vector2.right * UVDimen.x;
            uvs[3] = lLeftUV + UVDimen;
        }

        return uvs;
    }
    /** Helper to translate a pixel space to uv space.*/
    private Vector2 PixelSpaceToUVSpace(Vector2 xy)
    {
        Texture t = meshRenderer.sharedMaterial.mainTexture;

        return new Vector2(xy.x / ((float)t.width), xy.y / ((float)t.height));
    }
    /** Helper to translate a pixel coordinate to uv coordinate.*/
    private Vector2 PixelCoordToUVCoord(Vector2 xy)
    {
        Vector2 p = PixelSpaceToUVSpace(xy);
        p.y = 1.0f - p.y;
        return p;
    }
    #endregion
}