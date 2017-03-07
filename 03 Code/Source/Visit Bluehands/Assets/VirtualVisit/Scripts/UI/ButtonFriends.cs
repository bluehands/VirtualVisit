using UnityEngine;

public class ButtonFriends : ButtonBase
{
    public void Initialize(Transform parent, ButtonListener listener)
    {
        Initialize(typeof(ButtonFriends), parent, listener);
    }
}
