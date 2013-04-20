using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace klock.kEditPoly.helper
{
    public class kSelect
    {
        public static GameObject OBJECT
        {
            get
            {
                return Selection.activeGameObject;
            }
        }
        public static MeshFilter MESHFILTER
        {
            get
            {
                GameObject selection = OBJECT;
                return (selection != null && selection.GetComponent<MeshFilter>() != null ? selection.GetComponent<MeshFilter>() : null);
            }
        }
        public static Mesh MESH
        {
            get
            {
                MeshFilter meshFilter = MESHFILTER;
                return (meshFilter != null ?  ((Application.isEditor) ? meshFilter.sharedMesh : meshFilter.mesh) : null);
            }
        }
        public static Material MATERIAL
        {
            get
            {
                MeshFilter meshFilter = MESHFILTER;
                return (meshFilter != null ? meshFilter.renderer : null) ?  
                    ((Application.isEditor ) ? meshFilter.renderer.sharedMaterial : meshFilter.renderer.material)
                    : 
                    null;
            }
            set
            {
                MeshFilter meshFilter = MESHFILTER;
                if (meshFilter != null ? meshFilter.renderer : null) meshFilter.renderer.sharedMaterial = value;
            }
        }
    }
}
