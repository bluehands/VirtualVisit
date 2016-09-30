﻿using UnityEngine;
using System.Collections.Generic;
using System;

public class Visit : MonoBehaviour
{

    public VisitNode nodePrefab;

    public VisitEdge edgePrefab;

    public Map mapPrefab;

    private VisitSettings m_VisitSettings;

    private List<VisitNode> m_VisitNodes;

    public Map Map { get; private set; }

    public VisitNode CurrentVisitNode { get; private set; }

    private VisitNodeChangeListener[] m_VisitNodeChangeListeners;

    public void Generate(string visitId, VisitSettingsFactory visitSettingsFactory)
    {
        m_VisitNodes = new List<VisitNode>();

        m_VisitSettings = visitSettingsFactory.GetVisitSetting(visitId);

        foreach (var visitNodeSetting in m_VisitSettings.nodeSettings)
        {
            createNode(m_VisitSettings, visitNodeSetting, visitSettingsFactory);
        }

        foreach (var visitNodeSetting in m_VisitSettings.nodeSettings)
        {
            foreach (var edgeId in visitNodeSetting.getEdgeIds())
            {
                createEdge(visitNodeSetting.id, edgeId);
            }
        }
        CurrentVisitNode = m_VisitNodes[0];

        Map = Instantiate(mapPrefab) as Map;
        Map.Generate(m_VisitNodes);
        Map.Initialize();
    }

    public void Initialize(VisitNodeChangeListener visitNodeChangeListener)
    {
        m_VisitNodeChangeListeners = new VisitNodeChangeListener[2];
        m_VisitNodeChangeListeners[0] = Map;
        m_VisitNodeChangeListeners[1] = visitNodeChangeListener;

        foreach (var visitNode in m_VisitNodes)
        {
            visitNode.Leave();
        }
        m_VisitNodes[0].GoThere();
        informListener(null, CurrentVisitNode);
    }

    private void createNode(VisitSettings visitSettings, VisitNodeSettings visitNodeSetting, VisitSettingsFactory visitSettingsFactory)
    {
        VisitNode node = Instantiate(nodePrefab) as VisitNode;

        Texture textureLeft = null;
        Texture textureRight = null;

        bool isStereo = visitSettingsFactory.TryToLoadTextures(visitSettings, visitNodeSetting, out textureLeft, out textureRight);

        if (isStereo)
        {
            node.Initialize(visitNodeSetting.id, visitNodeSetting.title, visitNodeSetting.position, transform, textureLeft, textureRight);
        }
        else
        {
            node.Initialize(visitNodeSetting.id, visitNodeSetting.title, visitNodeSetting.position, transform, textureLeft);
        }
        m_VisitNodes.Add(node);
    }

    internal void SetVisitEdgeVisibility(bool visibility)
    {
        foreach (var node in m_VisitNodes)
        {
            if (node.gameObject.activeInHierarchy)
            {
                node.SetEdgesActive(visibility);
            }
        }
    }

    private void createEdge(string fromId, string toId)
    {
        VisitEdge edge = Instantiate(edgePrefab) as VisitEdge;

        VisitNode fromNode = getNode(fromId);
        VisitNode toNode = getNode(toId);

        edge.Initialize(this, fromNode, toNode);
    }

    private VisitNode getNode(string id)
    {
        for (int i = 0; i < m_VisitNodes.Count; i++)
        {
            if (m_VisitNodes[i].Id.Equals(id))
            {
                return m_VisitNodes[i];
            }
        }
        throw new InvalidOperationException(String.Format("Tour couldn't find node for {0} in visit {1}.", id, m_VisitSettings.id));
    }

    public void MoveTo(VisitEdge edge)
    {
        VisitNode fromNode = edge.FromNode;
        VisitNode toNode = edge.ToNode;

        Debug.Log(String.Format("MoveTo({0},{1})", fromNode.Id, toNode.Id));

        fromNode.Leave();
        toNode.GoThereFrom(fromNode);
        CurrentVisitNode = toNode;

        informListener(fromNode, toNode);
    }

    private void informListener(VisitNode fromNode, VisitNode toNode)
    {
        foreach(var visitNodeChangeListener in m_VisitNodeChangeListeners)
        {
            visitNodeChangeListener.IsChangedFromTo(fromNode, toNode);
        }
    }
}
