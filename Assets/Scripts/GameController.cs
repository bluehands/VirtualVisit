using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class GameController : MonoBehaviour {

    public Camera mainCamera;
    public Camera displayCamera;
    public GameObject pointer;
    public GameObject pointerScreen;
    private float speed = 2.0f;
    

    private int screenshotCounter = 0;

    private GameObject lastPointedTarget = null;
    private GameObject currentPointedTarget = null;

    void Start () {
    }

	void Update () {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            print("Save screenshot in " + Application.persistentDataPath + "Screenshot_" + screenshotCounter + ".png");
            Application.CaptureScreenshot("Screenshot_" + screenshotCounter + ".png");
            screenshotCounter++;
        }
        /*
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetMouseButtonDown(0))
        {
            traceCourser();
        }
        traceCourser();*/
    }

    private void traceCourser()
    {
        RaycastHit hit;
        //Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        var ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "Display")
            {
                currentPointedTarget = null;

                //pointer.transform.position = hit.point;

                RaycastHit hitDis;
                Vector3 rayDirection = new Vector3(hit.textureCoord.x * displayCamera.pixelWidth, hit.textureCoord.y * displayCamera.pixelHeight, 0);
                Ray rayDis = displayCamera.ScreenPointToRay(rayDirection);
                if (Physics.Raycast(rayDis, out hitDis))
                {
                    //pointerScreen.transform.position = hitDis.point;

                    currentPointedTarget = hitDis.transform.gameObject;
                }
            }
        }

        checkPointedAt();
    }

    private void checkPointedAt()
    {
        var pt = new PointerEventData(EventSystem.current);
        if (lastPointedTarget != currentPointedTarget)
        {
            if(currentPointedTarget != null)
            {
                print("Pointed Enter " + currentPointedTarget.name);
                ExecuteEvents.Execute(currentPointedTarget, pt, ExecuteEvents.pointerEnterHandler);
            }
            if (lastPointedTarget != null)
            {
                print("Pointed Exet " + lastPointedTarget.name);
                ExecuteEvents.Execute(lastPointedTarget, pt, ExecuteEvents.pointerExitHandler);
            }
        }
        lastPointedTarget = currentPointedTarget;
    }
}
