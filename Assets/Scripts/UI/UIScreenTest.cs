using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class UIScreenTest : MonoBehaviour {

    public Camera mainCamera;
    public Camera displayCamera;

    public VRInputModule vrInputModule;

    private Vector3[] initVerts;

    private bool shrinking = false;
    private bool exspanding = false;

    private bool isShrinked = false;

    private GameObject lastPointedTarget = null;
    private GameObject currentPointedTarget = null;

    // Use this for initialization
    void Start () {
        initVerts = GetComponent<MeshFilter>().mesh.vertices;

        shrinking = true;
        isShrinked = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetMouseButtonDown(0))
        {
            toggleShrinking();

        }
        if (Input.GetKey(KeyCode.P))
        {
            toggleShrinking();
            //SrinkDisplay();
            //shrinking = true;
        }
        if(Input.GetKey(KeyCode.O))
        {
            //ExspandDisplay();
            exspanding = true;
        }
        if(shrinking)
        {
            shrinking = SrinkDisplay();
            if(shrinking == false)
            {
            }
            exspanding = false;
        }
        if(exspanding)
        {
            exspanding = ExspandDisplay();
            if(exspanding == false)
            {
            }
        }
        if (shrinking == false && exspanding == false && isShrinked == false)
        {
            traceCourser();
        }
	}

    private void toggleShrinking()
    {
        if (shrinking == false && exspanding == false)
        {
            if (isShrinked)
            {
                exspanding = true;
                isShrinked = false;
            }
            else
            {
                shrinking = true;
                isShrinked = true;
            }
        }
        
    }

    private bool SrinkDisplay()
    {
        bool isSrink = false;
        var mesh = GetComponent<MeshFilter>().mesh;
        var verts = mesh.vertices;
        for (int i = 0; i < mesh.vertexCount; i++)
        {
            Vector3 dirCurrent = verts[i] - Vector3.back;
            float angleCurrent = Mathf.Atan2(dirCurrent.x, dirCurrent.y) * Mathf.Rad2Deg;

            verts[i] = Quaternion.AngleAxis(angleCurrent / 10, Vector3.forward) * verts[i];
            if(angleCurrent > 0.01)
            {
                isSrink = true;
            }
        }
        mesh.vertices = verts;
        return isSrink;
    }

    private bool ExspandDisplay()
    {
        bool isExspand = false;
        var mesh = GetComponent<MeshFilter>().mesh;
        var verts = mesh.vertices;
        for (int i = 0; i < mesh.vertexCount; i++)
        {
            Vector3 dirCurrent = verts[i] - Vector3.back;
            float angleCurrent = Mathf.Atan2(dirCurrent.x, dirCurrent.y) * Mathf.Rad2Deg;

            Vector3 dirInit = initVerts[i] - Vector3.back;
            float angleInit = Mathf.Atan2(dirInit.x, dirInit.y) * Mathf.Rad2Deg;

            verts[i] = Quaternion.AngleAxis((-(angleInit - angleCurrent) / 10), Vector3.forward) * verts[i];
            if(angleInit - angleCurrent > 0.01)
            {
                isExspand = true;
            }
        }
        mesh.vertices = verts;
        return isExspand;
    }

    private void traceCourser()
    {
        RaycastHit hit;
        var ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "Display")
            {
                //currentPointedTarget = null;

                vrInputModule.UpdateCursorPosition(new Vector2(hit.textureCoord.x * displayCamera.pixelWidth, hit.textureCoord.y * displayCamera.pixelHeight));
                   /*
                RaycastHit hitDis;
                Vector3 rayDirection = new Vector3(hit.textureCoord.x * displayCamera.pixelWidth, hit.textureCoord.y * displayCamera.pixelHeight, 0);
                Ray rayDis = displayCamera.ScreenPointToRay(rayDirection);
                if (Physics.Raycast(rayDis, out hitDis))
                {

                    currentPointedTarget = hitDis.transform.gameObject;
                }*/
            }
        }

        //checkPointedAt();
    }

    private void checkPointedAt()
    {
        var pt = new PointerEventData(EventSystem.current);
        if (lastPointedTarget != currentPointedTarget)
        {
            if (currentPointedTarget != null)
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
