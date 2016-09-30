using UnityEngine;
using UnityEngine.UI;
using System;

public class Stage : MonoBehaviour {

    public float StepWidth = 400;

    public float MenuPositionOffset = 600;

    public float TotalMenuWidth { get; private set; }

    public float CurrentMenuPosition { get; private set; }

    public PanelMenuItem panelMenuItemPrefab;

    public PanelAbout panelAboutPrefab;

    public PanelInfo panelInfoPrefab;

    public PanelPaging panelPagingPrefab;

    private VisitSettingsFactory m_VisitSettingsFactory;

    private PanelMenuItem[] m_PanelMenuItems;

    private PanelAbout m_PanelAbout;

    private PanelInfo[] m_PanelInfos;

    private PanelPaging m_PanelPaging;

    private Transform m_CanvaseMenu;

    public void Generate(VisitSettingsFactory visitSettingsFactory)
    {
        m_VisitSettingsFactory = visitSettingsFactory;
        
        m_CanvaseMenu = transform.FindChild("Menu Canvas");
        var canvasePage = transform.FindChild("Page Canvas");

        generatePage(canvasePage.transform);
        generateMainMenu(m_CanvaseMenu.transform);
        generateInfoMenus(m_CanvaseMenu.transform);
        ShowMainMenu();
        HideInfo();
    }

    public void ShowInfo(int infoIndex)
    {
        PanelInfo panelInfo = m_PanelInfos[infoIndex];
        panelInfo.gameObject.SetActive(true);
    }

    public void HideInfo()
    {
        foreach (var panel in m_PanelInfos)
        {
            panel.gameObject.SetActive(false);
        }
    }

    public void ShowMainMenu()
    {
        setMainMenuActive(true);
    }

    public void HideMainMenu()
    {
        setMainMenuActive(false);
    }

    private void setMainMenuActive(bool isActive)
    {
        foreach (var panel in m_PanelMenuItems)
        {
            panel.gameObject.SetActive(isActive);
        }
        m_PanelAbout.gameObject.SetActive(isActive);
    }

    private void generateInfoMenus(Transform parent)
    {
        var visitSettings = m_VisitSettingsFactory.GetVisitSettings();
        m_PanelInfos = new PanelInfo[visitSettings.Length];

        for (int i = 0; i < visitSettings.Length; i++)
        {
            VisitSettings visitSetting = m_VisitSettingsFactory.GetVisitSettings()[i];
            PanelMenuItem panelMenuItem = m_PanelMenuItems[i];

            PanelInfo panelInfo = Instantiate(panelInfoPrefab) as PanelInfo;
            panelInfo.Initialize(panelMenuItem.transform.localPosition, visitSetting, this, parent);
            m_PanelInfos[i]= panelInfo;
        }
    }

    internal void moveMainMenuToLeft()
    {
        moveMainMenu(-8f * 4);
    }

    internal void moveMainMenuToRight()
    {
        moveMainMenu(8f * 4);
    }

    private void moveMainMenu(float moveDicretion)
    {
        RectTransform panelRectTransform = m_CanvaseMenu.GetComponent<RectTransform>();
        var position = panelRectTransform.localPosition;
        position.x += moveDicretion;
        panelRectTransform.localPosition = position;
        CurrentMenuPosition += moveDicretion;
    }

    private void generateMainMenu(Transform parent)
    {
        var visitSettings = m_VisitSettingsFactory.GetVisitSettings();

        m_PanelMenuItems = new PanelMenuItem[visitSettings.Length];

        var currentStep = 0f;
        for (int i=0; i< visitSettings.Length; i++)
        {
            VisitSettings visitSetting = m_VisitSettingsFactory.GetVisitSettings()[i];
            Vector3 position = new Vector3(currentStep, 0, 0);
            PanelMenuItem panelMenuItem = Instantiate(panelMenuItemPrefab) as PanelMenuItem;
            panelMenuItem.Initialize(position, visitSetting, this, i, m_VisitSettingsFactory, parent);
            m_PanelMenuItems[i] = panelMenuItem;
            currentStep += StepWidth;
        }

        m_PanelAbout = Instantiate(panelAboutPrefab) as PanelAbout;
        m_PanelAbout.Initialize(new Vector3(currentStep, 0, 0), parent);

        TotalMenuWidth = currentStep + StepWidth;
        moveMainMenu(-MenuPositionOffset);
    }

    private void generatePage(Transform parent)
    {
        m_PanelPaging = Instantiate(panelPagingPrefab) as PanelPaging;
        m_PanelPaging.Initialize(this, parent);
    }
}
