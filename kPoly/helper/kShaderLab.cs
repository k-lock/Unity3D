using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace klock.kEditPoly.helper
{
    class kShaderLab
    {
        public static string[] CATEGORY = new string[2] 
    { 
        "Built-in Shader Materials", 
        "Extra" 
    };
        public static string[] FAMILY = new string[6] 
    { 
        "Normal", 
        "Transparent", 
        "Transparent Cutout", 
        "Self-Illuminated", 
        "Reflective",
        "Particles"
        
    };
        public static string[] NORMAL = new string[9] 
    { 
        "VertexLit", 
        "Diffuse", 
        "Specular", 
        "Bumped Diffuse", 
        "Bumped Specular", 
        "Parallax Diffuse", 
        "Parallax Specular", 
        "Decal",
        "Diffuse Detail"
    };
        public static string[] ALPHA = new string[7] 
    {
        "VertexLit", 
        "Diffuse",
        "Specular",
        "Bumped Diffuse",
        "Bumped Specular",
        "Parallax Diffuse",
        "Parallax Specular"
    };
        public static string[] ALPHACUT = new string[6] 
    {
        "VertexLit",
        "Diffuse",
        "Specular",
        "Bumped Diffuse",
        "Bumped Specular",
        "Soft Edge Unlit"
    };
        public static string[] ILLUMIN = new string[7] 
    {   
        "VertexLit",
        "Diffuse",
        "Specular",
        "Bumped Diffuse",
        "Bumped Specular",
        "Parallax Diffuse",
        "Parallax Specular"
    };
        public static string[] REFLECT = new string[9] 
    {   "VertexLit",
        "Diffuse",
        "Specular",
        "Parallax Diffuse",
        "Parallax Specular",
        "Bumped Diffuse",
        "Bumped Specular",
        "Bumped Unlit",
        "Bumped VertexLit"
    };
        public static string[] PARTICLES = new string[8] 
    {   "Additive",
        "Additive (Soft)",
        "Alpha Blended",
        "Alpha Blended Premultiply", 
        "Multiply",
        "Multiply (Double)",
        "VertexLit Blended",
        "~Additive~Multiply"
    };
        private static System.Collections.IDictionary shaderLib = new System.Collections.Hashtable();
        public static Shader GetShader(int cat, int fam, int id)
        {
            string sp = "";
            Shader s = null;
            switch (cat)
            {
                case 0: // Build-In Materials
                    switch (fam)
                    {
                        case 0: sp = NORMAL[id]; break;
                        case 1: sp = "Transparent/" + ALPHA[id]; break;
                        case 2: sp = "Transparent/Cutout/" + ALPHACUT[id]; break;
                        case 3: sp = "Self-Illumin/" + ILLUMIN[id]; break;
                        case 4: sp = "Reflective/" + REFLECT[id]; break;
                        case 5: sp = "Particles/" + PARTICLES[id]; break;
                    }
                    break;
                case 1:
                    // Extra Materials
                    break;
            }
            if (sp != "")
            {
                if (shaderLib[sp] == null)
                {
                    s = Shader.Find(sp);
                    shaderLib[sp] = s;
                }
                else
                {
                    s = shaderLib[sp] as Shader;
                }
            }
            return s;
        }
        public static string[] GetShaderList(int index)
        {
            string[] s = null;
            switch (index)
            {
                case 0: s = NORMAL; break;
                case 1: s = ALPHA; break;
                case 2: s = ALPHACUT; break;
                case 3: s = ILLUMIN; break;
                case 4: s = REFLECT; break;
                case 5: s = PARTICLES; break;
            }
            return s;
        }

        private static Material _dummyMaterial = null;
        public static Shader[] FIND_ALL_SHADERS()
        {

            // Create and cache a dummy material

            if (_dummyMaterial == null)
            {
                _dummyMaterial = new Material(Shader.Find("Diffuse"));
                _dummyMaterial.hideFlags = HideFlags.HideAndDontSave;
            }

            // First ensure that all shader assets are loaded
            UnityEditorInternal.InternalEditorUtility.SetupShaderMenu(_dummyMaterial);

            Shader[] _shaders = (Shader[])UnityEngine.Resources.FindObjectsOfTypeAll(typeof(Shader));
            //int n = _shaders.Length;
            //Debug.Log("Find " + n + " Shaders");
            System.Collections.Generic.List<Shader> ls = new System.Collections.Generic.List<Shader>();
            foreach (Shader s in _shaders)
            {
                if (s != null && s.name != "" && s.isSupported && !s.name.StartsWith("__") && !s.name.Contains("Hidden/"))
                {
                    ls.Add(s);
                }
                //else Debug.Log("Block Shader " + s.name);
            }
            /* n = ls.Count;
            shader = new string[ls.Count];
             for (int i = 0; i < n; i++)
             {
                 shader[i] = ls[i].name;
                 // Debug.Log(i + " " + ls[i].name);
             }

             Debug.Log("Find " + MAT_TYPE_a.Length + " Shaders");*/
            return ls.ToArray();
        }
    }
}