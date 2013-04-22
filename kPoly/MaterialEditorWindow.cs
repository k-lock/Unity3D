using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using klock.kEditPoly.helper;

class MaterialEditorWindow : EditorWindow
{
    static MaterialEditorWindow instance;

    bool FOLD_slot = true;
    bool FOLD_sett = true;
    bool FOLD_expo = true;
    List<Material> lMat = null;
    List<Texture2D> lTex = null;
    List<GUIContent> lCon = null;
    int SlotSize = 50;
    int SlotIndex = -1;

    [MenuItem("Window/Material Editor Test")]
    static void ShowWindow()
    {
        instance = (MaterialEditorWindow)EditorWindow.GetWindow(typeof(MaterialEditorWindow), false, "Material Editor Test");
        instance.Show();
        // instance.OnEnable();
        instance.position = new Rect(200, 100, 250, 420);
        instance.minSize = new Vector2(200, 300);
        instance.maxSize = new Vector2(1200, 900);
    }
    void OnEnable()
    {
        OnInspectorUpdate();
    }
    void OnInspectorUpdate()
    {

    }
    void OnGUI()
    {
        if (lCon == null) InitLists();
        FOLD_slot = EditorGUILayout.Foldout(FOLD_slot, "Material Slots");
        if (FOLD_slot && lCon != null) Draw_Mat_Selection();
        FOLD_sett = EditorGUILayout.Foldout(FOLD_sett, "Material Settings");
        if (FOLD_sett && SlotIndex != -1 && lCon != null) Draw_Mat_Settings();
        FOLD_expo = EditorGUILayout.Foldout(FOLD_expo, "Material Export");
        if (FOLD_expo && lCon != null) Draw_Mat_Exports();
    }
    void Draw_Mat_Exports()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Asset")) Create_Export(0);
       // if (GUILayout.Button("File")) Create_Export(1);
       // if (GUILayout.Button("...")) Create_Export(2);
        GUILayout.EndHorizontal();
    }
    private static string FOLDER_path = "Assets/";
    void Create_Export(int index)
    {
        if (SlotIndex == -1 && index <2) return;
        switch (index)
        {
            case 0:
            case 1:
                AssetDatabase.CreateAsset(Instantiate( lMat[SlotIndex] ), FOLDER_path + lMat[SlotIndex].name + ".mat");
                break;
            case 2:
                break;
        }
    }
    void Draw_Mat_Settings()
    {
        EditorGUI.BeginChangeCheck();

        Material am = lMat[SlotIndex];
        GUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Name");
        am.name = EditorGUILayout.TextField(am.name);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Shader");
        EditorGUILayout.LabelField(am.shader.name);
        GUILayout.EndHorizontal();

        Shader s = lMat[SlotIndex].shader;
        if (s != null)
        {
            //Debug.Log(s.name);
            //EditorGUILayout.LabelField("sName : " + s.name);
            int n = ShaderUtil.GetPropertyCount(s);
            for (int i = 0; i < n; i++)
            {
                // foreach property in current selected 

                string label = ShaderUtil.GetPropertyDescription(s, i);
                string propertyName = ShaderUtil.GetPropertyName(s, i);

                //Debug.Log(ShaderUtil.GetPropertyType(s, i));
                switch (ShaderUtil.GetPropertyType(s, i))
                {
                    case ShaderUtil.ShaderPropertyType.Range: // float ranges
                        {
                            //GUILayout.BeginHorizontal();
                            float v2 = ShaderUtil.GetRangeLimits(s, i, 1);
                            float v3 = ShaderUtil.GetRangeLimits(s, i, 2);
                            RangeProperty(propertyName, label, v2, v3, am);

                            //GUILayout.EndHorizontal();
                            break;
                        }
                    case ShaderUtil.ShaderPropertyType.Float: // floats

                        FloatProperty(propertyName, label, am);
                        break;

                    case ShaderUtil.ShaderPropertyType.Color: // colors
                        {
                            // Debug.Log(propertyName +" / "+ label);
                            ColorProperty(propertyName, label, am);
                            break;
                        }
                    case ShaderUtil.ShaderPropertyType.TexEnv: // textures
                        {
                            ShaderUtil.ShaderPropertyTexDim desiredTexdim = ShaderUtil.GetTexDim(s, i);
                            TextureProperty(propertyName, label, desiredTexdim, am);
                            //GUILayout.Space(6);
                            break;
                        }
                    case ShaderUtil.ShaderPropertyType.Vector: // vectors
                        {
                            //Debug.Log(label);
                           // VectorProperty(propertyName, label, am);
                            break;
                        }
                    default:
                        {
                            GUILayout.Label("ARGH" + label + " : " + ShaderUtil.GetPropertyType(s, i));
                            break;
                        }
                }
            }
        }
        if (EditorGUI.EndChangeCheck())
        {
            // Debug.Log("EndChangeCheck == true ");
            lMat[SlotIndex] = am;
            UpdateMaterialPreview();
        }
    }
    private static void RangeProperty(string propertyName, string label, float v2, float v3, Material m)
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel(label);
        //GUILayout.Space(3);
        m.SetFloat(propertyName, EditorGUILayout.Slider(m.GetFloat(propertyName), v2, v3));
        GUILayout.EndHorizontal();
    }
    private static void VectorProperty(string propertyName, string label, Material m)
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel(label);
        m.SetVector(propertyName, EditorGUILayout.Vector4Field("",m.GetVector(propertyName)));
        GUILayout.EndHorizontal();
    }
    private static void FloatProperty(string propertyName, string label, Material m)
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel(label);
        m.SetFloat(propertyName, EditorGUILayout.FloatField(m.GetFloat(propertyName)));
        GUILayout.EndHorizontal();
    }
    private static void ColorProperty(string propertyName, string label, Material m)
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel(label);
        m.SetColor(propertyName, EditorGUILayout.ColorField(m.GetColor(propertyName)));
        GUILayout.EndHorizontal();
    }
    private static void TextureProperty(string propertyName, string label, ShaderUtil.ShaderPropertyTexDim desiredTexdim, Material m)
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel(label);
        m.SetTexture(propertyName, EditorGUILayout.ObjectField(m.GetTexture(propertyName), typeof(Texture), true) as Texture);
        GUILayout.EndHorizontal();
    }
    void Draw_Mat_Selection()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("New")) NEW_SELECTION();
        if (GUILayout.Button("Set")) Set_SELECTION();
        if (GUILayout.Button("Get")) GET_SELECTION();
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        for (int i = 0, n = 4; i < n; i++)
        {
            GUI.backgroundColor = (SlotIndex == i) ? Color.green : Color.white;
            if (GUILayout.Button(lCon[i], GUI.skin.box))
            {
                if (SlotIndex != i)
                {
                    FOLD_sett = true;
                    SlotIndex = i;
                }
                else
                {
                    SlotIndex = -1;
                    FOLD_sett = false;
                }
                Repaint();
            }
            GUI.backgroundColor = Color.white;
        }
        GUILayout.EndHorizontal();
    }
    void Set_SELECTION()
    {
        if (SlotIndex == -1) return;
        GameObject go = Selection.activeGameObject;
        if (go != null)
        {
            MeshFilter mf = go.GetComponent<MeshFilter>();
            if (mf != null)
            {
                mf.renderer.sharedMaterial = lMat[SlotIndex];
            }
        }
    }
    void NEW_SELECTION()
    {
        if (SlotIndex == -1) return;

        lMat[SlotIndex] = new Material(kShaderLab.GetShader(0, 0, 1));
        UpdateMaterialPreview();
    }
    static MaterialEditor me = null;
    void GET_SELECTION()
    {
        if (SlotIndex == -1) return;
        GameObject go = Selection.activeGameObject;
        if (go != null)
        {
            MeshFilter mf = go.GetComponent<MeshFilter>();
            if (mf != null)
            {
                lMat[SlotIndex] = mf.renderer.sharedMaterial;
                UpdateMaterialPreview();
            }
        }
    }
    void UpdateMaterialPreview()
    {
        me = Editor.CreateEditor(lMat[SlotIndex]) as MaterialEditor;
        Texture2D t = me.RenderStaticPreview("", null, SlotSize, SlotSize) as Texture2D;
        lTex[SlotIndex] = t;
        lCon[SlotIndex] = new GUIContent(t);
    }
    void InitLists()
    {
        Debug.Log(" InitLists() -----------------------------");

        lMat = new List<Material>(4) {  new Material(kShaderLab.GetShader(0, 0, 1)), new Material(kShaderLab.GetShader(0, 0, 1)), new Material(kShaderLab.GetShader(0, 0, 1)), new Material(kShaderLab.GetShader(0, 0, 1)) };
        MaterialEditor me = Editor.CreateEditor(lMat[0]) as MaterialEditor;
        Texture2D t = me.RenderStaticPreview("", null, SlotSize, SlotSize) as Texture2D;
        lTex = new List<Texture2D>(4) { t, t, t, t };
        GUIContent g = new GUIContent(t as Texture);
        lCon = new List<GUIContent>(4) { g, g, g, g };
    }
}
/*
 Material mat;
 Editor matEditor;

 mat = EditorGUILayout.ObjectField(mat, typeof(Material)) as Material;
 if (mat != null)
 {
     if (matEditor == null)
         matEditor = Editor.CreateEditor(mat);

     matEditor.OnPreviewGUI(GUILayoutUtility.GetRect(50, 50), EditorStyles.whiteLabel);
 }*/