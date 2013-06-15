/* 

kTile Base  V.0.2 - 2013 - Paul Knab 
____________________________________

Description : Sprite Sheets/ TextureAtlas/ Tile 
		
Updates: 
------------------------------------
 14-06-2013
  
 recreate class structure
 
*/
using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class TileBase : MonoBehaviour
{
    public Rect uvRect = new Rect(0, 0, 50, 50);
    public float _width = 1;
    public float _height = 1;
    public TextAnchor _pivot = TextAnchor.LowerLeft;
    public FACING facing = FACING.XY;
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

    protected virtual void Awake()
    {
        //	if( mesh != null) mesh = null;
    }
    protected virtual void Update()
    {
        if (facing == FACING.BB)
            FaceDirection();
    }
    protected virtual void OnEnable()
    {
        MESH_refresh();
    }
    protected virtual void OnDisable()
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
    /** Update only mesh uvs.*/
    public void MESH_uvtexture(Rect rect)
    {
        uvRect = rect;
        mesh.uv = UV_setup();
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
        mesh.vertices = VertSetup2();
        mesh.uv = UV_setup();
        mesh.triangles = new int[6] { 0, 1, 3, 0, 3, 2 };

        // Update mesh
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.Optimize();

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
    /** Setup the align mode for the mesh.*/
    private Vector3[] VertSetup2()
    {
        if (verts == null)
            verts = new Vector3[4];

        int index = 0;
        int _uSegments = 1;
        int _vSegments = 1;

        float xSC = _width / _uSegments;
        float ySC = _height / _vSegments;

        Vector2 pivot = Pivot2D();

        Vector3[] vertices = new Vector3[4];
        Vector2[] uvs = new Vector2[4];

        for (float y = 0.0f; y < 2; y++)
        {
            for (float x = 0.0f; x < 2; x++)
            {
                float dx = x * xSC - _width * .5f - pivot.x;
                float dy = y * ySC - _height * .5f - pivot.y;

                switch (facing)
                {
                    case FACING.XZ:
                        vertices[index] = new Vector3(dx, 0.0f, dy);
                        break;
                    case FACING.BB:
                    case FACING.ZX:
                        vertices[index] = new Vector3(dx, 0.0f, -dy);
                        break;
                    case FACING.YZ:
                        vertices[index] = new Vector3(0.0f, dy, dx);
                        break;
                    case FACING.ZY:
                        vertices[index] = new Vector3(0.0f, dy, -dx);
                        break;
                    case FACING.XY:
                        vertices[index] = new Vector3(dx, dy, 0.0f);
                        break;
                    case FACING.YX:
                        vertices[index] = new Vector3(-dx, dy, 0.0f);
                        break;
                }
                index++;
            }
        }
        return vertices;
    }
    /** Helper method to the current uvRect object. 
     * 	@params returns Rect - The Rectangle object of uvRect.*/
    protected virtual Rect UV_rect()
    {
        return uvRect;
    }
    /** Helper to calculate the uv texure coordinates for the current uvRect.*/
    private Vector2[] UV_setup()
    {

        Vector2 lLeftUV = PixelCoordToUVCoord(new Vector2(uvRect.x, uvRect.y));
        Vector2 UVDimen = PixelSpaceToUVSpace(new Vector2(uvRect.width, -uvRect.height));

        uvs[0] = lLeftUV + UVDimen;
        uvs[1] = lLeftUV + Vector2.up * UVDimen.y;
        uvs[2] = lLeftUV + Vector2.right * UVDimen.x;
        uvs[3] = lLeftUV;

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
    /** Helper method to rotate the mesh facing into the Main Cameras direction.*/
    public void FaceDirection()
    {
        transform.LookAt(Camera.main.transform);

        Vector3 direction = transform.eulerAngles;
        transform.eulerAngles = new Vector3(direction.x + 90, direction.y, direction.z); ;
    }
    /** Holding the directions for mesh.*/
    public enum FACING
    {
        XY = 0,
        YX = 1,
        YZ = 2,
        ZY = 3,
        ZX = 4,
        XZ = 5,
        BB = 6,
    }
    public Vector2 Pivot2D()
    {
        Vector2 p = Vector2.zero;
        float width = _width * .5f;
        float height = _height * .5f;
        switch (_pivot)
        {
            case TextAnchor.UpperLeft:
                p = new Vector2(width, height);
                break;
            case TextAnchor.UpperCenter:
                p = new Vector2(0, height);
                break;
            case TextAnchor.UpperRight:
                p = new Vector2(-width, height);
                break;
            case TextAnchor.MiddleLeft:
                p = new Vector2(width, 0);
                break;
            case TextAnchor.MiddleCenter:
                p = Vector2.zero;
                break;
            case TextAnchor.MiddleRight:
                p = new Vector2(-width, 0);
                break;
            case TextAnchor.LowerLeft:
                p = new Vector2(width, -height);
                break;
            case TextAnchor.LowerCenter:
                p = new Vector2(0, -height);
                break;
            case TextAnchor.LowerRight:
                p = new Vector2(-width, -height);
                break;
        }
        return p;
    }
    #endregion
}