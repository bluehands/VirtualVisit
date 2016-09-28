using UnityEngine;
using System;

public class Player : MonoBehaviour, ButtonListener, VisitNodeChangeListener {

    public ButtonMenu buttonMenuPrefab;

    public ButtonMap buttonMapPrefab;

    public Button3D dutton3DPrefab;

    public UIScreen uiScreenPrefab;

    private ButtonMenu m_MenuButton;

    private ButtonMap m_MapButton;

    private Button3D m_3DButton;

    private UIScreen m_UIScreen;

    private Visit m_Visit;

    public void Initialize(Visit visit)
    {
        m_Visit = visit;

        m_Visit.Map.transform.parent = Camera.main.transform;

        m_UIScreen = Instantiate(uiScreenPrefab) as UIScreen;
        m_UIScreen.transform.parent = transform;

        m_MenuButton = Instantiate(buttonMenuPrefab) as ButtonMenu;
        m_MenuButton.Initialize(GameObject.Find("Canvas Menu").transform, this);
        m_MenuButton.setSelected(!m_UIScreen.IsShrinked());
        m_Visit.SetVisitEdgeVisibility(m_UIScreen.IsShrinked());

        m_MapButton = Instantiate(buttonMapPrefab) as ButtonMap;
        m_MapButton.Initialize(GameObject.Find("Canvas Map").transform, this);
        m_MapButton.setSelected(m_Visit.Map.GetVisibility());

        m_3DButton = Instantiate(dutton3DPrefab) as Button3D;
        m_3DButton.Initialize(GameObject.Find("Canvas 3D").transform, this);
        m_3DButton.setSelected(m_Visit.CurrentVisitNode.IsStereoView());

        toggleMainMenu();
    }

    public void DoButtonAction(Type clazz)
    {
        if (clazz.Equals(typeof(ButtonMenu)))
        {
            toggleMainMenu();
        }
        if (clazz.Equals(typeof(ButtonMap)))
        {
            bool visibility = m_Visit.Map.ToggleVisibility();
            m_MapButton.setSelected(visibility);
            
        }
        if(clazz.Equals(typeof(Button3D)))
        {
            bool isStereo = m_Visit.CurrentVisitNode.ToggleStereoView();
            m_3DButton.setSelected(isStereo);
        }
    }

    private void toggleMainMenu()
    {
        m_UIScreen.ToggleShrinking();
        bool isShrinked = m_UIScreen.IsShrinked();
        m_MenuButton.setSelected(!isShrinked);
        m_Visit.SetVisitEdgeVisibility(isShrinked);
    }

    public void IsChangedTo(VisitNode toNode)
    {
        m_3DButton.SetInteractable(toNode.IsStereo);
    }
}
