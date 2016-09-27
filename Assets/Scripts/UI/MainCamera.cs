using UnityEngine;

public class MainCamera : MonoBehaviour {

    private bool isInit;
	
	void Update () {
	    if(!isInit)
        {
            setCullingMask();
        }
	}

    private void setCullingMask()
    {
        GvrEye[] eyes = GetComponentsInChildren<GvrEye>();
        if(eyes.Length == 0)
        {
            return;
        }
        foreach (var eye in eyes)
        {
            if (eye.eye == GvrViewer.Eye.Left)
            {
                eye.cam.cullingMask ^= 1 << LayerMask.NameToLayer("Right Eye");
            }
            if (eye.eye == GvrViewer.Eye.Right)
            {
                eye.cam.cullingMask ^= 1 << LayerMask.NameToLayer("Left Eye");
            }
        }
        isInit = true;
    }
}
