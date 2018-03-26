using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInScript : MonoBehaviour {

    private float currentAlpha = 1;
    private int currentFrames = 0;

    // Use this for initialization
    void Start () {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }
	
	// Update is called once per frame
	void Update () {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, currentAlpha);
        currentAlpha = currentAlpha - 0.05f;

        GameObject cameraThing = GameObject.Find("Main Camera");
        transform.position = new Vector3(cameraThing.transform.position.x, cameraThing.transform.position.y, 0) ;

        if (currentFrames == 120)
        {
            Destroy(gameObject);
        }

        currentFrames++;
    }
}
