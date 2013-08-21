using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNet : MonoBehaviour
{

    List<PathSingle> _pathlist;
    public List<PathSingle> PathList { get { return _pathlist; } }

    void Awake()
    {
        _pathlist = new List<PathSingle>();

        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            PathSingle ps = new PathSingle();

            ps.start = transform.GetChild(i).FindChild("TargetA").position;
            ps.end = transform.GetChild(i).FindChild("TargetB").position;
            ps.pathIndex = i;

            _pathlist.Add(ps);
        }

    }


}

