using UnityEngine;

public class ButtonBackStereo : ButtonBase
{
    private UIScreen uiScreen;

    public void Initialize(UIScreen uiScreen, Transform parent)
    {
        this.uiScreen = uiScreen;
        transform.SetParent(parent.transform, false);
    }

    protected override void DoAction()
    {
        uiScreen.toggleShrinkingForce();
    }

}
