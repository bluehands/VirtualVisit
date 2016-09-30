using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PanelMenuItem : MonoBehaviour, ButtonListener {

    public ButtonGo buttonGoPrefab;

    public ButtonInfo buttonInfoPrefab;

    private ButtonGo m_ButtonGo;

    private ButtonInfo m_ButtonInfo;

    private string m_NextVisitId;
    private Stage m_Stage;
    private int m_Index;

    public void Initialize(Vector3 position, VisitSettings visitSetting, Stage stage, int index,  VisitSettingsFactory visitSettingsFactory, Transform parent)
    {
        m_NextVisitId = visitSetting.id;
        m_Stage = stage;
        m_Index = index;

        transform.parent = parent;
        transform.name = "Menu Panel " + visitSetting.id;

        RectTransform panelRectTransform = GetComponent<RectTransform>();
        panelRectTransform.localPosition = position;
        panelRectTransform.localScale = new Vector3(1, 1, 1);

        m_ButtonGo = Instantiate(buttonGoPrefab) as ButtonGo;
        m_ButtonGo.Initialize(transform, this);
        m_ButtonGo.GetComponent<RectTransform>().localPosition = new Vector3(130, -80, 0);

        m_ButtonInfo = Instantiate(buttonInfoPrefab) as ButtonInfo;
        m_ButtonInfo.Initialize(transform, this);
        m_ButtonInfo.GetComponent<RectTransform>().localPosition = new Vector3(-130, -80, 0);

        RawImage rawImage = transform.FindChild("Background Image").GetComponent<RawImage>();

        Texture textureLeft = null;
        Texture textureRight = null;

        visitSettingsFactory.TryToLoadTextures(visitSetting, visitSetting.getPreviewNodeSetting(), out textureLeft, out textureRight);

        rawImage.texture = textureLeft;

        Text text = transform.FindChild("Headline Text").GetComponent<Text>();
        text.text = visitSetting.title;
    }

    void Start () {
	
	}
	
	void Update () {
	
	}

    public void DoButtonAction(Type clazz)
    {
        if (clazz.Equals(typeof(ButtonGo)))
        {
            ApplicationModel.SelectedVisitId = m_NextVisitId;

            SceneManager.LoadScene(0);
        }
        if (clazz.Equals(typeof(ButtonInfo)))
        {
            m_Stage.HideMainMenu();
            m_Stage.ShowInfo(m_Index);
        }
    }
}
