using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class PanelPaging : MonoBehaviour, ButtonListener {

    public ButtonPageLeft buttonPageLeftPrefab;

    public ButtonPageRight buttonPageRightPrefab;

    private ButtonPageLeft m_ButtonPageLeft;

    private ButtonPageRight m_ButtonPageRight;

    private Stage m_Stage;

    private bool m_IsMovingLeft;
    private bool m_IsMovingRight;

    private int m_MovingCounter;
    private const int m_MovingSteps = 50;

    public void Initialize(Stage stage, Transform parent)
    {
        m_Stage = stage;
        transform.parent = parent;

        RectTransform panelRectTransform = GetComponent<RectTransform>();
        panelRectTransform.localScale = new Vector3(1, 1, 1);


        m_ButtonPageLeft = Instantiate(buttonPageLeftPrefab) as ButtonPageLeft;
        m_ButtonPageLeft.Initialize(parent, this);
        m_ButtonPageLeft.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(750, 0, -1);

        m_ButtonPageRight = Instantiate(buttonPageRightPrefab) as ButtonPageRight;
        m_ButtonPageRight.Initialize(parent, this);
        m_ButtonPageRight.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(-750, 0, -1);
    }

    void Start () {
	
	}

	void Update () {
        checkBoarders();
        if (m_IsMovingLeft || m_IsMovingRight)
        {
            if (m_MovingCounter > 0)
            {
                if(m_IsMovingLeft)
                {
                    m_Stage.moveMainMenuToLeft();
                } else if(m_IsMovingRight)
                {
                    m_Stage.moveMainMenuToRight();
                }
                m_MovingCounter--;
            }
            else
            {
                m_IsMovingLeft = false;
                m_IsMovingRight = false;
                m_MovingCounter = m_MovingSteps;
            }
        }
    }

    private void checkBoarders()
    {
        if(m_Stage.CurrentMenuPosition <= -(m_Stage.TotalMenuWidth + m_Stage.MenuPositionOffset - m_Stage.StepWidth))
        {
            m_IsMovingLeft = false;
        }
        if (m_Stage.CurrentMenuPosition >= m_Stage.MenuPositionOffset)
        {
            m_IsMovingRight = false;
        }
    }

    public void DoButtonAction(Type clazz)
    {
        if (clazz.Equals(typeof(ButtonPageLeft)))
        {
            m_IsMovingLeft = true;
        }
        if (clazz.Equals(typeof(ButtonPageRight)))
        {
            m_IsMovingRight = true;
        }
    }
}
