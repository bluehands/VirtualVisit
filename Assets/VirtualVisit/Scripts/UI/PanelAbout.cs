using UnityEngine;
using System.Collections;

public class PanelAbout : MonoBehaviour {

    public void Initialize(Vector3 position, Transform parent)
    {
        transform.SetParent(parent, false);

        RectTransform panelRectTransform = GetComponent<RectTransform>();
        panelRectTransform.localPosition = position;
        panelRectTransform.localScale = new Vector3(1, 1, 1);
    }

    void Start () {
	
	}
	
	void Update () {
	
	}
}
