using UnityEngine;
using System.Collections;
using System;

public class MapNodeStereo : MonoBehaviour {

    public Material selectedMaterial;

    public Material unselectedMaterial;

    public void Initialize(Vector3 position, Transform parent)
    {
        transform.position = position;
        transform.parent = parent;
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
