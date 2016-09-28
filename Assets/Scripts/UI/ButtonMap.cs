using UnityEngine;

public class ButtonMap : ButtonBase
{
    public void Initialize(Transform parent, ButtonListener buttonListener)
    {
        Initialize(typeof(ButtonMap), parent, buttonListener);
    }

    protected override void DoButtonAction()
    {
    }
}
