using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FollowingDisplayItem : MonoBehaviour, ButtonListener
{
    public ButtonGo buttonGoPrefab;

    private ButtonGo m_ButtonGo;

    private SwitchTourListener m_SwitchTourListener;

    private string m_VisitId;

    internal void Initialize(Transform parent, string visitId, string visitTitle, float yaw, float pitch, SwitchTourListener switchTourListener)
    {
        m_VisitId = visitId;
        m_SwitchTourListener = switchTourListener;

        transform.SetParent(parent, false);
        transform.rotation = Quaternion.Euler(pitch, yaw, 0);

        m_ButtonGo = Instantiate(buttonGoPrefab) as ButtonGo;
        m_ButtonGo.Initialize(transform.FindChild("Canvas Item"), this);
        m_ButtonGo.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
        m_ButtonGo.GetComponent<RectTransform>().localPosition = new Vector3(7, -2, 0);

        Text text = transform.FindChild("Canvas Item").FindChild("Text").GetComponent<Text>();
        text.text = visitTitle;

        
        Texture preview = null;
        TexturesFactory.TryToLoadPreviewTexture(visitId, out preview);
        if(preview != null)
        {
            var rawImage = transform.FindChild("Canvas Item").FindChild("Background Image").GetComponent<RawImage>();
            rawImage.texture = preview;
        }

        Texture logo = null;
        TexturesFactory.TryToLoadLogoTexture(visitId, out logo);
        if (logo != null)
        {
            var rawImage = transform.FindChild("Canvas Item").FindChild("Logo Image").GetComponent<RawImage>();
            rawImage.texture = logo;
        }
    }

    public void DoButtonAction(Type clazz)
    {
        if (clazz.Equals(typeof(ButtonGo)))
        {
            m_SwitchTourListener.SwitchTour(m_VisitId);
        }
    }
}
