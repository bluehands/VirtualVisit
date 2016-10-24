using UnityEngine;
using UnityEngine.UI;

public class ButtonMark : MonoBehaviour
{
    public void Initialize(string title, string description, Transform parent)
    {
        transform.SetParent(parent, false);

        transform.Translate(new Vector3(2.5f, 2.5f, 0));
        transform.localScale = new Vector3(0.01f, 0.01f, 1);

        transform.FindChild("Title Text").GetComponent<Text>().text = title;
        transform.FindChild("Context Text").GetComponent<Text>().text = description;
        transform.FindChild("Context Text Shadow").GetComponent<Text>().text = description;
    }
}
