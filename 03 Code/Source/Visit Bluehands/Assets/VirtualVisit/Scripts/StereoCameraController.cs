using UnityEngine;

public class StereoCameraController : MonoBehaviour {

    private void Update()
    {
        Transform leftCamera = Camera.main.transform.Find("MainCamera Left");
        if (leftCamera != null)
        {
            Camera left = leftCamera.GetComponent<Camera>();
            if (left != null)
            {
                left.cullingMask &= -1 ^ (1 << LayerMask.NameToLayer("Right Eye"));
            }
        }

        Transform rightCamera = Camera.main.transform.Find("MainCamera Right");
        if (rightCamera != null)
        {
            Camera right = rightCamera.GetComponent<Camera>();
            if (right != null)
            {
                right.cullingMask &= -1 ^ (1 << LayerMask.NameToLayer("Left Eye"));
            }
        }
    }
}
