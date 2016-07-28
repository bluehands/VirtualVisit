using UnityEngine;
using System.Collections;
using System;

public class MapNode : MonoBehaviour {

    public Material selectedMaterial;

    public Material unselectedMaterial;

    public void Initialize(VisitNode visitNode, Map map)
    {
        transform.position = visitNode.transform.position;
        transform.parent = map.transform;
        visitNode.mapNode = this;
    }

    internal void unselect()
    {
        transform.GetChild(0).GetComponent<Renderer>().material = unselectedMaterial;
    }

    internal void select()
    {
        transform.GetChild(0).GetComponent<Renderer>().material = selectedMaterial;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
