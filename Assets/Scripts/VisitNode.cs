using UnityEngine;
using System.Collections.Generic;
using System;

public class VisitNode : MonoBehaviour {

    private const int LEFT_EYE = 0;
    private const int RIGHT_EYE = 1;

    public string Id { get; private set; }

    public string Title { get; private set; }

    public Vector3 Position { get; private set; }

    public MapNode MapNode { get; set; }

    private List<VisitEdge> m_Edges;

    private bool m_IsStereo;

    //private bool isAppearing;
    //private bool isDisappearing;

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
        m_IsStereo = false;

        Renderer rendLeft = getLeftSphere().GetComponent<Renderer>();
        rendLeft.enabled = true;
        rendLeft.material.SetTextureScale("_MainTex", new Vector2(-1, 1));
        rendLeft.material.mainTexture = sphereTexture;

        getLeftSphere().gameObject.layer = LayerMask.NameToLayer("Default");

        Renderer rendRight = getRightSphere().GetComponent<Renderer>();
        rendRight.enabled = false;
    }

    public void Initialize(string id, string title, Vector3 position, Transform parent, Texture sphereLeft, Texture sphereRight)
    {
        init(id, title, position, parent);
        m_IsStereo = true;

        Renderer rendLeft = getLeftSphere().GetComponent<Renderer>();
        rendLeft.enabled = true;
        rendLeft.material.SetTextureScale("_MainTex", new Vector2(-1, 1));
        rendLeft.material.mainTexture = sphereLeft;
        getLeftSphere().gameObject.layer = LayerMask.NameToLayer("Left Eye");

        Renderer rendRight = transform.GetChild(1).GetComponent<Renderer>();
        rendRight.enabled = true;
        rendRight.material.SetTextureScale("_MainTex", new Vector2(-1, 1));
        rendRight.material.mainTexture = sphereRight;
        getRightSphere().gameObject.layer = LayerMask.NameToLayer("Right Eye");
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
        if(m_IsStereo)
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

    internal void Unselect()
    {
        gameObject.SetActive(false);
        //isDisappearing = true;
        //isAppearing = false;
        MapNode.Unselect();
    }

    internal void Select()
    {
        gameObject.SetActive(true);
        //isAppearing = true;
        //isDisappearing = false;
        MapNode.Select();
    }
    /*
    void Update()
    {
        if(isAppearing)
        {
            float a = getAlpha();
            
            if (getMode() != BlendMode.Transparent)
            {
                setMode(BlendMode.Transparent);
            }
            if (a <= 1.0f)
            {
                setAlpha(a + 0.009f);
            }
            else
            {
                isAppearing = false;
                setMode(BlendMode.Opaque);
            }
        }
        if (isDisappearing)
        {
            float a = getAlpha();
            if(getMode() != BlendMode.Transparent)
            {
                setMode(BlendMode.Transparent);
            }
            if (a >= 0.0f)
            {
                setAlpha(a - 0.01f);
            }
            else
            {
                gameObject.SetActive(false);
                isDisappearing = false;
                setMode(BlendMode.Opaque);
            }
        }
    }

    private BlendMode getMode()
    {
        Material materialLeft = transform.GetChild(0).GetComponent<Renderer>().material;
        var value = materialLeft.GetFloat("_Mode");
        if (value == 1)
            return BlendMode.Cutout;
        if (value == 2)
            return BlendMode.Fade;
        if (value == 3)
            return BlendMode.Transparent;
        return BlendMode.Opaque;
    }

    private void setMode(BlendMode mode)
    {
        setMaterialMode(transform.GetChild(0).GetComponent<Renderer>().material, mode);
        setMaterialMode(transform.GetChild(1).GetComponent<Renderer>().material, mode);
    }

    private void setMaterialMode(Material material, BlendMode mode)
    {
        if(mode == BlendMode.Transparent)
        {
            material.SetFloat("_Mode", (float)mode);
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = 3000;
        } else
        {
            material.SetFloat("_Mode", (float)mode);
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            material.SetInt("_ZWrite", 1);
            material.EnableKeyword("_ALPHATEST_ON");
            material.DisableKeyword("_ALPHABLEND_ON");
            material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = 2000;
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
    */
}
