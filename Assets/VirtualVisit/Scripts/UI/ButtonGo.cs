using UnityEngine;

public class ButtonGo : ButtonBase
{
    public void Initialize(Transform parent, ButtonListener listener)
    {
        Initialize(typeof(ButtonGo), parent, listener);
    }
}
