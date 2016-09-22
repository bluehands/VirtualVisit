using UnityEngine;

public class Button3D : ButtonBase
{
    public void Initialize(Transform parent, ButtonListener buttonListener)
    {
        Initialize(typeof(Button3D), parent, buttonListener);
    }

    protected override void DoAction()
    {
    }
}
