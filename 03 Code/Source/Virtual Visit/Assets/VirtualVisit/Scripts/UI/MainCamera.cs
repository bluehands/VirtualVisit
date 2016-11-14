using UnityEngine;

public class MainCamera : MonoBehaviour {

    private bool isInit;
	
	void Update () {
	    if(!isInit)
        {
           setCullingMask();
        }
	}

    void OnApplicationFocus(bool hasFocus)
    {
        setCullingMask();
    }

    private void setCullingMask()
    {
        Debug.Log("setCullingMask");
        GvrEye[] eyes = GetComponentsInChildren<GvrEye>();
        if(eyes.Length == 0)
        { 
            return;
        }
        foreach (var eye in eyes)
        {
            if (eye.eye == GvrViewer.Eye.Left)
            {
                eye.cam.cullingMask &= int.MaxValue ^ (1 << LayerMask.NameToLayer("Right Eye"));
            }
            if (eye.eye == GvrViewer.Eye.Right)
            {
                eye.cam.cullingMask &= int.MaxValue ^ (1 << LayerMask.NameToLayer("Left Eye"));
            }
        }
        isInit = true;
    }
}
