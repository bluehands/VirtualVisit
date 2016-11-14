using UnityEngine;

public class MapNode : MonoBehaviour {

    public Material selectedMaterial;

    public Material unselectedMaterial;

    public string Id { get; private set; }

    public void Initialize(string id, Vector3 position, string title, Transform parent)
    {
        Id = id;
        transform.position = position;
        transform.parent = parent;
        addTitle(title);
    }

    private void addTitle(string title)
    {
        var theText = new GameObject("NodeTitle");
        theText.transform.parent = this.transform;
        theText.transform.position = transform.position + new Vector3(2, 5, 0);

        var textMesh = theText.AddComponent<TextMesh>();
        textMesh.text = title;
        textMesh.fontSize = 50;
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
