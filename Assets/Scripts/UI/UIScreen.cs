using UnityEngine;
using UnityEngine.EventSystems;

public class UIScreen : MonoBehaviour {

    public Camera uiCamera;

    private Camera mainCamera;

    private Vector3[] initVerts;

    private bool shrinking = false;
    private bool exspanding = false;

    private bool isShrinked = false;

    private GameObject lastPointedTarget;
    private GameObject currentPointedTarget;

    void Start () {
        initVerts = GetComponent<MeshFilter>().mesh.vertices;
        mainCamera = Camera.main;

        //shrinking = true;
        //isShrinked = true;
        //gameObject.SetActive(false);
    }
	
	void Update () {
        if(shrinking)
        {
            shrinking = srinkDisplay();
            if(shrinking == false)
            {
                gameObject.SetActive(false);
            }
            exspanding = false;
        }
        if(exspanding)
        {
            exspanding = exspandDisplay();
            if(exspanding == false)
            {
            }
        }
        if (shrinking == false && exspanding == false && isShrinked == false)
        {
            traceCourser();
        }
	}

    internal void ToggleShrinking()
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

    internal bool IsShrinked()
    {
        return isShrinked;
    }

    internal bool ToggleShrinkingForce()
    {
        if (isShrinked)
        {
            gameObject.SetActive(true);
            exspanding = true;
            isShrinked = false;
        }
        else
        {
            shrinking = true;
            isShrinked = true;
        }
        return isShrinked;
    }

    private bool srinkDisplay()
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

    private bool exspandDisplay()
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
                currentPointedTarget = null;

                RaycastHit hitDis;
                Vector3 rayDirection = new Vector3(hit.textureCoord.x * uiCamera.pixelWidth, hit.textureCoord.y * uiCamera.pixelHeight, 0);
                Ray rayDis = uiCamera.ScreenPointToRay(rayDirection);
                if (Physics.Raycast(rayDis, out hitDis))
                {

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
            if (currentPointedTarget != null)
            {
                Debug.Log("Pointed Enter " + currentPointedTarget.name);
                ExecuteEvents.Execute(currentPointedTarget, pt, ExecuteEvents.pointerEnterHandler);
            }
            if (lastPointedTarget != null)
            {
                Debug.Log("Pointed Exet " + lastPointedTarget.name);
                ExecuteEvents.Execute(lastPointedTarget, pt, ExecuteEvents.pointerExitHandler);
            }
        }
        lastPointedTarget = currentPointedTarget;
    }
}
