using UnityEngine;
using System.Collections;

public class KarlsruheInnenstadt : MonoBehaviour {

    float speed = 5.0f;

	// Use this for initialization
	void Start () {
	
	}

    void Update()
    {
        /*
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            moveKarlsruheMap();
        }

        if (Input.touchCount > 0 )
        {
            moveKarlsruheMap();
        }

        if (Input.GetKey(KeyCode.Space))
        {
            moveKarlsruheMap();
        }*/
    }

    private void moveKarlsruheMap()
    {
        var move = Vector3.left;
        print(move);
        transform.position += move * speed * Time.deltaTime;
    }
}
