using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SceneViewObjectWindow : EditorWindow
{
    static GameObject m_LastObject;
    static List<GameObject> m_Stack = new List<GameObject>();

    static bool m_UseStack = false;
    static float m_MaxStackSize = 5;

    [MenuItem("Tools/Open SceneView Object Selector")]
    public static void OpenWindow()
    {
        EditorWindow.GetWindow<SceneViewObjectWindow>();
    }

    [MenuItem("Tools/Select Scene Object &s")]
    public static void SelectObject()
    {
        if (m_LastObject != null)
        {
            if (!m_UseStack)
                m_Stack.Clear();
            m_Stack.Add(m_LastObject);
        }
        EditorWindow.GetWindow<SceneViewObjectWindow>().Repaint();
    }

    void OnGUI()
    {
        Event e = Event.current;
        m_UseStack = GUILayout.Toggle(m_UseStack, "Use Stack");
        if (m_UseStack)
            m_MaxStackSize = GUILayout.HorizontalSlider(m_MaxStackSize, 1, 20);

        for (int i = m_Stack.Count - 1; i >= 0; i--)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.ObjectField(m_Stack[i], typeof(GameObject));
            if (GUILayoutUtility.GetLastRect().Contains(e.mousePosition) && e.type == EventType.MouseDrag)
            {
                DragAndDrop.PrepareStartDrag();
                DragAndDrop.objectReferences = new UnityEngine.Object[] { m_Stack[i] };
                DragAndDrop.StartDrag("drag");
                Event.current.Use();
            }
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                m_Stack.RemoveAt(i);
                Repaint();
            }
            GUILayout.EndHorizontal();
            if (e.type == EventType.Repaint && m_Stack[i] == null)
            {
                m_Stack.RemoveAt(i);
                Repaint();
            }
        }
        if (m_UseStack && e.type == EventType.Repaint)
        {
            while (m_Stack.Count > m_MaxStackSize)
                m_Stack.RemoveAt(0);
        }
    }

    [DrawGizmo(GizmoType.NotSelected)]
    static void RenderCustomGizmo(Transform objectTransform, GizmoType gizmoType)
    {
        if (Event.current == null)
            return;
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            m_LastObject = hit.transform.gameObject;
        }
    }
}