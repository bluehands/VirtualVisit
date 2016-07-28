using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.EventSystems;

public class ImageBox : MonoBehaviour {

	void OnTriggerEnter(Collider other)
    {
        print("Enter Trigger");
        SceneManager.LoadScene(1);
    }
}
