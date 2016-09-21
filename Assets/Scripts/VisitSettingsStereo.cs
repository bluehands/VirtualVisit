using UnityEngine;
using System;

[Serializable]
public class VisitSettingsStereo {

    public string id;

    public string author;

    public string created;

    public int previewNodeIndex;

    public VisitNodeSettingsStereo[] nodeSettings;

    public VisitNodeSettingsStereo getPreviewNodeSetting()
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
