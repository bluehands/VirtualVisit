using UnityEngine;
using System.Collections.Generic;
using System;

public class VisitNode : MonoBehaviour {

    private const int LEFT_EYE = 0;
    private const int RIGHT_EYE = 1;

    private int LAYER_DEFAULT;
    private int LAYER_LEFT_EYE;
    private int LAYER_RIGHT_EYE;

    private const string MAIN_TEXTURE = "_MainTex";
    private const string BLEND_TEXTURE = "_BlendTex";
    private const string BLEND_ALPHA = "_BlendAlpha";

    public string Id { get; private set; }

    public string Title { get; private set; }

    public Vector3 Position { get; private set; }

    public MapNode MapNode { get; set; }

    public Texture SphereTextureLeft { get; private set; }

    public Texture SphereTextureRight { get; private set; }

    public bool IsStereo { get; private set; }

    private VisitNode m_LastFromNode;

    private List<VisitEdge> m_Edges;

    private float m_BlendAlpha = 1;

    public enum BlendMode
    {
        Opaque,
        Cutout,
        Fade,
        Transparent
    }

    void Start()
    {
        LAYER_DEFAULT = LayerMask.NameToLayer("Default");
        LAYER_LEFT_EYE = LayerMask.NameToLayer("Left Eye");
        LAYER_RIGHT_EYE = LayerMask.NameToLayer("Right Eye");
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

        initSphere(getLeftSphere(), SphereTextureLeft, LAYER_DEFAULT);
        getRightSphere().GetComponent<Renderer>().enabled = false;
    }

    public void Initialize(string id, string title, Vector3 position, Transform parent, Texture sphereLeft, Texture sphereRight)
    {
        init(id, title, position, parent);
        SphereTextureLeft = sphereLeft;
        SphereTextureRight = sphereRight;
        IsStereo = true;

        initSphere(getLeftSphere(), SphereTextureLeft, LAYER_LEFT_EYE);
        initSphere(getRightSphere(), SphereTextureRight, LAYER_RIGHT_EYE);

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
    }

    internal bool IsStereoView()
    {
        return getLeftSphere().gameObject.layer == LayerMask.NameToLayer("Left Eye");
    }

    internal bool ToggleStereoView()
    {
        if(IsStereo)
        {
            if (getLeftSphere().gameObject.layer == LAYER_LEFT_EYE)
            {
                getLeftSphere().gameObject.layer = LAYER_DEFAULT;
                getRightSphere().gameObject.SetActive(false);
                return false;
            }
            else
            {
                getLeftSphere().gameObject.layer = LAYER_LEFT_EYE;
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

    public void SetEdgesActive(bool ative)
    {
        foreach (var edge in m_Edges)
        {
            edge.gameObject.SetActive(ative);
        }
    }

    internal void Leave()
    {
        gameObject.SetActive(false);
        MapNode.Unselect();
    }

    internal void GoThere()
    {
        setAlpha(0.0f);

        gameObject.SetActive(true);
        MapNode.Select();
    }

    internal void GoThereFrom(VisitNode node)
    {
        setAlpha(1.0f);

        m_BlendAlpha = 1;
        m_LastFromNode = node;

        SetEdgesActive(false);
        gameObject.SetActive(true);
        MapNode.Select();
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
            if(m_BlendAlpha < 0)
            {
                m_LastFromNode = null;
                SetEdgesActive(true);
            }
            m_BlendAlpha = m_BlendAlpha - 0.01f;
        }
        
    }
    
}
