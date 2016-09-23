﻿using UnityEngine;
using System.Collections.Generic;
using System;

public class Visit : MonoBehaviour {

    public VisitNode nodePrefab;

    public VisitEdge edgePrefab;

    public Map mapPrefab;

    public ButtonStep buttonStepPrefab;

    private VisitSettings m_VisitSettings;

    private List<VisitNode> m_VisitNodes;

    public Map Map { get; private set; }

    public VisitNode CurrentVisitNode { get; private set; }

    public void Generate(string visitId, VisitSettingsFactory visitSettingsFactory)
    {
        m_VisitNodes = new List<VisitNode>();

        m_VisitSettings = visitSettingsFactory.GetVisitSetting(visitId);

        foreach (var visitNodeSetting in m_VisitSettings.nodeSettings)
        {
            createNode(m_VisitSettings, visitNodeSetting, visitSettingsFactory);
        }

        foreach(var visitNodeSetting in m_VisitSettings.nodeSettings)
        {
            foreach(var edgeId in visitNodeSetting.getEdgeIds())
            {
                createEdge(visitNodeSetting.id, edgeId);
            }
        }

        Map = Instantiate(mapPrefab) as Map;
        Map.Generate(m_VisitNodes);
        Map.Initialize();
    }

    public void Initialize()
    {
        foreach(var visitNode in m_VisitNodes)
        {
            visitNode.Unselect();
        }
        m_VisitNodes[0].Select();
        CurrentVisitNode = m_VisitNodes[0];
    }

    private void createNode(VisitSettings visitSettings, VisitNodeSettings visitNodeSetting, VisitSettingsFactory visitSettingsFactory)
    {
        VisitNode node = Instantiate(nodePrefab) as VisitNode;

        Texture textureLeft = null;
        Texture textureRight = null;

        bool isStereo = visitSettingsFactory.TryToLoadTextures(visitSettings, visitNodeSetting, out textureLeft, out textureRight);

        if (isStereo)
        {
            node.Initialize(visitNodeSetting.id, visitNodeSetting.title, visitNodeSetting.postion, transform, textureLeft, textureRight);
        }
        else
        {
            node.Initialize(visitNodeSetting.id, visitNodeSetting.title, visitNodeSetting.postion, transform, textureLeft);
        }
        m_VisitNodes.Add(node);
    }

    internal void SetVisitEdgeVisibility(bool visibility)
    {
        foreach(var node in m_VisitNodes)
        {
            if(node.gameObject.activeInHierarchy)
            {
                foreach (var edge in node.GetEdges())
                {
                    edge.gameObject.SetActive(visibility);
                }
            }
        }
    }

    private void createEdge(string fromId, string toId)
    {
        VisitEdge edge = Instantiate(edgePrefab) as VisitEdge;
        VisitNode fromNode = getNode(fromId);
        VisitNode toNode = getNode(toId);

        ButtonStep buttonStep = Instantiate(buttonStepPrefab) as ButtonStep;

        edge.Initialize(fromNode, toNode, buttonStep);
    }

    private VisitNode getNode(string id)
    {
        for (int i = 0; i < m_VisitNodes.Count; i++)
        {
            if(m_VisitNodes[i].Id.Equals(id))
            {
                return m_VisitNodes[i];
            }
        }
        throw new InvalidOperationException(String.Format("Tour couldn't find node for {0} in visit {1}." , id, m_VisitSettings.id));
    }

    public void MoveTo(VisitEdge edge)
    {
        VisitNode fromNode = edge.FromNode;
        VisitNode toNode = edge.ToNode;

        Debug.Log(String.Format("MoveTo({0},{1})", fromNode.Id, toNode.Id));

        fromNode.Unselect();
        toNode.Select();
        CurrentVisitNode = toNode;
    }

}
