using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class ButtonBase : MonoBehaviour, ISelectHandler, IPointerEnterHandler, IPointerExitHandler
{
    private bool selected;

    private Text buttonText;

    private int waitingTimeInSeconds = 2;

    private int waitingCounter;

    private float timeCounter = 0;

    void Start()
    {
        buttonText = GetComponentInChildren<Text>();

        waitingCounter = waitingTimeInSeconds;
    }

    void Update()
    {
        timeCounter += Time.deltaTime;
        if (timeCounter >= 1)
        {
            timeCounter = 0;
            if (selected)
            {
                if (waitingCounter > 0)
                {
                    waitingCounter--;
                }
                else
                {
                    DoAction();
                }
            }
        }
        buttonText.text = "" + waitingCounter;
    }

    public void OnSelect(BaseEventData eventData)
    {
        DoAction();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        selected = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        selected = false;
        waitingCounter = waitingTimeInSeconds;
    }

    protected abstract void DoAction();
}
