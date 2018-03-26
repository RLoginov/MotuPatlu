using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBounceScript : MonoBehaviour {

    private Rigidbody2D rb2d;
    private int framesPassed = 0;
    public Rigidbody2D shotObject;
    private int myBulletThing = 3;
    private GameObject audioGuy;
    private GameObject cameraGuy;
    private int touchedGround = 0;

    // Use this for initialization
    void Start() {
        rb2d = GetComponent<Rigidbody2D>();
        audioGuy = GameObject.Find("AudioManager");
        cameraGuy = GameObject.Find("Main Camera");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            touchedGround = 1;
        }


        if (Time.time > 0.0f &&    // to prevent an immediate re-collision 
            !PlayerOne.invincible) // to make sure player has just not been hit
        {
            if (collision.gameObject.tag == "Player")
            {
                collision.gameObject.GetComponent<PlayerOne>().Invoke("hurtState", 0.1f);
                P1HealthManager.health -= 33f;
            }

            if (collision.gameObject.tag == "Player2")
            {
                collision.gameObject.GetComponent<PlayerTwo>().Invoke("hurtState", 0.1f);
                P2HealthManager.health -= 33f;
            }
        }
    }

    void OnCollisionStay2D(Collision2D coll)
    {
        if ((coll.gameObject.tag == "Rock" || coll.gameObject.tag == "PunchBox") && coll.gameObject.GetComponent<Rigidbody2D>().velocity.x != 0)
        {
            audioGuy.GetComponent<AudioManager>().Play("EnemyHit");
            audioGuy.GetComponent<AudioManager>().Play("Death4");
            if (coll.gameObject.tag == "PunchBox")
            {
                audioGuy.GetComponent<AudioManager>().Play("Punched");
            }

            GameObject bishoom = (GameObject)Instantiate(Resources.Load("Prefabs/bishoom"));
            bishoom.transform.position = coll.transform.position;

            GameObject damageNumber = (GameObject)Instantiate(Resources.Load("Prefabs/five"));
            damageNumber.transform.position = coll.transform.position;

            GameObject defeatedSelf = (GameObject)Instantiate(Resources.Load("Prefabs/DefeatedEnemy"));
            defeatedSelf.transform.position = transform.position;
            defeatedSelf.transform.localScale = transform.localScale;
            if (coll.gameObject.GetComponent<Rigidbody2D>().velocity.x > 0)
            {
                defeatedSelf.GetComponent<FlyingDefeatScript>().right = 1;
            }
            else
            {
                defeatedSelf.GetComponent<FlyingDefeatScript>().right = 0;
            }
            defeatedSelf.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;

            if ((coll.gameObject.GetComponent("GrabbableScript") as GrabbableScript) != null)
            {
                if (coll.gameObject.GetComponent<GrabbableScript>().invincible == 0)
                {
                    GameObject defeatedOther = (GameObject)Instantiate(Resources.Load("Prefabs/DefeatedObject"));
                    defeatedOther.transform.position = coll.transform.position;
                    defeatedOther.transform.localScale = coll.transform.localScale;

                    if (coll.gameObject.GetComponent<Rigidbody2D>().velocity.x > 0)
                    {
                        defeatedOther.GetComponent<FlyingDefeatScript>().right = 0;
                    }
                    else
                    {
                        defeatedOther.GetComponent<FlyingDefeatScript>().right = 1;
                    }
                    defeatedOther.GetComponent<SpriteRenderer>().sprite = coll.gameObject.GetComponent<SpriteRenderer>().sprite;
                    Destroy(coll.gameObject);
                }
            }

            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update () {
        if (cameraGuy.transform.position.x + 25 < transform.position.x)
        {
            return;
        }


        if(touchedGround == 1)
        {
            rb2d.velocity = new Vector2(-5, 25);
            touchedGround = 0;
        }
        else
        {
            rb2d.velocity = new Vector2(-5, rb2d.velocity.y);
        }
    }
}
