using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using klock.kEditPoly.helper;
using klock.kEditPoly.prefs;
using klock.kEditPoly.style;

namespace klock.kEditPoly.panels
{
    public class KP_material
    {
        private static List<Material> MA_LIST = new List<Material>(4) 
        { 
            new Material(kShaderLab.GetShader(0,0,1)), 
            new Material(kShaderLab.GetShader(0,0,1)), 
            new Material(kShaderLab.GetShader(0,0,1)), 
            new Material(kShaderLab.GetShader(0,0,1)), 
            new Material(kShaderLab.GetShader(0,0,1))
        };
        private static List<MaterialEditor> ME_LIST = null;
        /* new List<MaterialEditor>(4) 
        { 
            new MaterialEditor (), 
            new MaterialEditor (), 
            new MaterialEditor (),
            new MaterialEditor (),
            new MaterialEditor ()
        };*/
        private static float _LABEL_WIDTH = 200f;

        public static void DRAW_PANEL()
        {
            bool GUI_TEMP = GUI.enabled;
            int CART_temp = KP.MAT_CART_INDEX;
            int FAM_temp = KP.MAT_FAM_INDEX;
            int TYP_temp = KP.MAT_TYPE_INDEX;
            if (ME_LIST == null)
            {
                ME_LIST = new List<MaterialEditor>(4);
                Material m = new Material( Shader.Find("Diffuse") );
                MaterialEditor me;
                for (int i = 0, n = 5; i < n; i++)
                {
                    me = Editor.CreateEditor(m) as MaterialEditor;
               
                    me.SetTexture("_mainTexture", kLibary.LoadBitmap("create",25,25));
                    ME_LIST.Add( me);
                }
            }
            //GUI.enabled = (_selection != null);
            // GUILayoutOption glo = {  };
            EditorGUILayout.BeginVertical(); //----------------------------------------------------------> Begin Vertical
            EditorGUI.BeginChangeCheck();
            GUILayout.Space(2);
            // Material operation and selection slots
            KP.FOLD_mSele = EditorGUILayout.Foldout(KP.FOLD_mSele, "Material Operation ");
            if (KP.FOLD_mSele)
            {
                KP.MAT_SELE_INDEX = GUILayout.Toolbar(KP.MAT_SELE_INDEX, new string[] { "Get", "Set", "2file", "2data" });
                //KP.MAT_SELE_INDEX = GUILayout.Toolbar(KP.MAT_SELE_INDEX, new string[] { "MAT I", "MAT II", "MAT III" });
                EditorGUILayout.BeginHorizontal();

                for (int i = 0, n = 4; i < n; i++)
                {
                    GUILayout.BeginVertical();
                    GUILayout.Box(new GUIContent("Slot " + i), GUILayout.ExpandWidth(true), GUILayout.Height(22));
                   // Debug.Log(ME_LIST[i]);
                    MaterialEditor med = ME_LIST[i];

                    if (med && Event.current.type == EventType.layout) med.OnPreviewGUI(GUILayoutUtility.GetRect(45, 45), EditorStyles.whiteLabel);
                    GUILayout.EndVertical();
                    GUILayout.Space(2);
                }

                EditorGUILayout.EndHorizontal();
            }
            KP.FOLD_object = EditorGUILayout.Foldout(KP.FOLD_object, "Shader Family ");
            if (KP.FOLD_object)
            {
                // Material category
                KP.MAT_CART_INDEX = EditorGUILayout.Popup(KP.MAT_CART_INDEX, kShaderLab.CATEGORY);
                GUILayout.Space(2);
                // Material family
                KP.MAT_FAM_INDEX = GUILayout.SelectionGrid(KP.MAT_FAM_INDEX, kShaderLab.FAMILY, 2, KP_Style.grid(), GUILayout.MinWidth(100));
            }
            // Material type
            KP.FOLD_type = EditorGUILayout.Foldout(KP.FOLD_type, "Shader Type ");
            if (KP.FOLD_type)
            {
                //sc1 = EditorGUILayout.BeginScrollView(sc1, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true), GUILayout.MaxHeight(250), GUILayout.MinHeight(20));
                KP.MAT_TYPE_INDEX = GUILayout.SelectionGrid(KP.MAT_TYPE_INDEX, kShaderLab.GetShaderList(KP.MAT_FAM_INDEX), 1, KP_Style.grid());
                //EditorGUILayout.EndScrollView();
            }
            // Material NAME
            KP.FOLD_name = EditorGUILayout.Foldout(KP.FOLD_name, "Material Name");
            if (KP.FOLD_name)
            {
                KP._meshName = EditorGUILayout.TextField(KP._meshName, KP_Style.tf_input_center());
            }
            // Material shader properties
            KP.FOLD_para = EditorGUILayout.Foldout(KP.FOLD_para, "Material Parameters");
            if (KP.FOLD_para)
            {
                Shader s = (KP._sMaterial != null) ? kShaderLab.GetShader(KP.MAT_CART_INDEX, KP.MAT_FAM_INDEX, KP.MAT_TYPE_INDEX) : null;
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

                                    RangeProperty(propertyName, label, v2, v3);

                                    //GUILayout.EndHorizontal();
                                    break;
                                }
                            case ShaderUtil.ShaderPropertyType.Float: // floats
                                Debug.Log(label);
                                FloatProperty(propertyName, label);
                                break;

                            case ShaderUtil.ShaderPropertyType.Color: // colors
                                {
                                    ColorProperty(propertyName, label);
                                    break;
                                }
                            case ShaderUtil.ShaderPropertyType.TexEnv: // textures
                                {
                                    ShaderUtil.ShaderPropertyTexDim desiredTexdim = ShaderUtil.GetTexDim(s, i);
                                    TextureProperty(propertyName, label, desiredTexdim);
                                    //GUILayout.Space(6);
                                    break;
                                }
                            case ShaderUtil.ShaderPropertyType.Vector: // vectors
                                {
                                    Debug.Log(label);
                                    //VectorProperty(propertyName, label);
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
            }
            if (EditorGUI.EndChangeCheck())
            {
                Debug.Log("REPAINT GUI");
                if (CART_temp != KP.MAT_CART_INDEX ||
                    FAM_temp != KP.MAT_FAM_INDEX ||
                    TYP_temp != KP.MAT_TYPE_INDEX)
                {
                    if (KP.MAT_SELE_INDEX != -1) KP.Reset_material();
                }
                if (KP.MAT_SELE_INDEX != -1)
                {
                    switch (KP.MAT_SELE_INDEX)
                    {
                        case 0: KP._sMaterial = kSelect.MATERIAL; break;
                        case 1: kSelect.MATERIAL = KP._sMaterial; break;
                        case 2: break;
                        case 3: break;
                    }
                    KP.MAT_SELE_INDEX = -1;
                }
                kPoly2Tool.instance.Repaint();
            }
            EditorGUILayout.EndVertical(); //------------------------------------------------------------> End Vertical
            //GUILayout.Space(10);
            //GUILayout.EndHorizontal();
            GUI.enabled = GUI_TEMP;
        }

        private static void RangeProperty(string propertyName, string label, float v2, float v3)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label + " |RP " + propertyName + " | " + v2 + " " + v3, GUILayout.Width(_LABEL_WIDTH));
            GUILayout.Space(3);
            switch (propertyName)
            {
                case "_Shininess":
                case "_InvFade":
                    KP._width = EditorGUILayout.Slider(KP._width, v2, v3);
                    break;
                case "_Parallax":
                case "_Cutoff":
                    KP._height = EditorGUILayout.Slider(KP._height, v2, v3);
                    break;
            }
            GUILayout.EndHorizontal();
        }
        private static void FloatProperty(string propertyName, string label)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label + " |FP " + propertyName, GUILayout.Width(_LABEL_WIDTH));
            GUILayout.Space(3);
            KP._sMaterial.SetFloat(propertyName, EditorGUILayout.FloatField(KP._sMaterial.GetFloat(propertyName)));
            GUILayout.EndHorizontal();
        }
        private static void ColorProperty(string propertyName, string label)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label + " |CP " + propertyName, GUILayout.Width(_LABEL_WIDTH));
            GUILayout.Space(3);
            switch (propertyName)
            {
                case "_Color":
                case "_TintColor":
                case "_EmisColor":
                    KP.Color_a = EditorGUILayout.ColorField(KP.Color_a);
                    KP._sMaterial.SetColor(label, KP.Color_a);
                    break;
                case "_SpecColor":

                    KP.Color_b = EditorGUILayout.ColorField(KP.Color_b);
                    KP._sMaterial.SetColor(label, KP.Color_b);
                    break;
                case "_Emission":
                case "_ReflectColor":
                    KP.Color_c = EditorGUILayout.ColorField(KP.Color_c);
                    KP._sMaterial.SetColor(label, KP.Color_c);
                    break;
            }
            GUILayout.EndHorizontal();
        }

        private static void TextureProperty(string propertyName, string label, ShaderUtil.ShaderPropertyTexDim desiredTexdim)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label + " |TP " + propertyName, GUILayout.Width(_LABEL_WIDTH));
            GUILayout.Space(3);
            switch (propertyName)
            {
                case "_MainTex":
                    KP.Mtexture_a = ((Texture2D)EditorGUILayout.ObjectField(KP.Mtexture_a, typeof(Texture2D), true));
                    KP._sMaterial.SetTexture(propertyName, KP.Mtexture_a);
                    break;
                case "_BumpMap":
                case "_DecalTex":
                case "_Detail":
                case "_Illum":
                    KP.Mtexture_b = ((Texture2D)EditorGUILayout.ObjectField(KP.Mtexture_b, typeof(Texture2D), true));
                    KP._sMaterial.SetTexture(propertyName, KP.Mtexture_b);
                    break;
                case "_ParallaxMap":
                    KP.Mtexture_c = ((Texture2D)EditorGUILayout.ObjectField(KP.Mtexture_c, typeof(Texture2D), true));
                    KP._sMaterial.SetTexture(propertyName, KP.Mtexture_c);
                    break;
            }
            GUILayout.EndHorizontal();
        }
    }//class
}//namespace
