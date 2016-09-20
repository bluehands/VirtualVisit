using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class VisitNode : MonoBehaviour {

    public int id;

    public string title;

    private List<VisitEdge> edges = new List<VisitEdge>();

    public MapNode mapNode;

    public void Initialize(int id, string title, Vector3 position, Material sphereMaterial, Visit visit)
    {
        this.id = id;
        this.title = title;

        name = "Tour Node " + id;
        transform.parent = visit.transform;
        transform.position = position;

        transform.GetChild(0).GetComponent<Renderer>().material = sphereMaterial;
    }

    public void addEdge(VisitEdge edge)
    {
        edges.Add(edge);
    }

    public List<VisitEdge> getEdges()
    {
        return edges;
    }

    internal void unselect()
    {
        mapNode.unselect();
    }

    internal void select()
    {
        mapNode.select();
    }
}
