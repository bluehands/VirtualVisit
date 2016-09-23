using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public abstract class ButtonPageBase: MonoBehaviour, ISelectHandler, IPointerEnterHandler, IPointerExitHandler
{
    private bool selected;

    private Type m_Class;

    private ButtonListener m_buttonListener;

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
        if(selected)
        {
            InformListener();
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
    }

    private void InformListener()
    {
        if(m_buttonListener != null)
        {
            m_buttonListener.doAction(m_Class);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        selected = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        selected = false;
    }
}
