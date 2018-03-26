using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingDefeatScript : MonoBehaviour {

    public int right = 0;
    private Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        Invoke("MassDestruction", 2);
	}
	
	// Update is called once per frame
	void Update () {
		if (right == 1)
        {
            rb2d.velocity = new Vector2(40, 20);
        }
        else
        {
            rb2d.velocity = new Vector2(-40, 20);
        }
	}

    void MassDestruction()
    {
        Destroy(gameObject);
    }
}
