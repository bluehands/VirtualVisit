using UnityEngine;
using UnityEngine.UI;
using System;

public class Stage : MonoBehaviour, ButtonListener {

    public ButtonGo buttonGoPrefab;

    public ButtonInfo buttonInfoPrefab;

    public ButtonPageLeft buttonPageLeftPrefab;

    public ButtonPageRight buttonPageRightPrefab;

    private VisitSettingsFactory m_VisitSettingsFactory;

    private GameObject[] m_MainMenuPanels;

    private GameObject[] m_InfoMenuPanels;

    private GameObject m_CanvaseMenu;

    public void Generate(VisitSettingsFactory visitSettingsFactory)
    {
        this.m_VisitSettingsFactory = visitSettingsFactory;

        m_CanvaseMenu = GameObject.Find("Menu Canvas");
        var canvasePage = GameObject.Find("Page Canvas");

        generatePage(canvasePage.transform);
        generateMainMenu(m_CanvaseMenu.transform);
        ShowMainMenu();
        generateInfoMenus(m_CanvaseMenu.transform);
        HideInfo();
    }

    public void ShowInfo(int infoIndex)
    {
        GameObject visitInfoPanel = m_InfoMenuPanels[infoIndex];
        visitInfoPanel.SetActive(true);
    }

    public void HideInfo()
    {
        if(m_InfoMenuPanels != null)
        {
            foreach (var panel in m_InfoMenuPanels)
            {
                panel.SetActive(false);
            }
        }
    }


    public void ShowMainMenu()
    {
        if (m_MainMenuPanels != null)
        {
            foreach (var panel in m_MainMenuPanels)
            {
                panel.SetActive(true);
            }
        }
    }

    public void HideMainMenu()
    {
        if (m_MainMenuPanels != null)
        {
            foreach (var panel in m_MainMenuPanels)
            {
                panel.SetActive(false);
            }
        }
    }

    private void generateInfoMenus(Transform parent)
    {
        var visitSettings = m_VisitSettingsFactory.GetVisitSettings();
        m_InfoMenuPanels = new GameObject[visitSettings.Length];

        for (int i = 0; i < visitSettings.Length; i++)
        {
            generateInfoMenu(i, parent);
        }
    }

    private void moveMainMenuToLeft()
    {
        RectTransform panelRectTransform = m_CanvaseMenu.GetComponent<RectTransform>();
        var position = panelRectTransform.localPosition;
        position.x -= 0.1f;
        panelRectTransform.localPosition = position;
    }

    private void moveMainMenuToRight()
    {
        RectTransform panelRectTransform = m_CanvaseMenu.GetComponent<RectTransform>();
        var position = panelRectTransform.localPosition;
        position.x += 0.1f;
        panelRectTransform.localPosition = position;
    }

    private void generateInfoMenu(int index, Transform parent)
    {
        VisitSettings visitSetting = m_VisitSettingsFactory.GetVisitSettings()[index];
        GameObject mainMenu = m_MainMenuPanels[index];

        GameObject panel = new GameObject("InfoPanel " + visitSetting.id);
        panel.AddComponent<CanvasRenderer>();
        RectTransform panelRectTransform = panel.AddComponent<RectTransform>();
        panelRectTransform.sizeDelta = new Vector2(400, 300);
        panelRectTransform.localPosition = mainMenu.GetComponent<RectTransform>().localPosition;
        Image image = panel.AddComponent<Image>();
        image.color = new Color32(255, 255, 255, 157);
        panel.transform.SetParent(parent, false);
        /*
        GameObject backgroundImage = new GameObject("Background Image");
        RectTransform panelRectTransformImage = backgroundImage.AddComponent<RectTransform>();
        panelRectTransformImage.sizeDelta = new Vector2(300, 200);
        RawImage rawImage = backgroundImage.AddComponent<RawImage>();

        Texture textureLeft = null;
        Texture textureRight = null;

        m_VisitSettingsFactory.TryToLoadTextures(visitSetting, visitSetting.getPreviewNodeSetting(), out textureLeft, out textureRight);

        rawImage.texture = textureLeft;
        backgroundImage.transform.SetParent(panel.transform, false);*/

        ButtonInfo buttonInfo = Instantiate(buttonInfoPrefab) as ButtonInfo;
        buttonInfo.Initialize(-1, this, panel.transform);
        buttonInfo.GetComponent<RectTransform>().localPosition = new Vector3(-130, -80, 0);

        GameObject headlineText = new GameObject("Headline Text");
        RectTransform headlineTextRectTransform = headlineText.AddComponent<RectTransform>();
        headlineTextRectTransform.sizeDelta = new Vector2(3000, 400);
        headlineTextRectTransform.localScale = new Vector3(0.1f, 0.1f, 1);
        headlineTextRectTransform.localPosition = new Vector3(-35, 120, 0);
        Text text = headlineText.AddComponent<Text>();
        text.text = visitSetting.title;
        text.fontSize = 200;
        text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        headlineText.transform.SetParent(panel.transform, false);

        GameObject descriptionText = new GameObject("Info Text");
        RectTransform descriptionTextRectTransform = descriptionText.AddComponent<RectTransform>();
        descriptionTextRectTransform.sizeDelta = new Vector2(3000, 500);
        descriptionTextRectTransform.localScale = new Vector3(0.1f, 0.1f, 1);
        descriptionTextRectTransform.localPosition = new Vector3(80, 0, 0);
        Text textDescription = descriptionText.AddComponent<Text>();

        textDescription.text = String.Format("Erstellt \n\t von: {0} \n\t am: {1}", visitSetting.author, visitSetting.created);

        textDescription.fontSize = 140;
        textDescription.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        descriptionText.transform.SetParent(panel.transform, false);

        m_InfoMenuPanels[index] = panel;
    }

    private void generateMainMenu(Transform parent)
    {
        var visitSettings = m_VisitSettingsFactory.GetVisitSettings();

        m_MainMenuPanels = new GameObject[visitSettings.Length + 1];

        var xStep = 400;
        var currentStep = -600;
        for (int i=0; i< visitSettings.Length; i++)
        {
            generateMenu(i, new Vector3(currentStep, 0, 0), parent);
            currentStep += xStep;
        }

        generateAbout(visitSettings.Length, new Vector3(currentStep, 0, 0), parent);
    }

    private void generatePage(Transform parent)
    {
        ButtonPageLeft buttonPageLeft = Instantiate(buttonPageLeftPrefab) as ButtonPageLeft;
        buttonPageLeft.Initialize(parent.transform, this);
        buttonPageLeft.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(750, 0, -1);

        ButtonPageRight buttonPageRight = Instantiate(buttonPageRightPrefab) as ButtonPageRight;
        buttonPageRight.Initialize(parent.transform, this);
        buttonPageRight.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(-750, 0, -1);
    }

    private void generateMenu(int index, Vector3 position, Transform parent)
    {
        VisitSettings visitSetting = m_VisitSettingsFactory.GetVisitSettings()[index];

        GameObject panel = new GameObject("MenuPanel " + visitSetting.id);
        panel.AddComponent<CanvasRenderer>();
        RectTransform panelRectTransform = panel.AddComponent<RectTransform>();
        panelRectTransform.sizeDelta = new Vector2(400, 300);
        panelRectTransform.localPosition = position;
        Image i = panel.AddComponent<Image>();
        i.color = new Color32(255, 255, 255, 157);
        panel.transform.SetParent(parent, false);

        GameObject backgroundImage = new GameObject("Background Image");
        RectTransform panelRectTransformImage = backgroundImage.AddComponent<RectTransform>();
        panelRectTransformImage.sizeDelta = new Vector2(300, 200);
        RawImage rawImage = backgroundImage.AddComponent<RawImage>();

        Texture textureLeft = null;
        Texture textureRight = null;

        m_VisitSettingsFactory.TryToLoadTextures(visitSetting, visitSetting.getPreviewNodeSetting(), out textureLeft, out textureRight);

        rawImage.texture = textureLeft;
        backgroundImage.transform.SetParent(panel.transform, false);

        ButtonGo buttonGo = Instantiate(buttonGoPrefab) as ButtonGo;
        buttonGo.Initialize(visitSetting.id, panel.transform);
        buttonGo.GetComponent<RectTransform>().localPosition = new Vector3(130, -80, 0);

        ButtonInfo buttonInfo = Instantiate(buttonInfoPrefab) as ButtonInfo;
        buttonInfo.Initialize(index, this, panel.transform);
        buttonInfo.GetComponent<RectTransform>().localPosition = new Vector3(-130, -80, 0);

        GameObject headlineText = new GameObject("Headline Text");
        RectTransform headlineTextRectTransform = headlineText.AddComponent<RectTransform>();
        headlineTextRectTransform.sizeDelta = new Vector2(3000, 400);
        headlineTextRectTransform.localScale = new Vector3(0.1f, 0.1f, 1);
        headlineTextRectTransform.localPosition = new Vector3(-35, 120, 0);
        Text text = headlineText.AddComponent<Text>();
        text.text = visitSetting.title;
        text.fontSize = 200;
        text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        headlineText.transform.SetParent(panel.transform, false);

        m_MainMenuPanels[index] = panel;
    }

    private void generateAbout(int index, Vector3 position, Transform parent)
    {
        GameObject panel = new GameObject("About Panel");
        panel.AddComponent<CanvasRenderer>();
        RectTransform panelRectTransform = panel.AddComponent<RectTransform>();
        panelRectTransform.sizeDelta = new Vector2(400, 300);
        panelRectTransform.localPosition = position;
        Image i = panel.AddComponent<Image>();
        i.color = new Color32(255, 255, 255, 157);
        panel.transform.SetParent(parent, false);

        GameObject headlineText = new GameObject("Headline Text");
        RectTransform headlineTextRectTransform = headlineText.AddComponent<RectTransform>();
        headlineTextRectTransform.sizeDelta = new Vector2(3000, 500);
        headlineTextRectTransform.localScale = new Vector3(0.1f, 0.1f, 1);
        headlineTextRectTransform.localPosition = new Vector3(-35, 120, 0);
        Text text = headlineText.AddComponent<Text>();
        text.text = "About bluehands";
        text.fontSize = 400;
        text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        headlineText.transform.SetParent(panel.transform, false);

        GameObject descriptionText = new GameObject("Info Text");
        RectTransform descriptionTextRectTransform = descriptionText.AddComponent<RectTransform>();
        descriptionTextRectTransform.sizeDelta = new Vector2(5000, 3000);
        descriptionTextRectTransform.localScale = new Vector3(0.1f, 0.1f, 1);
        descriptionTextRectTransform.localPosition = new Vector3(70, -50, 0);
        Text textDescription = descriptionText.AddComponent<Text>();

        textDescription.text = "//_softwareentwicklung()\n\n\t\t\t\t\"WIR BAUEN\n\t\t\tIHRE SOFTWARE\"\n\nfirma:\tbluehands GmbH &\n\t\t\tCo.mmunication KG\nmail:\t\tinfo@bluehands.de\nweb:\t\tbluehands.de";

        textDescription.fontSize = 200;
        textDescription.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        descriptionText.transform.SetParent(panel.transform, false);

        m_MainMenuPanels[index] = panel;
    }

    public void DoButtonAction(Type clazz)
    {
        if(clazz.Equals(typeof(ButtonGo)))
        {
        }
        if(clazz.Equals(typeof(ButtonPageLeft)))
        {
            moveMainMenuToLeft();
        }
        if (clazz.Equals(typeof(ButtonPageRight)))
        {
            moveMainMenuToRight();
        }
    }
}
