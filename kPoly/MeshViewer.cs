using UnityEditor;
using UnityEngine;
using System.Collections;

public class MeshViewer : EditorWindow
{
    const int MAX_DISPLAY = 100;
    Mesh mesh;
    GameObject obj;
    bool linkScroll, showVerts, showNorm, showTangents, showIndices, showUVs, showUV2s, showBoneWeights, showBindPoses, showBones;
    Vector2 vertScroll, normScroll, tangentScroll, triScroll, UVScroll, UV2Scroll, boneWeightScroll, bindPoseScroll, boneScroll;

    Vector3[] vertices;
    Vector3[] normals;
    Vector4[] tangents;
    Vector2[] uv, uv2;
    BoneWeight[] boneWeights;
    Matrix4x4[] bindPoses;
    Transform[] bones;
    int[] triangles;

    SkinnedMeshRenderer SMR;

    [MenuItem("Window/MeshViewer")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        EditorWindow.GetWindow(typeof(MeshViewer));
    }
    GameObject oldObject;

    MeshTopology topo;

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        {
            obj = (GameObject)EditorGUILayout.ObjectField(obj, typeof(GameObject), true);
            if (obj == null)
            {
                mesh = null;
                SMR = null;
            }
            else if (obj.GetComponent<MeshFilter>() && (mesh == null || obj != oldObject))
            {
                SMR = null;
                mesh = obj.GetComponent<MeshFilter>().sharedMesh;
                topo = mesh.GetTopology(0);
            }
            else if (obj.GetComponent<SkinnedMeshRenderer>() && (mesh == null || obj != oldObject))
            {
                SMR = obj.GetComponent<SkinnedMeshRenderer>();
                mesh = SMR.sharedMesh;
                topo = mesh.GetTopology(0);
            }
            oldObject = obj;
            linkScroll = GUILayout.Toggle(linkScroll, "Link scroll bars");
            GUILayout.Label("Mesh Topology: ", GUILayout.Width(92));
            topo = (MeshTopology)EditorGUILayout.EnumPopup(topo);
        } GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        {
            showVerts = GUILayout.Toggle(showVerts, "Show Vertices");
            showNorm = GUILayout.Toggle(showNorm, "Show Normals");
            showTangents = GUILayout.Toggle(showTangents, "Show Tangents");
            showIndices = GUILayout.Toggle(showIndices, "Show Indices");
            showUVs = GUILayout.Toggle(showUVs, "Show UVs");
            showUV2s = GUILayout.Toggle(showUV2s, "Show UV2s");
            showBoneWeights = GUILayout.Toggle(showBoneWeights, "Show BoneWeights");
            showBindPoses = GUILayout.Toggle(showBindPoses, "Show BindPoses");
            showBones = GUILayout.Toggle(showBones, "Show Bones");
        } GUILayout.EndHorizontal();
        if (mesh)
        {
            GUILayout.BeginHorizontal();
            {
                if (showVerts)
                {
                    BeginScroll(ref vertScroll);
                    vertices = mesh.vertices;
                    for (int i = 0; i < mesh.vertexCount && i < MAX_DISPLAY; i++)
                    {
                        vertices[i] = EditorGUILayout.Vector3Field(i + ":", vertices[i]);
                    }
                    mesh.vertices = vertices;
                    GUILayout.EndScrollView();

                }
                if (showNorm)
                {
                    BeginScroll(ref normScroll);
                    normals = mesh.normals;
                    for (int i = 0; i < mesh.normals.Length && i < MAX_DISPLAY; i++)
                    {
                        normals[i] = EditorGUILayout.Vector3Field(i + ":", normals[i]);
                    }
                    mesh.normals = normals;
                    GUILayout.EndScrollView();
                }
                if (showTangents)
                {
                    BeginScroll(ref tangentScroll);
                    tangents = mesh.tangents;
                    for (int i = 0; i < mesh.tangents.Length && i < MAX_DISPLAY; i++)
                    {
                        tangents[i] = EditorGUILayout.Vector3Field(i + ":", tangents[i]);
                    }
                    mesh.tangents = tangents;
                    GUILayout.EndScrollView();
                }
                if (showBoneWeights)
                {
                    BeginScroll(ref boneWeightScroll);
                    boneWeights = mesh.boneWeights;
                    for (int i = 0; i < mesh.boneWeights.Length && i < MAX_DISPLAY; i++)
                    {
                        GUILayout.Label(i + ":");
                        boneWeights[i].boneIndex0 = EditorGUILayout.IntField("Bone 0 idx: ", boneWeights[i].boneIndex0);
                        boneWeights[i].boneIndex1 = EditorGUILayout.IntField("Bone 1 idx: ", boneWeights[i].boneIndex1);
                        boneWeights[i].boneIndex2 = EditorGUILayout.IntField("Bone 2 idx: ", boneWeights[i].boneIndex2);
                        boneWeights[i].boneIndex3 = EditorGUILayout.IntField("Bone 3 idx: ", boneWeights[i].boneIndex3);
                        boneWeights[i].weight0 = EditorGUILayout.FloatField("Bone 0 Weight: ", boneWeights[i].weight0);
                        boneWeights[i].weight1 = EditorGUILayout.FloatField("Bone 1 Weight: ", boneWeights[i].weight1);
                        boneWeights[i].weight2 = EditorGUILayout.FloatField("Bone 2 Weight: ", boneWeights[i].weight2);
                        boneWeights[i].weight3 = EditorGUILayout.FloatField("Bone 3 Weight: ", boneWeights[i].weight3);
                    }
                    //mesh.boneWeights = boneWeights;
                    GUILayout.EndScrollView();
                }
                if (showIndices)
                {
                    BeginScroll(ref triScroll);
                    triangles = mesh.GetIndices(0);
                    int jump = 1;
                    switch (topo)
                    {
                        case MeshTopology.Quads:
                            jump = 4;
                            break;
                        case MeshTopology.Triangles:
                            jump = 3;
                            break;
                        case MeshTopology.Lines:
                            jump = 2;
                            break;
                    }
                    for (int i = 0; i < triangles.Length && i < MAX_DISPLAY; i += jump)
                    {
                        GUILayout.BeginHorizontal();
                        {
                            triangles[i] = EditorGUILayout.IntField(i + ":", triangles[i]);
                            if (topo == MeshTopology.Lines || topo == MeshTopology.Triangles || topo == MeshTopology.Quads)
                                triangles[i + 1] = EditorGUILayout.IntField(i + 1 + ":", triangles[i + 1]);
                            if (topo == MeshTopology.Triangles || topo == MeshTopology.Quads)
                                triangles[i + 2] = EditorGUILayout.IntField(i + 2 + ":", triangles[i + 2]);
                            if (topo == MeshTopology.Quads)
                                triangles[i + 3] = EditorGUILayout.IntField(i + 3 + ":", triangles[i + 3]);
                        } GUILayout.EndHorizontal();

                    }
                    mesh.SetIndices(triangles, topo, 0);
                    GUILayout.EndScrollView();
                }
                if (showUVs)
                {
                    BeginScroll(ref UVScroll);
                    uv = mesh.uv;
                    for (int i = 0; i < mesh.uv.Length && i < MAX_DISPLAY; i++)
                    {
                        GUILayout.BeginHorizontal();
                        {
                            uv[i] = EditorGUILayout.Vector2Field(i + ":", uv[i]);
                        } GUILayout.EndHorizontal();
                    }
                    mesh.uv = uv;
                    GUILayout.EndScrollView();
                }
                if (showUV2s)
                {
                    BeginScroll(ref UV2Scroll);
                    uv2 = mesh.uv2;
                    for (int i = 0; i < mesh.uv2.Length && i < MAX_DISPLAY; i++)
                    {
                        GUILayout.BeginHorizontal();
                        {
                            uv2[i] = EditorGUILayout.Vector2Field(i + ":", uv2[i]);
                        } GUILayout.EndHorizontal();
                    }
                    mesh.uv2 = uv2;
                    GUILayout.EndScrollView();
                }
                if (showBindPoses)
                {
                    BeginScroll(ref bindPoseScroll);
                    bindPoses = mesh.bindposes;
                    for (int i = 0; i < bindPoses.Length && i < MAX_DISPLAY; i++)
                    {
                        GUILayout.BeginHorizontal();
                        {
                            Matrix4x4Field(i + ":", ref bindPoses[i]);
                        } GUILayout.EndHorizontal();
                    }
                    mesh.bindposes = bindPoses;
                    GUILayout.EndScrollView();
                }
                if (SMR && showBones)
                {
                    BeginScroll(ref boneWeightScroll);
                    bones = SMR.bones;
                    for (int i = 0; i < bones.Length && i < MAX_DISPLAY; i++)
                    {
                        GUILayout.BeginHorizontal();
                        {
                            bones[i] = EditorGUILayout.ObjectField(bones[i], typeof(Transform)) as Transform;
                        } GUILayout.EndHorizontal();
                    }
                    SMR.bones = bones;
                    GUILayout.EndScrollView();
                }
            } GUILayout.EndHorizontal();
        }
    }
    void BeginScroll(ref Vector2 input)
    {
        if (linkScroll)
            vertScroll = GUILayout.BeginScrollView(vertScroll);
        else
            input = GUILayout.BeginScrollView(input);
    }
    static void Matrix4x4Field(string label, ref Matrix4x4 mat)
    {
        GUILayout.Label(label);
        mat.SetRow(0, EditorGUILayout.Vector4Field("0", mat.GetRow(0)));
        mat.SetRow(1, EditorGUILayout.Vector4Field("1", mat.GetRow(1)));
        mat.SetRow(2, EditorGUILayout.Vector4Field("2", mat.GetRow(2)));
        mat.SetRow(3, EditorGUILayout.Vector4Field("3", mat.GetRow(3)));
    }
}