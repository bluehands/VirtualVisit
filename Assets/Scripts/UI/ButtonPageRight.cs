using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ButtonPageRight: ButtonPageBase
{
    public void Initialize(Transform parent, ButtonListener buttonListener)
    {
        Initialize(typeof(ButtonPageRight), parent, buttonListener);
    }
}
