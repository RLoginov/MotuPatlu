using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lion : MonoBehaviour
{
    // movement variables
    public float velX = 1f;
    public float velY = -1f;

    // flipping variables
    public static float flipRate = 3f;
    private float nextFlip = 0.0F;

    // attacking variables
    public static float pounceRate = 5.0f;
    private float nextPounce = 0.0F;

    // ground and direction facing variables
    bool onGround = false;
    bool facingRight = true;
    
    // audio and rigid body
    Rigidbody2D rb;
    private GameObject audioGuy;

    void Start()
    {
        // get audio and rigidbody
        rb = this.GetComponent<Rigidbody2D>();
        audioGuy = GameObject.Find("AudioManager");
    }

    void OnCollisionStay2D(Collision2D coll)
    {
        // check if getting punch or if rock has been thrown at us
        if ((coll.gameObject.tag == "Rock" || coll.gameObject.tag == "PunchBox") && coll.gameObject.GetComponent<Rigidbody2D>().velocity.x != 0)
        {
            // play death and hits sounds
            audioGuy.GetComponent<AudioManager>().Play("EnemyHit");
            audioGuy.GetComponent<AudioManager>().Play("Death6");

            // if being punched
            if (coll.gameObject.tag == "PunchBox")
            {
                // play punch sound
                audioGuy.GetComponent<AudioManager>().Play("Punched");
            }

            // get and display hit effect
            GameObject bishoom = (GameObject)Instantiate(Resources.Load("Prefabs/bishoom"));
            bishoom.transform.position = coll.transform.position;

            // get and display damage
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

    void Update()
    {
        rb.velocity = new Vector2(velX, velY);

        if (Time.time > nextFlip)
        {
            nextFlip = Time.time + flipRate;
            Invoke("Flip", 1);
        }
    }

    // reverse character sprite 
    void Flip()
    {
        Vector3 flip = transform.localScale;

        facingRight = !facingRight;
        velX = -velX;
        flip.x *= -1;
        transform.localScale = flip;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            onGround = true;
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Time.time > nextPounce)
        {
            if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Player2")
            {
                nextPounce = Time.time + pounceRate;

                if (facingRight)
                {
                    rb.AddForce(new Vector3(10000, 100, 0));
                }

                else
                {
                    rb.AddForce(new Vector3(-10000, 100, 0));
                }
            }
        }
    }
}


