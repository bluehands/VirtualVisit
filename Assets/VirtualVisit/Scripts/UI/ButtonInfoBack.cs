using UnityEngine;

public class ButtonInfoBack : ButtonBase
{
    public void Initialize(Transform parent, ButtonListener listener)
    {
        Initialize(typeof(ButtonInfoBack), parent, listener);
    }
}
