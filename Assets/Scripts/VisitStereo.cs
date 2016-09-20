using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class VisitStereo : MonoBehaviour {

    public VisitNodeStereo nodePrefab;

    public VisitEdgeStereo edgePrefab;

    public MapStereo mapPrefab;

    public ButtonNextStereo nextButtonPrefab;

    private VisitSettingsStereo visitSettings;

    private List<VisitNodeStereo> visitNodes = new List<VisitNodeStereo>();

    public MapStereo map;

    public void Generate(string visitId)
    {
        visitSettings = getVisitSettings(visitId); ;

        foreach (var visitNodeSetting in visitSettings.nodeSettings)
        {
            createNode(visitSettings.id, visitNodeSetting.id, visitNodeSetting.title, visitNodeSetting.postion);
        }

        foreach(var visitNodeSetting in visitSettings.nodeSettings)
        {
            foreach(var edgeId in getEdgeIds(visitNodeSetting.edgeIds))
            {
                createEdge(visitNodeSetting.id, edgeId);
            }
        }

        map = Instantiate(mapPrefab) as MapStereo;
        map.Generate(visitNodes);
        map.Initialize();
    }

    public void Initialize()
    {
        foreach(var visitNode in visitNodes)
        {
            visitNode.unselect();
        }
        visitNodes[0].select();
    }

    private VisitSettingsStereo getVisitSettings(string visitId)
    {
        if (visitId == null || visitId.Equals(""))
        {
            visitId = "Epple";
        }
        VisitSettingsStereo visitSettings = null;

        var jsonText = Resources.Load(String.Format("Visits\\{0}", visitId)) as TextAsset;

        visitSettings = JsonUtility.FromJson<VisitSettingsStereo>(jsonText.text);

        return visitSettings;
    }

    private bool tryToLoadTextures(string visitId, string nodeId, out Texture textureLeft, out Texture textureRight)
    {
        bool isStereo;
        textureLeft = Resources.Load(String.Format("Panoramas\\{0}_{1}_{2}", visitId, nodeId, "l")) as Texture;
        textureRight = Resources.Load(String.Format("Panoramas\\{0}_{1}_{2}", visitId, nodeId, "r")) as Texture;
        if (textureLeft != null && textureRight != null)
        {
            isStereo = true;
        } else
        {
            textureLeft = Resources.Load(String.Format("Panoramas\\{0}_{1}", visitId, nodeId)) as Texture;
            isStereo = false;
        }
        return isStereo;
    }

    private string[] getEdgeIds(string edgeIdsStr)
    {
        return edgeIdsStr.Split(',');
    }

    private void createNode(string visitId, string nodeId, string title, Vector3 position)
    {
        VisitNodeStereo node = Instantiate(nodePrefab) as VisitNodeStereo;

        Texture textureLeft = null;
        Texture textureRight = null;

        bool isStereo = tryToLoadTextures(visitId, nodeId, out textureLeft, out textureRight);

        if (isStereo)
        {
            node.Initialize(nodeId, title, position, transform, textureLeft, textureRight);
        } else
        {
            node.Initialize(nodeId, title, position, transform, textureLeft);
        }
        visitNodes.Add(node);
    }

    private void createEdge(string fromId, string toId)
    {
        VisitEdgeStereo edge = Instantiate(edgePrefab) as VisitEdgeStereo;
        VisitNodeStereo fromNode = getNode(fromId);
        VisitNodeStereo toNode = getNode(toId);

        ButtonNextStereo nextButton = Instantiate(nextButtonPrefab) as ButtonNextStereo;

        edge.Initialize(fromNode, toNode, nextButton);
    }

    private VisitNodeStereo getNode(string id)
    {
        for (int i = 0; i < visitNodes.Count; i++)
        {
            if(visitNodes[i].id.Equals(id))
            {
                return visitNodes[i];
            }
        }
        throw new InvalidOperationException(String.Format("Tour couldn't find node for {0} in visit {1}." , id, visitSettings.id));
    }

    public void moveTo(VisitEdgeStereo edge)
    {
        VisitNodeStereo fromNode = edge.fromNode;
        VisitNodeStereo toNode = edge.toNode;

        Debug.Log(String.Format("MoveTo({0},{1})", fromNode.id, toNode.id));

        fromNode.unselect();
        toNode.select();
    }

}
