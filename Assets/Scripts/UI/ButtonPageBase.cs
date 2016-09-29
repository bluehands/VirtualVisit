using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public abstract class ButtonPageBase: MonoBehaviour, ISelectHandler, IPointerEnterHandler, IPointerExitHandler
{
    private bool isSelected;

    private Type m_Class;

    private ButtonListener m_buttonListener;

    private float timeCounter = 0;

    private bool isMoving;

    private int movingCounter;
    private const int movingSteps = 24;

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
        if(isMoving)
        {
            if(movingCounter > 0)
            {
                InformListener();
                movingCounter--;
            } else
            {
                isMoving = false;
                //isSelected = false;
                movingCounter = movingSteps;
            } 
        } else if(isSelected)
        {
            timeCounter += Time.deltaTime;
            if (timeCounter >= 0.5)
            {
                isMoving = true;
                timeCounter = 0;
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
