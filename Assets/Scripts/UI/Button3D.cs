using System;
using UnityEngine;
using UnityEngine.UI;

public class Button3D : ButtonBase
{
    public void Initialize(Transform parent, ButtonListener buttonListener)
    {
        Initialize(typeof(Button3D), parent, buttonListener);
    }

    protected override void DoButtonAction()
    {
    }

    internal void SetInteractable(bool isStereo)
    {
        var myButtonScript = GetComponent<Button>();
        myButtonScript.enabled = isStereo;
    }
}
