using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class UICamera : MonoBehaviour {

    public ButtonStart buttonStartPrefebs;

	void Start () {
    }

    public void Generate()
    {
        initVisitPanel("Epple", "Epple_P0", new Vector3(200, 0, 0));
        initVisitPanel("UStrab", "UStrab_P0", new Vector3(-200, 0, 0));
        initVisitPanel("Dougeon", "Dungeon_P1", new Vector3(-600, 0, 0));
        initVisitPanel("Bluehands", "Bluehands_P0", new Vector3(600, 0, 0));
    }

    private void initVisitPanel(string visitId, string startImage, Vector3 position)
    {
        Canvas canvase = this.transform.GetComponentInChildren<Canvas>();

        GameObject panel = new GameObject("Panel " + visitId);
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
        rawImage.texture = Resources.Load(String.Format("Panoramas\\{0}", startImage)) as Texture;
        backgroundImage.transform.SetParent(panel.transform, false);

        ButtonStart buttonStart = Instantiate(buttonStartPrefebs) as ButtonStart;
        RectTransform buttonStartRectTransform = buttonStart.GetComponent<RectTransform>();
        buttonStartRectTransform.localPosition = new Vector3(130, -80, 0);
        buttonStart.nextVisitId = visitId;
        buttonStart.transform.SetParent(panel.transform, false);

        GameObject headlineText = new GameObject("Headline Text");
        RectTransform headlineTextRectTransform = headlineText.AddComponent<RectTransform>();
        headlineTextRectTransform.sizeDelta = new Vector2(3000, 400);
        headlineTextRectTransform.localScale = new Vector3(0.1f, 0.1f, 1);
        headlineTextRectTransform.localPosition = new Vector3(-35, 120, 0);
        Text text = headlineText.AddComponent<Text>();
        text.text = visitId;
        text.fontSize = 200;
        text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        headlineText.transform.SetParent(panel.transform, false);

        GameObject descriptionText = new GameObject("Info Text");
        RectTransform descriptionTextRectTransform = descriptionText.AddComponent<RectTransform>();
        descriptionTextRectTransform.sizeDelta = new Vector2(3000, 500);
        descriptionTextRectTransform.localScale = new Vector3(0.1f, 0.1f, 1);
        descriptionTextRectTransform.localPosition = new Vector3(0, -60, 0);
        Text textDescription = descriptionText.AddComponent<Text>();
        textDescription.text = "Erstellt \n von: Marcel Weigel \n am: 24.7.2016";
        textDescription.fontSize = 140;
        textDescription.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        descriptionText.transform.SetParent(panel.transform, false);
    }

	void Update () {
	
	}
}
