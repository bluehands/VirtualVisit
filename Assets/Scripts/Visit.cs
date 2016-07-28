using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Visit : MonoBehaviour {

    public VisitNode nodePrefab;

    public VisitEdge edgePrefab;

    public Map mapPrefab;

    public ButtonNext nextButtonPrefab;

    private List<VisitNode> nodes = new List<VisitNode>();

    public Map map;

    public List<VisitNode> getNodes()
    {
        return nodes;
    }

    public void Generate(VisitSettings visitSettings)
    {
        VisitNodeSettings[] nodeSettings = visitSettings.nodeSettings;
        for (int i = 0; i < nodeSettings.Length; i++)
        {
            VisitNodeSettings settings = nodeSettings[i];
            createNode(settings.id, settings.title, settings.postion, settings.sphereMaterial);
        }
        for (int i = 0; i < nodeSettings.Length; i++)
        {
            VisitNodeSettings settings = nodeSettings[i];
            for (int k = 0; k < settings.edgeIds.Length; k++)
            {
                createEdge(settings.id, settings.edgeIds[k]);
            }
        }

        map = Instantiate(mapPrefab) as Map;

        map.Generate(this);
        map.Initialize();
    }

    public void createNode(int id, string title, Vector3 position, Material sphereMaterial)
    {
        VisitNode node = Instantiate(nodePrefab) as VisitNode;
        nodes.Add(node);
        node.Initialize(id, title, position, sphereMaterial, this);
    }

    public void createEdge(int fromId, int toId)
    {
        VisitEdge edge = Instantiate(edgePrefab) as VisitEdge;
        VisitNode fromNode = getNode(fromId);
        VisitNode toNode = getNode(toId);

        ButtonNext nextButton = Instantiate(nextButtonPrefab) as ButtonNext;

        edge.Initialize(fromNode, toNode, nextButton);
    }

    private VisitNode getNode(int id)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            if(nodes[i].id == id)
            {
                return nodes[i];
            }
        }
        throw new System.InvalidOperationException("Tour couldn't find TourNode for " + id);
    }

    public void moveTo(VisitEdge edge)
    {
        VisitNode fromNode = edge.fromNode;
        VisitNode toNode = edge.toNode;
        var position = fromNode.transform.position - toNode.transform.position;
        print("move to " + position);
        transform.Translate(position);
        fromNode.unselect();
        toNode.select();
    }

}
