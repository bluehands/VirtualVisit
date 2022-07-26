﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class PanelInfo : MonoBehaviour, ButtonListener {

    public ButtonInfoBack buttonInfoBackPrefab;

    private ButtonInfoBack m_ButtonInfoBack;

    private Stage m_Stage;

    public void Initialize(Vector3 position, VisitSetting visitSetting, Stage stage, Transform parent)
    {
        m_Stage = stage;
        transform.SetParent(parent, false);
        transform.name = "Info Panel " + visitSetting.id;

        RectTransform panelRectTransform = GetComponent<RectTransform>();
        panelRectTransform.localPosition = position;
        panelRectTransform.localScale = new Vector3(1, 1, 1);

        m_ButtonInfoBack = Instantiate(buttonInfoBackPrefab) as ButtonInfoBack;
        m_ButtonInfoBack.Initialize(transform, this);
        m_ButtonInfoBack.GetComponent<RectTransform>().localPosition = new Vector3(-130, -80, 0);

        Text text = transform.FindChild("Headline Text").GetComponent<Text>();
        text.text = "Info " + visitSetting.title;

        string infoText = String.Format("{0}\n\n\t\t\tErstellt \n\t\t\t\t von: {1} \n\t\t\t\t  am: {2} \n\t\t\t\t Blickpunkte: {3}", visitSetting.decription, visitSetting.author, visitSetting.created, visitSetting.nodeSettings.Length);
        Text textInfo = transform.FindChild("Info Text").GetComponent<Text>();
        textInfo.text = infoText;
    }

    void Start () {
	
	}

	void Update () {
	
	}

    public void DoButtonAction(Type clazz)
    {
        if (clazz.Equals(typeof(ButtonInfoBack)))
        {
            m_Stage.HideInfo();
            m_Stage.ShowMainMenu();
        }
    }
}
