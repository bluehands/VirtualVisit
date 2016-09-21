using UnityEngine;
using System.Collections.Generic;
using System;

public class VisitStereo : MonoBehaviour {

    public VisitNodeStereo nodePrefab;

    public VisitEdgeStereo edgePrefab;

    public MapStereo mapPrefab;

    public ButtonNextStereo nextButtonPrefab;

    private VisitSettingsStereo visitSettings;

    private List<VisitNodeStereo> visitNodes = new List<VisitNodeStereo>();

    public MapStereo map;

    private VisitNodeStereo currentVisitNode;

    public void Generate(string visitId, VisitSettingsFactory visitSettingsFactory)
    {
        visitSettings = visitSettingsFactory.getVisitSetting(visitId);

        foreach (var visitNodeSetting in visitSettings.nodeSettings)
        {
            createNode(visitSettings, visitNodeSetting, visitSettingsFactory);
        }

        foreach(var visitNodeSetting in visitSettings.nodeSettings)
        {
            foreach(var edgeId in visitNodeSetting.getEdgeIds())
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
        currentVisitNode = visitNodes[0];
    }

    public VisitNodeStereo getCurrentVisitNode()
    {
        return currentVisitNode;
    }

    private void createNode(VisitSettingsStereo visitSettings, VisitNodeSettingsStereo visitNodeSetting, VisitSettingsFactory visitSettingsFactory)
    {
        VisitNodeStereo node = Instantiate(nodePrefab) as VisitNodeStereo;

        Texture textureLeft = null;
        Texture textureRight = null;

        bool isStereo = visitSettingsFactory.tryToLoadTextures(visitSettings, visitNodeSetting, out textureLeft, out textureRight);

        if (isStereo)
        {
            node.Initialize(visitNodeSetting.id, visitNodeSetting.title, visitNodeSetting.postion, transform, textureLeft, textureRight);
        }
        else
        {
            node.Initialize(visitNodeSetting.id, visitNodeSetting.title, visitNodeSetting.postion, transform, textureLeft);
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
        currentVisitNode = toNode;
    }

}
