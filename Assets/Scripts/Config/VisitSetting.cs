using UnityEngine;
using System;

[Serializable]
public class VisitSetting {

    public string id;

    public string title;

    public string author;

    public string created;

    public string decription;

    public int previewNodeIndex;

    public VisitNodeSetting[] nodeSettings;

    public VisitNodeSetting getPreviewNodeSetting()
    {
        if(nodeSettings.Length == 0)
        {
            Debug.Log("No VisitNodeSettings loaded!");
            return null;
        }
        if(previewNodeIndex >= nodeSettings.Length)
        {
            Debug.Log("VisitNodeSetting Index out of bounce, use default!");
            return nodeSettings[0];
        }
        return nodeSettings[previewNodeIndex];
    }
    
}
