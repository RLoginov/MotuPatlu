using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairFixRock : MonoBehaviour {

    public float horizontalStrength = 0.1f;
    public float verticalStrength = 0.1f;

    private Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if (rb2d.velocity.x > 0)
        {
            rb2d.velocity -= new Vector2(horizontalStrength, 0);
        }
        if (rb2d.velocity.x < 0)
        {
            rb2d.velocity += new Vector2(horizontalStrength, 0);
        }
        if (rb2d.velocity.x > -0.1f && rb2d.velocity.x < 0.1f)
        {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }

        if (GetComponent<GrabbableScript>().grabbed == 0 && rb2d.velocity.y != 0)
        {
            Debug.Log(verticalStrength);
            rb2d.velocity -= new Vector2(0, verticalStrength);
        }
        if (GetComponent<GrabbableScript>().grabbed == 1)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.y, 0);
        }
	}
}
