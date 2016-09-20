using System;
using UnityEngine.SceneManagement;

public class ButtonBackStereo : ButtonBase
{
    private UIScreen uiScreen;

    protected override void DoAction()
    {
        uiScreen.toggleShrinkingForce();
    }

    internal void Initialize(UIScreen uiScreenInstance)
    {
        uiScreen = uiScreenInstance;
    }
}
