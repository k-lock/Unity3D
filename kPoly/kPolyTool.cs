/** http://www.k-lock.de  | Paul Knab 
 * 	_______________________________________
 * 	
 * 	kPolyTool | V.1.0.0 | 11.04.2013
 *  ________________________________________
 * 
 * 	Editor Window kPoly Utility
 * 
 * */
using UnityEngine;
using UnityEditor;

public class kPolyTool : EditorWindow
{
    #region vars
    /** Static instance to this editor class. */
    public static kPolyTool instance;
    private static PANEL _PANEL = PANEL.CREATE;
    private SceneView.OnSceneFunc _onSceneGUI_ = null;
    private static kPolyCreate pCreate = null;
    private static kPolyEdit pEdit = null;
    private static kPolyInfo pInfo = null;

    #endregion
    #region Editor
    /** The Unity EditorWindow start function.*/
    [MenuItem("Window/klock/kMesh/kPolyTools %n")]
    public static kPolyTool Init()
    {
        instance = (kPolyTool)EditorWindow.GetWindow(typeof(kPolyTool), false, "ToolsOLD");
        instance.Show();
        instance.OnEnable();
        instance.position = new Rect(200, 100, 250, 400);
        instance.minSize = new Vector2(100, 200);
        instance.maxSize = new Vector2(500, 600);

        return instance;
    }
    #endregion
    #region Unity
    private void OnDestroy()
    {

        pCreate = null;
        pEdit = null;
        pInfo = null;
        _onSceneGUI_ = null;
        instance = null;

    }
    private void OnEnable()
    {
        hideFlags = HideFlags.HideAndDontSave;
        if (instance == null)
        {
            instance = this;
        }
        InstanciesInit();
        Repaint();
    }

    private void OnDisable()
    {
        instance = null;
        pCreate = null;
        pEdit = null;
        pInfo = null;
    }

    private void OnGUI()
    {
        DrawPanel();
       
    }

    private void OnSelectionChange()
    {
        Debug.Log("TOOL - OnSelectionChange");
        if(pEdit != null )if (pEdit._freeze)return;
        switch (_PANEL)
        {
            case PANEL.EDIT:
                //CLEAR_EDITOR("Edit");

               
                pEdit.OnSelectionChange();
                pEdit.Repaint();
                Repaint();
                
                break;
            case PANEL.INFO:
                // CLEAR_EDITOR("Info");
                pInfo.OnSelectionChange();
                pInfo.Repaint();
                Repaint();
                break;
        }
        
    
        //Repaint();
    }
    private void Update()
    {
        if (pEdit != null ) pEdit.Updater(); 
    }

    public void OnSceneGUI(SceneView sceneView)
    {

    }

    #endregion
    #region Editor GUI
    /** Main GUI draw function.*/
    private void DrawPanel()
    {
        int controlID = GUIUtility.GetControlID(instance.GetHashCode(), FocusType.Passive);
        Event e = Event.current;
        switch (e.GetTypeForControl(controlID))
        {
            case EventType.mouseDown:
            case EventType.mouseUp:
            case EventType.mouseDrag:
            case EventType.keyUp:
            case EventType.keyDown:
            case EventType.repaint:
            case EventType.layout:
            case EventType.ExecuteCommand:
            case EventType.ValidateCommand:

                DrawMenu();
                DrawActPanel();

                break;
        }
    }

    private void InstanciesInit()
    {
        Debug.Log("InstanciesInit");

        if (_PANEL == PANEL.CREATE  && pCreate == null)
        {
           
            pCreate = kPolyCreate.Create();
        }
        if (_PANEL == PANEL.EDIT && pEdit == null)
        {
            pEdit = kPolyEdit.Create();
        }
        if (_PANEL == PANEL.INFO && pInfo == null)
        {
            pInfo = kPolyInfo.Create();
            pInfo.OnSelectionChange();
        }
        if (_PANEL != PANEL.CREATE || _PANEL != PANEL.PREFS)
        {
            /* if (_onSceneGUI_ == null)
             {
                 _onSceneGUI_ = new SceneView.OnSceneFunc(OnSceneGUI);
                 SceneView.onSceneGUIDelegate += _onSceneGUI_;
             }*/
        }
    }

    private void DrawActPanel()
    {
        switch (_PANEL)
        {
            case PANEL.CREATE:
                

                if (pCreate == null) InstanciesInit();
               //CLEAR_ALL_OF("Create");
                pCreate.DrawPanel();
                break;
            case PANEL.EDIT:
                
                if (pEdit == null) InstanciesInit();
               // CLEAR_ALL_OF("Edit");
                pEdit.DrawPanel();
                pEdit.OnEnable();
                break;
            case PANEL.INFO:
                
                if (pInfo == null) InstanciesInit();
               // CLEAR_ALL_OF("Info");
                pInfo.DrawPanel();
                break;
            case PANEL.PREFS:
                break;
        }
    }

    private void DrawMenu()
    {
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.BeginVertical();//new GUIStyle { contentOffset = new Vector2 (0, 0) });
        GUILayout.BeginHorizontal();

        GUI.color = (_PANEL == PANEL.CREATE) ? Color.grey : Color.white;
        if (GUILayout.Button(new GUIContent("Create")))
        {
            _PANEL = PANEL.CREATE;
        }
        GUI.color = Color.white;
        GUI.color = (_PANEL == PANEL.EDIT) ? Color.grey : Color.white;
        if (GUILayout.Button(new GUIContent("Edit")))
        {
            _PANEL = PANEL.EDIT;
        }
        GUI.color = Color.white;
        GUI.color = (_PANEL == PANEL.INFO) ? Color.grey : Color.white;
        if (GUILayout.Button(new GUIContent("Info")))
        {
            _PANEL = PANEL.INFO;
        }
        GUI.color = Color.white;
        GUI.color = (_PANEL == PANEL.PREFS) ? Color.grey : Color.white;
        if (GUILayout.Button(new GUIContent("Prefs")))
        {
            _PANEL = PANEL.PREFS;
        }
        GUI.color = Color.white;
        GUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
        if (EditorGUI.EndChangeCheck())
        {
            Repaint();
        }
    }

    #endregion
    public static void CLEAR_ALL_OF(string name)
    {
        EditorWindow[] pc = FIND_ALL_OF(name) as EditorWindow[];
        int n = pc.Length;
        if (n > 0)
        {
            //Debug.Log("---> Try to Close " + n + " "+name+" Editor Windows");
            for (int i = 0; i < n; i++)
            {
                //Debug.Log("---> Close " + i);
                if (pc[i] is ScriptableObject)
                {
                    DestroyImmediate(pc[i]);
                    i = n;
                }
                else
                    pc[i].Close();
            }
        }
    }
    public static EditorWindow[] FIND_ALL_OF(string name)
    {
        if (name == "Tool")     return (kPolyTool[])(Resources.FindObjectsOfTypeAll(typeof(kPolyTool)));
        if (name == "Create")   return (kPolyCreate[])(Resources.FindObjectsOfTypeAll(typeof(kPolyCreate)));
        if (name == "Edit")     return (kPolyEdit[])(Resources.FindObjectsOfTypeAll(typeof(kPolyEdit)));
        if (name == "Info")     return (kPolyInfo[])(Resources.FindObjectsOfTypeAll(typeof(kPolyInfo)));
        if (name == "Tools")    return (kPolyTool[])(Resources.FindObjectsOfTypeAll(typeof(kPolyTool)));

        return null;
    }

    /*public static void CLEAR_EDITOR(string name)
    {
        EditorWindow[] windows = (EditorWindow[])(Resources.FindObjectsOfTypeAll(typeof(EditorWindow)));
        for (int i = 0; i < windows.Length; i++)
        {
            if (windows[i].title == name)
            {
                if (name == "Edit")
                {
                    kPolyEdit f = windows[i] as kPolyEdit;
                    //f.Close();
                    ScriptableObject.DestroyImmediate(f);
                }
                if (name == "Create")
                {
                    kPolyCreate t = windows[i] as kPolyCreate;
                    ScriptableObject.DestroyImmediate(t);
                }
                if (name == "Info")
                {
                    kPolyInfo d = windows[i] as kPolyInfo;
                    ScriptableObject.DestroyImmediate(d);
                }
            }
        }
    }*/
}

public enum PANEL
{
    CREATE,
    EDIT,
    INFO,
    PREFS
}