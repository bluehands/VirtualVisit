using UnityEngine;
using System.Collections.Generic;
using System;

public class VisitNode : MonoBehaviour {

    private const int LEFT_EYE = 0;
    private const int RIGHT_EYE = 1;

    private const string MAIN_TEXTURE = "_MainTex";
    private const string BLEND_TEXTURE = "_BlendTex";
    private const string BLEND_ALPHA = "_BlendAlpha";

    public string Id { get; private set; }

    public string Title { get; private set; }

    public Vector3 Position { get; private set; }

    public Texture SphereTextureLeft { get; private set; }

    public Texture SphereTextureRight { get; private set; }

    public bool IsStereo { get; private set; }

    public VisitEdge edgePrefab;

    public VisitMark markPrefab;

    private VisitNode m_LastFromNode;

    private List<VisitEdge> m_Edges;

    private List<VisitMark> m_Marks;

    private float m_BlendAlpha = 1;

    public enum BlendMode
    {
        Opaque,
        Cutout,
        Fade,
        Transparent
    }

    private Transform getLeftSphere()
    {
        return transform.GetChild(LEFT_EYE);
    }

    private Transform getRightSphere()
    {
        return transform.GetChild(RIGHT_EYE);
    }

    public void Initialize(string id, string title, Vector3 position, Transform parent, Texture sphereTexture)
    {
        init(id, title, position, parent);
        SphereTextureLeft = sphereTexture;
        IsStereo = false;

        initSphere(getLeftSphere(), SphereTextureLeft, LayerMask.NameToLayer("Default"));
        getRightSphere().GetComponent<Renderer>().enabled = false;
    }

    public void Initialize(string id, string title, Vector3 position, Transform parent, Texture sphereLeft, Texture sphereRight)
    {
        init(id, title, position, parent);
        SphereTextureLeft = sphereLeft;
        SphereTextureRight = sphereRight;
        IsStereo = true;

        initSphere(getLeftSphere(), SphereTextureLeft, LayerMask.NameToLayer("Left Eye"));
        initSphere(getRightSphere(), SphereTextureRight, LayerMask.NameToLayer("Right Eye"));

    }

    internal void createEdge(VisitNode toNode, VisitEdgeListener visitEdgeListener)
    {
        VisitEdge edge = Instantiate(edgePrefab) as VisitEdge;

        edge.Initialize(this, toNode, visitEdgeListener);

        m_Edges.Add(edge);
    }

    internal void createEdge(VisitNode toNode, VisitEdgeListener visitEdgeListener, float u, float v)
    {
        VisitEdge edge = Instantiate(edgePrefab) as VisitEdge;

        edge.Initialize(this, toNode, visitEdgeListener, u, v);

        m_Edges.Add(edge);
    }

    internal void createMark(string title, string description, float u, float v)
    {
        VisitMark mark = Instantiate(markPrefab) as VisitMark;

        mark.Initialize(title, description, u, v, transform);

        m_Marks.Add(mark);
    }

    private void initSphere(Transform sphere, Texture texture, int layerMask)
    {
        Renderer renderer = sphere.GetComponent<Renderer>();
        renderer.enabled = true;
        renderer.material.SetTextureScale(MAIN_TEXTURE, new Vector2(-1, 1));
        renderer.material.mainTexture = texture;
        sphere.gameObject.layer = layerMask;
    }

    private void init(string id, string title, Vector3 position, Transform parent)
    {
        Id = id;
        Title = title;
        Position = position;

        transform.parent = parent;
        name = String.Format("VisitNode({0})", id);

        m_Edges = new List<VisitEdge>();
        m_Marks = new List<VisitMark>();
    }

    internal bool IsStereoView()
    {
        return getLeftSphere().gameObject.layer == LayerMask.NameToLayer("Left Eye");
    }

    internal bool ToggleStereoView()
    {
        if(IsStereo)
        {
            if (getLeftSphere().gameObject.layer == LayerMask.NameToLayer("Left Eye"))
            {
                getLeftSphere().gameObject.layer = LayerMask.NameToLayer("Default");
                getRightSphere().gameObject.SetActive(false);
                return false;
            }
            else
            {
                getLeftSphere().gameObject.layer = LayerMask.NameToLayer("Left Eye");
                getRightSphere().gameObject.SetActive(true);
                return true;
            }
        }
        return false;
    }

    public void AddEdge(VisitEdge edge)
    {
        m_Edges.Add(edge);
    }

    public List<VisitEdge> GetEdges()
    {
        return m_Edges;
    }

    public void SetEdgesActive(bool active)
    {
        foreach (var edge in m_Edges)
        {
            edge.gameObject.SetActive(active);
        }
    }

    public void SetMarksActive(bool active)
    {
        foreach (var mark in m_Marks)
        {
            mark.gameObject.SetActive(active);
        }
    }

    internal void Leave()
    {
        gameObject.SetActive(false);
    }

    internal void GoThere()
    {
        setAlpha(0.0f);

        gameObject.SetActive(true);
    }

    internal void GoThereFrom(VisitNode node)
    {
        setAlpha(1.0f);

        m_BlendAlpha = 1;
        m_LastFromNode = node;

        SetEdgesActive(false);
        gameObject.SetActive(true);
    }

    private void setAlpha(float alpha)
    {
        Renderer rendLeft = getLeftSphere().GetComponent<Renderer>();
        Renderer rendRight = getRightSphere().GetComponent<Renderer>();

        rendLeft.material.SetFloat(BLEND_ALPHA, alpha);
        rendRight.material.SetFloat(BLEND_ALPHA, alpha);
    }

    void Update()
    {
        if(m_LastFromNode != null)
        {
            Renderer rendLeft = getLeftSphere().GetComponent<Renderer>();
            Renderer rendRight = getRightSphere().GetComponent<Renderer>();
            if (m_BlendAlpha == 1)
            {
                rendLeft.material.SetTexture(BLEND_TEXTURE, m_LastFromNode.SphereTextureLeft);
                if (IsStereo)
                {
                    if (m_LastFromNode.IsStereo)
                    {
                        rendRight.material.SetTexture(BLEND_TEXTURE, m_LastFromNode.SphereTextureRight);
                    }
                    else
                    {
                        rendRight.material.SetTexture(BLEND_TEXTURE, m_LastFromNode.SphereTextureLeft);
                    }
                }
            }
            if (m_BlendAlpha >= 0)
            {
                setAlpha(m_BlendAlpha);
            }
            if(m_BlendAlpha <= 0)
            {
                setAlpha(0);
                m_LastFromNode = null;
                SetEdgesActive(true);
            }
            m_BlendAlpha = m_BlendAlpha - 0.1f;
        }
        
    }
    
}
