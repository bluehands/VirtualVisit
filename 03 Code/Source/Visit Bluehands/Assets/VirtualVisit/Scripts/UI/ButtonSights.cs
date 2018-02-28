using UnityEngine;

public class ButtonSights : ButtonBase
{
    public void Initialize(Transform parent, ButtonListener listener)
    {
        Initialize(typeof(ButtonSights), parent, listener);
    }
}
