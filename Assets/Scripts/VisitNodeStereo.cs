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

    public enum BlendMode
    {
        Opaque,
        Cutout,
        Fade,
        Transparent
    }

    public void Initialize(string id, string title, Vector3 position, Transform parent, Texture sphereTexture)
    {
        init(id, title, position, parent);

        Renderer rendLeft = transform.GetChild(0).GetComponent<Renderer>();
        rendLeft.enabled = true;
        rendLeft.material.SetTextureScale("_MainTex", new Vector2(-1, 1));
        rendLeft.material.mainTexture = sphereTexture;

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
        gameObject.SetActive(false);
        //isDisappearing = true;
        //isAppearing = false;
        mapNode.unselect();
    }

    internal void select()
    {
        gameObject.SetActive(true);
        //isAppearing = true;
        //isDisappearing = false;
        mapNode.select();
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
