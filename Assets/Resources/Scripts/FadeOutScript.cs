using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutScript : MonoBehaviour {

    public float currentAlpha = 0;
    private int currentFrames = 0;

    // Use this for initialization
    void Start()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, currentAlpha);
        currentAlpha = currentAlpha + 0.05f;

        GameObject cameraThing = GameObject.Find("Main Camera");
        transform.position = new Vector3(cameraThing.transform.position.x, cameraThing.transform.position.y, 0);

        
    }
}
