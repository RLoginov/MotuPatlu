using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableScript : MonoBehaviour {

    public int grabbed = 0;
    private GameObject patlu;
    private Rigidbody2D rb2d;
    public int right = 0;
    public float horizontalThrowSpeed = 30;

	// Use this for initialization
	void Start () {
        patlu = GameObject.Find("P2");
        rb2d = GetComponent<Rigidbody2D>();
	}

    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "GrabBox")
        {
            grabbed = 1;
            GetComponent<RockPhysics>().enabled = false;
            patlu.GetComponent<PlayerTwo>().carrying = 1;
            Destroy(coll.gameObject);
        }

        if (coll.gameObject.tag == "ThrowHitbox")
        {
            grabbed = 0;
            if (coll.gameObject.GetComponent<GrabHitboxScript>().right == 1)
            {
                transform.position = new Vector3(patlu.transform.position.x + 1, patlu.transform.position.y + 1, patlu.transform.position.z);
                rb2d.velocity = new Vector2(horizontalThrowSpeed, 0);
            }
            else
            {
                transform.position = new Vector3(patlu.transform.position.x - 1, patlu.transform.position.y + 1, patlu.transform.position.z);
                rb2d.velocity = new Vector2(-horizontalThrowSpeed, 0);
            }
            GetComponent<RockPhysics>().enabled = true;
            patlu.GetComponent<PlayerTwo>().carrying = 0;
            Destroy(coll.gameObject);
        }
    }

    // Update is called once per frame
    void Update () {
		if (grabbed == 1)
        {
            transform.position = new Vector3(patlu.transform.position.x, patlu.transform.position.y + 2.5f, patlu.transform.position.z);
        }
	}
}
