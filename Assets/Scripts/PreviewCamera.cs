using UnityEngine;
using System.Collections;

public class PreviewCamera : MonoBehaviour {

    public float speed = 0.1f;

	void Start () {
	
	}
	
	void Update () {
        transform.Rotate(0, speed, 0);
	}
}
