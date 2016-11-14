using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public abstract class ButtonPageBase: MonoBehaviour, ISelectHandler, IPointerEnterHandler, IPointerExitHandler
{
    private bool isSelected;

    private Type m_Class;

    private ButtonListener m_buttonListener;

    private const float m_WaitingTime = 1.0f;

    private float m_TimeCounter = 0;

    public void Initialize(Type clazz, Transform parent, ButtonListener buttonListener)
    {
        m_Class = clazz;

        m_buttonListener = buttonListener;
        transform.SetParent(parent, false);
    }

    void Start()
    {
    }

    void Update()
    {
        if(isSelected)
        {
            if(m_TimeCounter == 0)
            {
                InformListener();
            }
            m_TimeCounter += Time.deltaTime;
            if (m_TimeCounter >= m_WaitingTime)
            {
                m_TimeCounter = 0;
            }  
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
    }

    private void InformListener()
    {
        if(m_buttonListener != null)
        {
            m_buttonListener.DoButtonAction(m_Class);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isSelected = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isSelected = false;
    }
}
