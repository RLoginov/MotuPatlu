using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableScript : MonoBehaviour {

    public int grabbed = 0;
    private GameObject patlu;
    private Rigidbody2D rb2d;
    public int right = 0;
    public float horizontalThrowSpeed = 60;
    public float horizontalPunchSpeed = 60;
    private GameObject audioGuy;
    public int invincible = 0;

    // Use this for initialization
    void Start () {
        patlu = GameObject.Find("P2");
        rb2d = GetComponent<Rigidbody2D>();
        audioGuy = GameObject.Find("AudioManager");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" ||
            collision.gameObject.tag == "Player2")
        {
            collision.transform.SetParent(transform);
        }
    }

    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "GrabBox")
        {
            grabbed = 1;
            rb2d.velocity = new Vector2(0, 0);
            if ((GetComponent("RockPhysics") as RockPhysics) != null)
            {
                GetComponent<RockPhysics>().enabled = false;
            }
            patlu.GetComponent<PlayerTwo>().carrying = 1;
            audioGuy.GetComponent<AudioManager>().Play("Pickup");
            Destroy(coll.gameObject);

        }
        if (coll.gameObject.tag == "ThrowHitbox")
        {
            grabbed = 0;
            audioGuy.GetComponent<AudioManager>().Play("Throw");
            if (Input.GetAxis("Vertical") > 0.2)
            {
                if ((GetComponent("RockPhysics") as RockPhysics) != null)
                {
                    rb2d.velocity = new Vector2(0, horizontalThrowSpeed * 2);
                }
                else
                {
                    rb2d.velocity = new Vector2(0, horizontalThrowSpeed / 2);
                }
                    
            }
            else
            {
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
            }

            if ((GetComponent("RockPhysics") as RockPhysics) != null)
            {
                GetComponent<RockPhysics>().enabled = true;
            }
            patlu.GetComponent<PlayerTwo>().carrying = 0;
            Destroy(coll.gameObject);
        }




        if (coll.gameObject.tag == "PunchBox")
        {
            audioGuy.GetComponent<AudioManager>().Play("Punched");

            GameObject bishoom = (GameObject)Instantiate(Resources.Load("Prefabs/bishoom"));
            bishoom.transform.position = coll.transform.position;

            if (coll.gameObject.GetComponent<Rigidbody2D>().velocity.x > 0)
            {
                rb2d.velocity = new Vector2(horizontalPunchSpeed, 0);
            }
            else
            {
                rb2d.velocity = new Vector2(-horizontalPunchSpeed, 0);
            }

            Destroy(coll.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" ||
            collision.gameObject.tag == "Player2")
        {
            collision.transform.SetParent(null);
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
