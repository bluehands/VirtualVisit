using UnityEngine;

public class ButtonInfo : ButtonBase
{
    public void Initialize(Transform parent, ButtonListener listener)
    {
        Initialize(typeof(ButtonInfo), parent, listener);
    }
}
