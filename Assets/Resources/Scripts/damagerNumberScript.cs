using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damagerNumberScript : MonoBehaviour {

    private float myAlpha = 1f;
    private Rigidbody2D rb2d;
    private int currentFrame = 0;

    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = new Vector3(Random.Range(-5.0f, 5.0f), 15, 0);
    }
	
	// Update is called once per frame
	void Update () {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, myAlpha);
        if (currentFrame == 160)
        {
            Destroy(gameObject);
        }
        if(currentFrame > 20)
        {
            myAlpha = myAlpha - 0.03f;
        }
        
        currentFrame++;
    }
}
