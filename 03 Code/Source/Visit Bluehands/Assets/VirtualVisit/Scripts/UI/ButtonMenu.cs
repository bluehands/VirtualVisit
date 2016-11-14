using UnityEngine;

public class ButtonMenu : ButtonBase
{
    public void Initialize(Transform parent, ButtonListener buttonListener)
    {
        Initialize(typeof(ButtonMenu), parent, buttonListener);
    }
}
