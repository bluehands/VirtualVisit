using UnityEngine;

public class FollowingDisplayAbout : MonoBehaviour
{
    internal void Initialize(Transform parent, float yaw, float pitch)
    {
        transform.SetParent(parent, false);
        transform.rotation = Quaternion.Euler(pitch, yaw, 0);
    }
}
