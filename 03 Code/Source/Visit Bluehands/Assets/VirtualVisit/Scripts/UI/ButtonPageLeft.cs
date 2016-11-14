using UnityEngine;

public class ButtonPageLeft: ButtonPageBase
{
    public void Initialize(Transform parent, ButtonListener buttonListener)
    {
        Initialize(typeof(ButtonPageLeft), parent, buttonListener);
    }
}
