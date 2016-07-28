using UnityEngine;
using System.Collections;

public class Sphere : MonoBehaviour {

    private bool forward;
    private Renderer rend;

    public Texture[] textures;

    private int counter = 0;
    private int updateCounter = 0;

    void Start () {
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        rend.material.SetTextureScale("_MainTex", new Vector2(-1, 1));
    }
	
	void Update () {
        if(forward)
        {
           // transform.position += Vector3.forward * 0.02f;
        } else
        {
            //transform.position -= Vector3.forward * 0.02f;
        }

        if (transform.position.z < -10)
        {
           // resetSphere();
            //changeTexture();
        }
        updateCounter++;
        if (updateCounter >= 1000)
        {
            updateCounter = 0;
            changeTexture();
        }
    }

    private void changeTexture()
    {
        counter++;
        rend.material.mainTexture = textures[counter % textures.Length];
    }

    private void resetSphere()
    {
        transform.localPosition = new Vector3(0, 1, 10);
    }
}
