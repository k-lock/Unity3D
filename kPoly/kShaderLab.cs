using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

class kShaderLab
{
    public static string[] CATEGORY = new string[2] 
    { 
        "Built-in Shader Materials", 
        "Extra" 
    };
    public static string[] FAMILY = new string[5] 
    { 
        "Normal", 
        "Transparent", 
        "Transparent Cutout", 
        "Self-Illuminated", 
        "Reflective" 
    };
    public static string[] NORMAL = new string[9] 
    { 
        "Vertex-Lit", 
        "Diffuse", 
        "Specular", 
        "Bumped Diffuse", 
        "Bumped Specular", 
        "Parallax Diffuse", 
        "Parallax Bumped Specular", 
        "Decal",
        "Diffuse Detail"
    };
    public static string[] ALPHA = new string[9] 
    {
        "Vertex-Lit", 
        "Diffuse",
        "Specular",
        "Bumped Diffuse",
        "Bumped Specular",
        "Parallax Diffuse",
        "Parallax Bumped Specular",
        "Decal",
        "Diffuse Detail"};
    public static string[] ALPHACUT = new string[5] 
    {
        "Vertex-Lit",
        "Diffuse",
        "Specular",
        "Bumped Diffuse",
        "Bumped Specular"
    };
    public static string[] ILLUMIN = new string[7] 
    {   
        "Vertex-Lit",
        "Diffuse",
        "Specular",
        "Normal mapped Diffuse",
        "Normal mapped Specular",
        "Parallax Diffuse",
        "Parallax Specular"
    };
    public static string[] REFLECT = new string[9] 
    {   "Vertex-Lit",
        "Diffuse",
        "Specular",
        "Bumped Diffuse",
        "Bumped Specular",
        "Parallax Diffuse",
        "Parallax Specular",
        "Normal Mapped Unlit",
        "Normal mapped Vertex-lit"
    };
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
        }
        return s;
    }

    /*private static Material _dummyMaterial = null;
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
        int n = _shaders.Length;
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
        n = ls.Count;
        shader = new string[ls.Count];
        for (int i = 0; i < n; i++)
        {
            shader[i] = ls[i].name;
            // Debug.Log(i + " " + ls[i].name);
        }

        Debug.Log("Find " + MAT_TYPE_a.Length + " Shaders");
        return _shaders;
    }*/
}

