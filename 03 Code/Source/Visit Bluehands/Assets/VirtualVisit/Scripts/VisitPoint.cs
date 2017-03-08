using UnityEngine;
using System.Collections.Generic;
using System;

public class VisitPoint : MonoBehaviour {

    enum PointState
    {
        None,
        Visible,
        ChangePoint,
        FadedOut,
        FadingIn,
        FadingOut
    }

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

    public Texture FadeOutTexture;

    public bool IsStereo { get; private set; }

    public VisitPath pathPrefab;

    public VisitMark markPrefab;

    //private VisitPoint m_LastFromPoint;
    private Texture m_FadeOutTextureLeft;
    private Texture m_FadeOutTextureRight;

    private List<VisitPath> m_Paths;

    private List<VisitMark> m_Marks;

    private float m_BlendAlpha = 1;

    private PointState m_PointState = PointState.None;

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

    internal void createEdge(VisitPoint toPoint, VisitPathListener visitPathListener)
    {
        var path = Instantiate(pathPrefab) as VisitPath;

        path.Initialize(this, toPoint, visitPathListener);

        m_Paths.Add(path);
    }

    internal void createEdge(VisitPoint toPoint, VisitPathListener visitPathListener, float u, float v)
    {
        var path = Instantiate(pathPrefab) as VisitPath;

        path.Initialize(this, toPoint, visitPathListener, u, v);

        m_Paths.Add(path);
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
        name = String.Format("VisitPoint({0})", id);

        m_Paths = new List<VisitPath>();
        m_Marks = new List<VisitMark>();
    }

    internal bool IsStereoView()
    {
        return getLeftSphere().gameObject.layer == LayerMask.NameToLayer("Left Eye");
    }

    internal bool ToggleStereoView()
    {
        if (IsStereo)
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

    public void AddPath(VisitPath path)
    {
        m_Paths.Add(path);
    }

    public List<VisitPath> GetPaths()
    {
        return m_Paths;
    }

    public void SetEdgesActive(bool active)
    {
        foreach (var edge in m_Paths)
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

    internal void GoThereFrom(VisitPoint point)
    {
        setAlpha(1.0f);

        m_BlendAlpha = 1;
        m_FadeOutTextureLeft = point.SphereTextureLeft;
        m_FadeOutTextureRight = IsStereo && point.IsStereo ? point.SphereTextureRight : point.SphereTextureLeft;

        SetEdgesActive(false);
        gameObject.SetActive(true);
        m_PointState = PointState.ChangePoint;
    }

    private void setAlpha(float alpha)
    {
        Renderer rendLeft = getLeftSphere().GetComponent<Renderer>();
        Renderer rendRight = getRightSphere().GetComponent<Renderer>();

        rendLeft.material.SetFloat(BLEND_ALPHA, alpha);
        rendRight.material.SetFloat(BLEND_ALPHA, alpha);
    }

    public void FadeOut()
    {
        m_BlendAlpha = 1;

        m_PointState = PointState.FadingOut;
    }

    public void FadeIn()
    {
        Renderer rendLeft = getLeftSphere().GetComponent<Renderer>();
        Renderer rendRight = getRightSphere().GetComponent<Renderer>();

        rendLeft.material.SetTexture(BLEND_TEXTURE, FadeOutTexture);
        rendRight.material.SetTexture(BLEND_TEXTURE, FadeOutTexture);

        setAlpha(0);
    }

    void Update()
    {
        Renderer rendLeft = getLeftSphere().GetComponent<Renderer>();
        Renderer rendRight = getRightSphere().GetComponent<Renderer>();
        if (m_PointState == PointState.ChangePoint)
        {
            if (m_BlendAlpha == 1)
            {
                rendLeft.material.SetTexture(BLEND_TEXTURE, m_FadeOutTextureLeft);
                rendRight.material.SetTexture(BLEND_TEXTURE, m_FadeOutTextureRight);
            }
            if (m_BlendAlpha >= 0)
            {
                setAlpha(m_BlendAlpha);
            }
            if (m_BlendAlpha <= 0)
            {
                setAlpha(0);
                m_FadeOutTextureLeft = null;
                m_FadeOutTextureRight = null;
                SetEdgesActive(true);
                m_PointState = PointState.Visible;
            }
            m_BlendAlpha = m_BlendAlpha - 0.1f;
        } else if(m_PointState == PointState.FadingOut)
        {
            if (m_BlendAlpha == 1)
            {
                rendLeft.material.SetTexture(BLEND_TEXTURE, FadeOutTexture);
                rendRight.material.SetTexture(BLEND_TEXTURE, FadeOutTexture);
            }
            if (m_BlendAlpha >= 0)
            {
                setAlpha(0.5f - (m_BlendAlpha/2));
            }
            if (m_BlendAlpha <= 0)
            {
                setAlpha(0.5f);
                m_PointState = PointState.FadedOut;
            }
            m_BlendAlpha = m_BlendAlpha - 0.1f;
        } else if (m_PointState == PointState.FadingIn)
        {
            if (m_BlendAlpha == 1)
            {
                rendLeft.material.SetTexture(BLEND_TEXTURE, FadeOutTexture);
                rendRight.material.SetTexture(BLEND_TEXTURE, FadeOutTexture);
            }
            if (m_BlendAlpha >= 0)
            {
                setAlpha(m_BlendAlpha);
            }
            if (m_BlendAlpha <= 0)
            {
                setAlpha(0);
                m_PointState = PointState.Visible;
            }
            m_BlendAlpha = m_BlendAlpha - 0.1f;
        }
    }

}
