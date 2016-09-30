using UnityEngine;

public class ButtonStep : ButtonBase
{
    public void Initialize(Transform parent, ButtonListener buttonListener)
    {
        Initialize(typeof(ButtonStep), parent, buttonListener);
    }
}
