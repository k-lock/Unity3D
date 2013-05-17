using UnityEngine;

namespace klock.kEditPoly
{
    public class KP
    {
        public static void Reset_material()
        {
            Reset_create();

            Color_a = Color_b = Color_c = Color.white;
            _sMaterial = null;
            Mtexture_a = Mtexture_b = Mtexture_c = null;
            Checker_Size = 1;
            
        }
        public static void Reset_create()
        {
            _width = 1;
            _height = 1;
            _depth = 1;

            _uSegments = 2;
            _vSegments = 2;
            _zSegments = 1;

            _pivotIndex = 0;
            _faceIndex = 0;
            _colliderIndex = 1;
            _windinIndex = 2;

            openingAngle = 0f;
            outside = true;
            inside = false;
            _meshName = "kPoly";

            sc1 = Vector2.zero;
            // if cone
            if (MESH_TYPE_INDEX == 2)
            {
                _uSegments = 10;
                _width = 0;
            }
        }
        //------------------------------------------------------------------------- MAIN TOOLBAR
        //------------------------------------------------------------------------- CREATE
        public static bool
            FOLD_para = true,
            FOLD_name = true,
            FOLD_object = true,
            FOLD_create = true,
            FOLD_type = true,
            outside = true,
            inside = false;

        public static int MESH_CART_INDEX = 0;
        public static int MESH_TYPE_INDEX = 1;

        public static string[]
            MESH_CART = new string[2] { "Standard Primitive", "Unity Primitive" },
            MESH_TYPE_a = new string[6] { "Cube", "Plane", "Cone", "Cylinder", "Sphere", "Box" },
            MESH_TYPE_b = new string[5] { "Cube", "Sphere", "Capsule", "Cylinder", "Plane" },
            _pivotLabels = { "UpperLeft", "UpperCenter", "UpperRight", "MiddleLeft", "MiddleCenter", "MiddleRight", "LowerLeft", "LowerCenter", "LowerRight" },
            _windinLabels = { "TopLeft", "TopRight", "ButtomLeft", "ButtomRight" },
            _colliderLabels = { "none", "MeshCollider", "BoxCollider" };


        public static string _meshName = "kPoly";

        public static float
            _width = 3,
            _height = 3,
            _depth = 1,
            openingAngle = 0f;

        public static int
            _uSegments = 2,
            _vSegments = 2,
            _zSegments = 1;

        public static int 
            _pivotIndex = 0,
            _faceIndex = 0,
            _windinIndex = 2,
            _colliderIndex = 1;
        //------------------------------------------------------------------------- EDIT
        //------------------------------------------------------------------------- INFO
        //------------------------------------------------------------------------- MATERIAL
        public static bool 
            FOLD_mSele = true,
            FOLD_mSlot = true;

        public static int 
            MAT_SELE_INDEX = -1,
            MAT_CART_INDEX = 0,
            MAT_FAM_INDEX = 0,
            MAT_TYPE_INDEX = 0;

        public static int Checker_Size = 1;

        public static Color
            Color_a = Color.white,
            Color_b = Color.white,
            Color_c = Color.white;

        public static Texture2D
            Mtexture_a = null,
            Mtexture_b = null,
            Mtexture_c = null;

        //public static Shader Mshader = null;
        public static Vector2 sc1 = Vector2.zero;
        public static Material _sMaterial = null;
        public static string[] _mSLOTS = null;
        //------------------------------------------------------------------------- STATS
        public static bool hideWireframe = false;
    }
}
