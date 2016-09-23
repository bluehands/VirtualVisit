using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class UICamera : MonoBehaviour, ButtonListener {

    public ButtonGo buttonGoPrefebs;

    public ButtonInfo buttonInfoPrefebs;

    public ButtonPageLeft buttonPageLeftPrefebs;

    public ButtonPageRight buttonPageRightPrefebs;

    private VisitSettingsFactory visitSettingsFactory;

    private GameObject[] mainMenuPanels;

    private GameObject[] infoMenuPanels;

    private GameObject canvaseMenu;

    public void Generate(VisitSettingsFactory visitSettingsFactory)
    {
        this.visitSettingsFactory = visitSettingsFactory;

        canvaseMenu = GameObject.Find("Menu Canvas");
        var canvasePage = GameObject.Find("Page Canvas");
        generatePage(canvasePage.transform);
        generateMainMenu(canvaseMenu.transform);
        showMainMenu();
        generateInfoMenus(canvaseMenu.transform);
        hideInfo();
    }

    public void showInfo(int infoIndex)
    {
        GameObject visitInfoPanel = infoMenuPanels[infoIndex];
        visitInfoPanel.SetActive(true);
    }

    public void hideInfo()
    {
        if(infoMenuPanels != null)
        {
            foreach (var panel in infoMenuPanels)
            {
                panel.SetActive(false);
            }
        }
    }


    public void showMainMenu()
    {
        if (mainMenuPanels != null)
        {
            foreach (var panel in mainMenuPanels)
            {
                panel.SetActive(true);
            }
        }
    }

    public void hideMainMenu()
    {
        if (mainMenuPanels != null)
        {
            foreach (var panel in mainMenuPanels)
            {
                panel.SetActive(false);
            }
        }
    }

    private void generateInfoMenus(Transform parent)
    {
        var visitSettings = visitSettingsFactory.getVisitSettings();
        infoMenuPanels = new GameObject[visitSettings.Length];

        for (int i = 0; i < visitSettings.Length; i++)
        {
            generateInfoMenu(i, parent);
        }
    }

    private void moveMainMenuToLeft()
    {
        RectTransform panelRectTransform = canvaseMenu.GetComponent<RectTransform>();
        var position = panelRectTransform.localPosition;
        position.x -= 0.01f;
        panelRectTransform.localPosition = position;
    }

    private void moveMainMenuToRight()
    {
        RectTransform panelRectTransform = canvaseMenu.GetComponent<RectTransform>();
        var position = panelRectTransform.localPosition;
        position.x += 0.01f;
        panelRectTransform.localPosition = position;
    }

    private void generateInfoMenu(int index, Transform parent)
    {
        VisitSettingsStereo visitSetting = visitSettingsFactory.getVisitSettings()[index];
        GameObject mainMenu = mainMenuPanels[index];

        GameObject panel = new GameObject("InfoPanel " + visitSetting.id);
        panel.AddComponent<CanvasRenderer>();
        RectTransform panelRectTransform = panel.AddComponent<RectTransform>();
        panelRectTransform.sizeDelta = new Vector2(400, 300);
        panelRectTransform.localPosition = mainMenu.GetComponent<RectTransform>().localPosition;
        Image image = panel.AddComponent<Image>();
        image.color = new Color32(255, 255, 255, 157);
        panel.transform.SetParent(parent, false);

        GameObject backgroundImage = new GameObject("Background Image");
        RectTransform panelRectTransformImage = backgroundImage.AddComponent<RectTransform>();
        panelRectTransformImage.sizeDelta = new Vector2(300, 200);
        RawImage rawImage = backgroundImage.AddComponent<RawImage>();

        Texture textureLeft = null;
        Texture textureRight = null;

        visitSettingsFactory.tryToLoadTextures(visitSetting, visitSetting.getPreviewNodeSetting(), out textureLeft, out textureRight);

        rawImage.texture = textureLeft;
        backgroundImage.transform.SetParent(panel.transform, false);

        ButtonInfo buttonInfo = Instantiate(buttonInfoPrefebs) as ButtonInfo;
        buttonInfo.Initialize(-1, this, panel.transform);
        buttonInfo.GetComponent<RectTransform>().localPosition = new Vector3(-130, -80, 0);

        GameObject headlineText = new GameObject("Headline Text");
        RectTransform headlineTextRectTransform = headlineText.AddComponent<RectTransform>();
        headlineTextRectTransform.sizeDelta = new Vector2(3000, 400);
        headlineTextRectTransform.localScale = new Vector3(0.1f, 0.1f, 1);
        headlineTextRectTransform.localPosition = new Vector3(-35, 120, 0);
        Text text = headlineText.AddComponent<Text>();
        text.text = visitSetting.id;
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

        infoMenuPanels[index] = panel;
    }

    private void generateMainMenu(Transform parent)
    {
        var visitSettings = visitSettingsFactory.getVisitSettings();

        mainMenuPanels = new GameObject[visitSettings.Length];

        //var xStep = 1680 / visitSettings.Length;
        var xStep = 400;
        //var currentStep = -(1680 / 2) + 200;
        var currentStep = -600;
        for (int i=0; i< visitSettings.Length; i++)
        {
            generateMenu(i, new Vector3(currentStep, 0, 0), parent);
            currentStep += xStep;
        }  
    }

    private void generatePage(Transform parent)
    {
        ButtonPageLeft buttonPageLeft = Instantiate(buttonPageLeftPrefebs) as ButtonPageLeft;
        buttonPageLeft.Initialize(parent.transform, this);
        buttonPageLeft.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(-800, 0, 0);

        ButtonPageRight buttonPageRight = Instantiate(buttonPageRightPrefebs) as ButtonPageRight;
        buttonPageRight.Initialize(parent.transform, this);
        buttonPageRight.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(800, 0, 0);
    }

    private void generateMenu(int index, Vector3 position, Transform parent)
    {
        VisitSettingsStereo visitSetting = visitSettingsFactory.getVisitSettings()[index];

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

        visitSettingsFactory.tryToLoadTextures(visitSetting, visitSetting.getPreviewNodeSetting(), out textureLeft, out textureRight);

        rawImage.texture = textureLeft;
        backgroundImage.transform.SetParent(panel.transform, false);

        ButtonGo buttonGo = Instantiate(buttonGoPrefebs) as ButtonGo;
        buttonGo.Initialize(visitSetting.id, panel.transform);
        buttonGo.GetComponent<RectTransform>().localPosition = new Vector3(130, -80, 0);

        ButtonInfo buttonInfo = Instantiate(buttonInfoPrefebs) as ButtonInfo;
        buttonInfo.Initialize(index, this, panel.transform);
        buttonInfo.GetComponent<RectTransform>().localPosition = new Vector3(-130, -80, 0);

        GameObject headlineText = new GameObject("Headline Text");
        RectTransform headlineTextRectTransform = headlineText.AddComponent<RectTransform>();
        headlineTextRectTransform.sizeDelta = new Vector2(3000, 400);
        headlineTextRectTransform.localScale = new Vector3(0.1f, 0.1f, 1);
        headlineTextRectTransform.localPosition = new Vector3(-35, 120, 0);
        Text text = headlineText.AddComponent<Text>();
        text.text = visitSetting.id;
        text.fontSize = 200;
        text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        headlineText.transform.SetParent(panel.transform, false);

        mainMenuPanels[index] = panel;
    }

    public void doAction(Type clazz)
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
