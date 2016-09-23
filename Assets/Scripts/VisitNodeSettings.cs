using UnityEngine;
using System;

[Serializable]
public class VisitNodeSettings {

    public string id;

    public string title;

    public Vector3 postion;

    public string edgeIds;

    public string[] getEdgeIds()
    {
        return edgeIds.Split(',');
    }

}
