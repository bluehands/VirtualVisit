using UnityEngine;
using System;

[Serializable]
public class VisitNodeSetting {

    public string id;

    public string title;

    public Vector3 position;

    public string edgeIds;

    public VisitEdgeSetting[] edgeSettings;

    public VisitMarkSetting[] markSettings;

    public string[] getEdgeIds()
    {
        return edgeIds.Equals("") ? new string[0] : edgeIds.Split(',');
    }

}
