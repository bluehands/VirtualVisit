using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class VisitNodeStereo : MonoBehaviour {

    public string id;

    public string title;

    public Vector3 position;

    private List<VisitEdgeStereo> edges = new List<VisitEdgeStereo>();

    public MapNodeStereo mapNode;

    private bool isAppearing;
    private bool isDisappearing;

    public void Initialize(string id, string title, Vector3 position, Transform parent, Texture sphereTexture)
    {
        init(id, title, position, parent);

        Renderer rendLeft = transform.GetChild(0).GetComponent<Renderer>();
        rendLeft.enabled = true;
        rendLeft.material.SetTextureScale("_MainTex", new Vector2(-1, 1));
        rendLeft.material.mainTexture = sphereTexture;
/*
        Color color = rendLeft.material.GetColor("_Color");
        color.a = 0.5f;
        rendLeft.material.SetColor("_Color", color);*/

        transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Default");

        Renderer rendRight = transform.GetChild(1).GetComponent<Renderer>();
        rendRight.enabled = false;
    }

    public void Initialize(string id, string title, Vector3 position, Transform parent, Texture sphereLeft, Texture sphereRight)
    {
        init(id, title, position, parent);

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

    private void init(string id, string title, Vector3 position, Transform parent)
    {
        this.id = id;
        this.title = title;
        this.position = position;
        transform.parent = parent;

        name = String.Format("VisitNode({0})", id);
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
        isDisappearing = true;
        isAppearing = false;
        mapNode.unselect();
    }

    internal void select()
    {
        gameObject.SetActive(true);
        isAppearing = true;
        isDisappearing = false;
        mapNode.select();
    }

    void Update()
    {
        if(isAppearing)
        {
            float a = getAlpha();
            if(a <= 1.0f)
            {
                setAlpha(a + 0.01f);
            } else
            {
                isAppearing = false;
            }
        }
        if (isDisappearing)
        {
            float a = getAlpha();
            if (a >= 0.0f)
            {
                setAlpha(a - 0.01f);
            }
            else
            {
                gameObject.SetActive(false);
                isDisappearing = false;
            }
        }
    }

    private void setAlpha(float apha) {
        Renderer rendLeft = transform.GetChild(0).GetComponent<Renderer>();
        Color colorLeft = rendLeft.material.GetColor("_Color");
        colorLeft.a = apha;
        rendLeft.material.SetColor("_Color", colorLeft);

        Renderer rendRight = transform.GetChild(1).GetComponent<Renderer>();
        Color colorRight = rendRight.material.GetColor("_Color");
        colorRight.a = apha;
        rendRight.material.SetColor("_Color", colorRight);
    }

    private float getAlpha()
    {
        Renderer rendLeft = transform.GetChild(0).GetComponent<Renderer>();
        return rendLeft.material.GetColor("_Color").a;
    }

}
