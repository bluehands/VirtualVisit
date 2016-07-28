using UnityEngine;
using System.Collections;

public class MiniMap : MonoBehaviour {

    private static bool mapVisibility;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    void Start ()
    {
        SetVisibility();
    }

    void Update ()
    {
        transform.LookAt(new Vector3(0, 0, 10000));
    }

    public void ToggleVisibility()
    {
        print("Toggle Visibility from " + mapVisibility + " to " + !mapVisibility);
        mapVisibility = !mapVisibility;
        SetVisibility();
    }

    public void SetVisibility()
    {
        print("Set Visibility " + mapVisibility);
        GetComponent<MeshRenderer>().enabled = mapVisibility;
        foreach (Transform child in transform)
        {
            child.GetComponent<MeshRenderer>().enabled = mapVisibility;
        }
    }
}
