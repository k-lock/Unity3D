using UnityEngine;
using UnityEditor;
using System;
using klock.geometry;

public class kPolyGUI
{
    //------------------------------------------------- MAIN PANEL ELEMENTS
    /*private static Texture2D[] TOOL_BAR_ICONS = new Texture2D[] {   klock.kLibary.LoadBitmap("create", 20,20),
                                                                    klock.kLibary.LoadBitmap("modify", 20,20),
                                                                    klock.kLibary.LoadBitmap("utility", 20,20),
                                                                    klock.kLibary.LoadBitmap("utility", 20,20)};*/
    private static string[] S_MAIN_TOOLBAR = new string[] { "CREATE", "EDIT", "INFO", "MAT" };
    public static void MAIN_TOOLBAR()
    {
        kPoly2Tool k2p = kPoly2Tool.instance;
        k2p.MAIN_MENU_ID = GUILayout.Toolbar(k2p.MAIN_MENU_ID, S_MAIN_TOOLBAR);

    }
    //------------------------------------------------- PANEL CREATE 
    private static bool FOLD_para = true;
    private static bool FOLD_name = true;
    private static bool FOLD_object = true;
    private static int P_OBJECT_TYPE_INDEX = 1;
    private static string[] P_OBJECT_TYPE = new string[6] { "Cube", "Plane", "Cone", "Cylinder", "Sphere", "Box" };

    private static string _meshName = "kPoly";
    private static float _width = 1;
    private static float _height = 1;
    private static float _depth = 1;
    private static int _uSegments = 1;
    private static int _vSegments = 1;
    private static int _zSegments = 1;
    private static int _pivotIndex = 0;
    private static string[] _pivotLabels = { "UpperLeft", "UpperCenter", "UpperRight", "MiddleLeft", "MiddleCenter", "MiddleRight", "LowerLeft", "LowerCenter", "LowerRight" };
    private static int _faceIndex = 0;
    private static string[] _windinLabels = { "TopLeft", "TopRight", "ButtomLeft", "ButtomRight" };
    private static int _windinIndex = 2;
    private static string[] _colliderLabels = { "none", "MeshCollider", "BoxCollider" };
    private static int _colliderIndex = 1;

    private static float openingAngle = 0f;
    private static bool outside = true;
    private static bool inside = false;

    public static void CREATE_objectSelect()
    {
        bool GUI_TEMP = GUI.enabled;

        EditorGUILayout.BeginVertical();
        // OBJECT TYPE 
        FOLD_object = EditorGUILayout.Foldout(FOLD_object, "Object Types");//, folderSkin());
        if (FOLD_object)
        {
            int objectTemp = P_OBJECT_TYPE_INDEX;
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            P_OBJECT_TYPE_INDEX = GUILayout.SelectionGrid(P_OBJECT_TYPE_INDEX, P_OBJECT_TYPE, 2);
            GUILayout.Space(10);
            /*for (int i = 0; i < 3; i++)
            {
                GUI.color = Color.white;
                GUI.color = (P_OBJECT_TYPE_INDEX == i) ? Color.grey : Color.white;
                if (GUILayout.Button(P_OBJECT_TYPE[i], EditorStyles.miniButton))
                {
                    P_OBJECT_TYPE_INDEX = (P_OBJECT_TYPE_INDEX == i) ? -1 : i;
                }
                GUI.color = Color.white;
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            for (int i = 3; i < 6; i++)
            {
                GUI.color = Color.white;
                GUI.color = (P_OBJECT_TYPE_INDEX == i) ? Color.grey : Color.white;
                if (GUILayout.Button(P_OBJECT_TYPE[i], EditorStyles.miniButton))
                {
                    P_OBJECT_TYPE_INDEX = (P_OBJECT_TYPE_INDEX == i) ? -1 : i;
                }
                GUI.color = Color.white;
            }*/
            GUILayout.EndHorizontal();
            if (objectTemp != P_OBJECT_TYPE_INDEX)
            {
                ResetEditorValues();
            }
        }
        // OBJECT NAME
        FOLD_name = EditorGUILayout.Foldout(FOLD_name, "Object Name");
        if (FOLD_name)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            GUI.enabled = P_OBJECT_TYPE_INDEX != -1;
            _meshName = EditorGUILayout.TextField(_meshName, labelCSkin());
            GUI.enabled = GUI_TEMP;
            GUILayout.Space(10);
            GUILayout.EndHorizontal();
        }
        // OBJECT PARAMETERS
        FOLD_para = EditorGUILayout.Foldout(FOLD_para, "Parameters");

        if (FOLD_para)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            GUI.enabled = P_OBJECT_TYPE_INDEX != -1;
            switch (P_OBJECT_TYPE_INDEX)
            {
                case 0:
                    CREATE_cube();
                    break;
                case 1:
                    CREATE_plane();
                    break;
                case 2:
                    CREATE_cone();
                    break;
            }
            GUILayout.Space(10);
            GUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        GUI.enabled = GUI_TEMP;
        GUI.skin = null;
    }
    static void CREATE_cone()
    {
        EditorGUILayout.BeginVertical();
        // Editor value reset button
        if (GUILayout.Button(new GUIContent("Reset Editor"), EditorStyles.miniButton))
        {
            ResetEditorValues();
        }
        // if openingAngle>0, create a cone with this angle by setting radiusTop to 0, and adjust radiusBottom according to length;
        EditorGUILayout.Space();
        _uSegments = EditorGUILayout.IntField("Segments", _uSegments); // numVertices
        EditorGUILayout.Space();
        _width = EditorGUILayout.FloatField("Radius Top", _width); //radiusTop
        _depth = EditorGUILayout.FloatField("Radius Bottom", _depth);//radiusBottom
        _height = EditorGUILayout.FloatField("Height", _height);//length
        EditorGUILayout.Space();
        openingAngle = EditorGUILayout.FloatField("Open Angle", openingAngle);
        EditorGUILayout.Space();
        outside = EditorGUILayout.Toggle("Outside", outside);
        inside = EditorGUILayout.Toggle("Inside", inside);
        EditorGUILayout.Space();
        if (GUILayout.Button(new GUIContent("Create Mesh")))
        {
            kPoly.Create_Cone_Object(_meshName, _uSegments, _width, _depth, _height, openingAngle, outside, inside);
        }
        EditorGUILayout.EndVertical();
    }
    static void CREATE_cube()
    {
        EditorGUILayout.BeginVertical();
        // Editor value reset button
        if (GUILayout.Button(new GUIContent("Reset Editor"), EditorStyles.miniButton))
        {
            ResetEditorValues();
        }
        EditorGUILayout.Space();
        // Editor value for width and height of the created mesh [ float ]
        _width = EditorGUILayout.FloatField("Width", _width);
        _height = EditorGUILayout.FloatField("Height", _height);
        _depth = EditorGUILayout.FloatField("Depth", _depth);
        EditorGUILayout.Space();
        // Editor value for width and height segments of the created mesh [ int ]
        _uSegments = EditorGUILayout.IntField("uSegments", _uSegments);
        _vSegments = EditorGUILayout.IntField("vSegments", _vSegments);
        _zSegments = EditorGUILayout.IntField("zSegments", _zSegments);
        EditorGUILayout.Space();

        if (GUILayout.Button(new GUIContent("Create Mesh")))
        {
            kPoly.Create_Cube_Object(_meshName, _uSegments, _vSegments, _zSegments, _width, _height, _depth);
        }
        EditorGUILayout.EndVertical();
    }
    static void CREATE_plane()
    {
        EditorGUILayout.BeginVertical();
        EditorGUI.BeginChangeCheck();
        GUILayout.BeginHorizontal();
        // Editor value reset button
        if (GUILayout.Button(new GUIContent("Reset Editor"), EditorStyles.miniButton))
        {
            ResetEditorValues();
        }
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();
        // Editor value for width and height of the created mesh [ float ]
        _width = EditorGUILayout.FloatField("Width", _width);
        _height = EditorGUILayout.FloatField("Height", _height);
        EditorGUILayout.Space();
        // Editor value for width and height segments of the created mesh [ int ]
        _uSegments = EditorGUILayout.IntField("uSegments", _uSegments);
        _vSegments = EditorGUILayout.IntField("vSegments", _vSegments);
        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
        // Editor value for the pivot point of the created mesh Unity.TextAnchor
        GUILayout.Label("Pivot ");
        GUILayout.Space(18);
        _pivotIndex = EditorGUILayout.Popup(_pivotIndex, _pivotLabels);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        // Editor value for the mesh face direction FACING.XZ
        GUILayout.Label("Facing ");
        GUILayout.Space(10);
        _faceIndex = EditorGUILayout.Popup(_faceIndex, klock.geometry.kPoly.FACING);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        // Editor value for triangle winding order
        GUILayout.Label("Winding");
        GUILayout.Space(2);
        _windinIndex = EditorGUILayout.Popup(_windinIndex, _windinLabels);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        // Editor value for collider export
        GUILayout.Label("Collider ");
        GUILayout.Space(3);
        _colliderIndex = EditorGUILayout.Popup(_colliderIndex, _colliderLabels);
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        // Starting GUI changes check
        if (EditorGUI.EndChangeCheck())
        {
            _width = Mathf.Clamp(_width, 0, int.MaxValue);
            _height = Mathf.Clamp(_height, 0, int.MaxValue);
            _uSegments = Mathf.Clamp(_uSegments, 1, int.MaxValue);
            _vSegments = Mathf.Clamp(_vSegments, 1, int.MaxValue);
            Debug.Log("Change Editor");
        }
        // Editor Button for start mesh creation
        if (GUILayout.Button(new GUIContent("Create Mesh")))
        {
            kPoly.Create_Plane_Object(_meshName, _uSegments, _vSegments, _width, _height, _faceIndex, _windinIndex, _pivotIndex, _colliderIndex);
        }
        EditorGUILayout.EndVertical();
    }
    /*public static GUIStyle folderSkin()
    {
        GUIStyle cs = EditorStyles.foldout;
        //cs.normal.background = GUI.skin.label.onActive.background;
        // cs.fontStyle = FontStyle.Bold;
        // cs.fontSize = 11;
        return cs;
    }*/
    public static GUIStyle labelCSkin()
    {
        GUIStyle cs = new GUIStyle(EditorStyles.textField);
        cs.alignment = TextAnchor.MiddleCenter;

        return cs;
    }
    /** Reset the editor values to default.*/
    private static void ResetEditorValues()
    {
        _width = 1;
        _height = 1;
        _depth = 1;

        _uSegments = 1;
        _vSegments = 1;
        _zSegments = 1;

        _pivotIndex = 0;
        _faceIndex = 0;
        _colliderIndex = 1;
        _windinIndex = 2;

        openingAngle = 0f;
        outside = true;
        inside = false;

        if (P_OBJECT_TYPE_INDEX == 2)
        {
            _uSegments = 10;
            _width = 0;
        }
    }
    #region EXTRA GUI
    public static Enum EnumToolbar(Enum selected)
    {
        string[] toolbar = System.Enum.GetNames(selected.GetType());
        Array values = System.Enum.GetValues(selected.GetType());

        for (int i = 0; i < toolbar.Length; i++)
        {
            string toolname = toolbar[i];
            toolname = toolname.Replace("_", " ");
            toolbar[i] = toolname;
        }

        int selected_index = 0;
        while (selected_index < values.Length)
        {
            if (selected.ToString() == values.GetValue(selected_index).ToString())
            {
                break;
            }
            selected_index++;
        }
        selected_index = GUILayout.Toolbar(selected_index, toolbar);
        return (Enum)values.GetValue(selected_index);
    }
    #endregion
}

