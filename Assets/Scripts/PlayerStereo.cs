using UnityEngine;
using System.Collections;
using System;

public class PlayerStereo : MonoBehaviour, ButtonListener {

    public Cardboard cardboardPrefab;

    public ButtonMenu menuButtonPrefab;

    public ButtonMap mapButtonPrefab;

    public Button3D d3ButtonPrefab;

    public UIScreen uiScreenPrefabs;

    private Cardboard m_Cardboard;

    private ButtonMenu m_MenuButton;

    private ButtonMap m_MapButton;

    private Button3D m_3DButton;

    private UIScreen m_UIScreen;

    private VisitStereo m_Visit;

    public void Initialize(VisitStereo visit)
    {
        m_Visit = visit;

        m_Cardboard = Instantiate(cardboardPrefab) as Cardboard;
        m_Cardboard.transform.parent = transform;

        CardboardHead cardboardHead = m_Cardboard.GetComponentInChildren<CardboardHead>();
        m_Visit.map.transform.parent = cardboardHead.transform;

        m_UIScreen = Instantiate(uiScreenPrefabs) as UIScreen;
        m_UIScreen.transform.parent = transform;

        m_MenuButton = Instantiate(menuButtonPrefab) as ButtonMenu;
        m_MenuButton.Initialize(GameObject.Find("Canvas Menu").transform, this);
        m_MenuButton.setSelected(!m_UIScreen.IsShrinked());
        m_Visit.SetVisitEdgeVisibility(m_UIScreen.IsShrinked());

        m_MapButton = Instantiate(mapButtonPrefab) as ButtonMap;
        m_MapButton.Initialize(GameObject.Find("Canvas Map").transform, this);
        m_MapButton.setSelected(m_Visit.map.GetVisibility());

        m_3DButton = Instantiate(d3ButtonPrefab) as Button3D;
        m_3DButton.Initialize(GameObject.Find("Canvas 3D").transform, this);
        m_3DButton.setSelected(m_Visit.getCurrentVisitNode().IsStereoView());
    }

    public void doAction(Type clazz)
    {
        if (clazz.Equals(typeof(ButtonMenu)))
        {
            bool isShrinked = m_UIScreen.toggleShrinkingForce();
            m_MenuButton.setSelected(!isShrinked);
            m_Visit.SetVisitEdgeVisibility(isShrinked);
        }
        if (clazz.Equals(typeof(ButtonMap)))
        {
            bool visibility = m_Visit.map.ToggleVisibility();
            m_MapButton.setSelected(visibility);
            
        }
        if(clazz.Equals(typeof(Button3D)))
        {
            bool isStereo = m_Visit.getCurrentVisitNode().ToggleStereoView();
            m_3DButton.setSelected(isStereo);
        }
    }
}
