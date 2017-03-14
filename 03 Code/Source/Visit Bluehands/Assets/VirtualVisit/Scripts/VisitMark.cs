using UnityEngine;

public class VisitMark : MonoBehaviour {

    public ButtonMark buttonMarkPrefab;

    private ButtonMark m_ButtonMark;

    internal void Initialize(string title, string description, float u, float v, Transform parent)
    {
        transform.SetParent(parent, false);
        rotateToMarke(u, v);

        m_ButtonMark = Instantiate(buttonMarkPrefab) as ButtonMark;
        m_ButtonMark.Initialize(title, description, transform.FindChild("Canvas Mark").transform);
    }

    private void rotateToMarke(float u, float v)
    {
        float xRot = 180 * v - 90;
        float yRot = 360 * u + 184;
        Quaternion rotation = Quaternion.Euler(new Vector3(xRot, yRot, 0));
        transform.rotation = rotation;
    }
}
