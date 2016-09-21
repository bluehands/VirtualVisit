using UnityEngine;
using UnityEngine.UI;
using System;

public class UICamera : MonoBehaviour {

    public ButtonStart buttonStartPrefebs;

    public void Generate(VisitSettingsFactory visitSettingsFactory)
    {
        var visitSettings = visitSettingsFactory.getVisitSettings();
        var xStep = 1680 / visitSettings.Length;
        var currentStep = -(1680/2) + 200;
        foreach (var visitSetting in visitSettings)
        {
            initVisitPanel(visitSetting, new Vector3(currentStep, 0, 0), visitSettingsFactory);
            currentStep += xStep;
        }
    }

    private void initVisitPanel(VisitSettingsStereo visitSetting, Vector3 position, VisitSettingsFactory visitSettingsFactory)
    {
        Canvas canvase = this.transform.GetComponentInChildren<Canvas>();

        GameObject panel = new GameObject("Panel " + visitSetting.id);
        panel.AddComponent<CanvasRenderer>();
        RectTransform panelRectTransform = panel.AddComponent<RectTransform>();
        panelRectTransform.sizeDelta = new Vector2(400, 300);
        panelRectTransform.localPosition = position;
        Image i = panel.AddComponent<Image>();
        i.color = new Color32(255, 255, 255, 157);
        panel.transform.SetParent(canvase.transform, false);

        GameObject backgroundImage = new GameObject("Background Image");
        RectTransform panelRectTransformImage = backgroundImage.AddComponent<RectTransform>();
        panelRectTransformImage.sizeDelta = new Vector2(300, 200);
        RawImage rawImage = backgroundImage.AddComponent<RawImage>();

        Texture textureLeft = null;
        Texture textureRight = null;

        visitSettingsFactory.tryToLoadTextures(visitSetting, visitSetting.getPreviewNodeSetting(), out textureLeft, out textureRight);

        rawImage.texture = textureLeft;
        backgroundImage.transform.SetParent(panel.transform, false);

        ButtonStart buttonStart = Instantiate(buttonStartPrefebs) as ButtonStart;
        RectTransform buttonStartRectTransform = buttonStart.GetComponent<RectTransform>();
        buttonStartRectTransform.localPosition = new Vector3(130, -80, 0);
        buttonStart.nextVisitId = visitSetting.id;
        buttonStart.transform.SetParent(panel.transform, false);

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
        descriptionTextRectTransform.localPosition = new Vector3(0, -60, 0);
        Text textDescription = descriptionText.AddComponent<Text>();

        textDescription.text = String.Format("Erstellt \n von: {0} \n am: {1}", visitSetting.author, visitSetting.created);

        textDescription.fontSize = 140;
        textDescription.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        descriptionText.transform.SetParent(panel.transform, false);
    }

}
