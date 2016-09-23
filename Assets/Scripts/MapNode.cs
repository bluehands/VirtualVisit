using UnityEngine;

public class MapNode : MonoBehaviour {

    public Material selectedMaterial;

    public Material unselectedMaterial;

    public void Initialize(Vector3 position, Transform parent)
    {
        transform.position = position;
        transform.parent = parent;
    }

    internal void Unselect()
    {
        transform.GetChild(0).GetComponent<Renderer>().material = unselectedMaterial;
    }

    internal void Select()
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
