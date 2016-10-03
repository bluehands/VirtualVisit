using UnityEngine;
using UnityEngine.UI;

public class VisitMark : MonoBehaviour {

    internal void Initialize(string title, float u, float v, Transform parent)
    {
        transform.SetParent(parent, false);
        rotateToMarke(u, v);


        Text text = transform.FindChild("Canvas Mark").FindChild("Mark Text").GetComponent<Text>();
        text.text = title;
    }

    void Start () {
	
	}

	void Update () {
	}

    private void rotateToMarke(float u, float v)
    {
        float xRot = 180 * v - 90;
        float yRot = 360 * u + 184;
        Quaternion rotation = Quaternion.Euler(new Vector3(xRot, yRot, 0));
        transform.rotation = rotation;
    }
}
