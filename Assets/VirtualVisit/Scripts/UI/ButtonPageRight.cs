using UnityEngine;

public class ButtonPageRight: ButtonPageBase
{
    public void Initialize(Transform parent, ButtonListener buttonListener)
    {
        Initialize(typeof(ButtonPageRight), parent, buttonListener);
    }
}
