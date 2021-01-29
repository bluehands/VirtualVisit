using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public abstract class ButtonBase : MonoBehaviour, ISelectHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Type m_Class;

    private bool selected;
    private bool doneAction;

    private Text buttonText;

    private float waitingTimeInSeconds = 1;

    private float waitingCounter;

    private float timeCounter = 0;

    private ButtonListener m_buttonListener;

    protected void Initialize(Type clazz, Transform parent, ButtonListener buttonListener)
    {
        m_Class = clazz;
        m_buttonListener = buttonListener;
        transform.SetParent(parent, false);
        setSelected(false);
    }

    void Start()
    {
        buttonText = GetComponentInChildren<Text>();

        waitingCounter = waitingTimeInSeconds;
    }

    void Update()
    {
        timeCounter += Time.deltaTime;
        if (timeCounter >= 0.1)
        {
            timeCounter = 0;
            if (selected && !doneAction)
            {
                if (waitingCounter > 0)
                {
                    waitingCounter -= 0.1f;
                }
                else
                {
                    doneAction = true;
                    InformListener();
                }
            }
        }
        buttonText.text = "" + ((waitingCounter < 0) ? 0 : (int)(waitingCounter*10));
    }

    public void OnSelect(BaseEventData eventData)
    {
        InformListener();
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
        selected = true;
        doneAction = false;
        waitingCounter = waitingTimeInSeconds;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        selected = false;
        doneAction = false;
        waitingCounter = waitingTimeInSeconds;
    }

    public void setSelected(bool visibility)
    {
        Transform tranform = transform.Find("Selected");
        if(tranform != null)
        {
            if (!visibility)
            {
                tranform.gameObject.SetActive(false);
            }
            else
            {
                tranform.gameObject.SetActive(true);
            }
        }
    }
}
