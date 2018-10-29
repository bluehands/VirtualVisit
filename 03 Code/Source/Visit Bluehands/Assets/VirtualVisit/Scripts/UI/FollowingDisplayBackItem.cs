using UnityEngine;
using UnityEngine.UI;

public class FollowingDisplayBackItem : MonoBehaviour {

    public ButtonInfoBack buttonInfoBackPrefab;

    private ButtonInfoBack m_ButtonInfoBack;

    internal void Initialize(Transform parent, float yaw, float pitch, ButtonListener buttonListener)
    {
        transform.SetParent(parent, false);
        transform.rotation = Quaternion.Euler(pitch, yaw, 0);

        m_ButtonInfoBack = Instantiate(buttonInfoBackPrefab) as ButtonInfoBack;
        m_ButtonInfoBack.Initialize(transform.Find("Canvas Item"), buttonListener);
        m_ButtonInfoBack.transform.localScale = new Vector3(0.1f, 0.1f, 1f);

        Text text = transform.Find("Canvas Item").Find("Text").GetComponent<Text>();
        text.text = "Schließen";
    }
}
