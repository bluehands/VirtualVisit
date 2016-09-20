using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class VisitNodeStereo : MonoBehaviour {

    public string id;

    public string title;

    private List<VisitEdgeStereo> edges = new List<VisitEdgeStereo>();

    public MapNodeStereo mapNode;

    public void Initialize(string id, string title, Vector3 position, Texture sphereTexture)
    {
        init(id, title, position);

        Renderer rendLeft = transform.GetChild(0).GetComponent<Renderer>();
        rendLeft.enabled = true;
        rendLeft.material.SetTextureScale("_MainTex", new Vector2(-1, 1));
        rendLeft.material.mainTexture = sphereTexture;
        transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Default");

        Renderer rendRight = transform.GetChild(1).GetComponent<Renderer>();
        rendRight.enabled = false;
    }

    public void Initialize(string id, string title, Vector3 position, Texture sphereLeft, Texture sphereRight)
    {
        init(id, title, position);

        Renderer rendLeft = transform.GetChild(0).GetComponent<Renderer>();
        rendLeft.enabled = true;
        rendLeft.material.SetTextureScale("_MainTex", new Vector2(-1, 1));
        rendLeft.material.mainTexture = sphereLeft;
        transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Left Eye");

        Renderer rendRight = transform.GetChild(1).GetComponent<Renderer>();
        rendRight.enabled = true;
        rendRight.material.SetTextureScale("_MainTex", new Vector2(-1, 1));
        rendRight.material.mainTexture = sphereRight;
        transform.GetChild(1).gameObject.layer = LayerMask.NameToLayer("Right Eye");
    }

    private void init(string id, string title, Vector3 position)
    {
        this.id = id;
        this.title = title;

        name = "Tour Node " + id;
        transform.position = position;
    }

    public void addEdge(VisitEdgeStereo edge)
    {
        edges.Add(edge);
    }

    public List<VisitEdgeStereo> getEdges()
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
