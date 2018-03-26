﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    Rigidbody2D rb;
    public float velX = 1f;
    public float velY = -1f;
    public static float flipRate = 3f;
    private float nextFlip = 0.0F;
    bool onGround = false;
    private GameObject audioGuy;

    void Start ()
    {
        rb = this.GetComponent<Rigidbody2D>();
        audioGuy = GameObject.Find("AudioManager");
    }


    void OnCollisionStay2D(Collision2D coll)
    {
        if ((coll.gameObject.tag == "Rock" || coll.gameObject.tag == "PunchBox") && coll.gameObject.GetComponent<Rigidbody2D>().velocity.x != 0)
        {
            audioGuy.GetComponent<AudioManager>().Play("EnemyHit");
            audioGuy.GetComponent<AudioManager>().Play("Death6");
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



    void Update ()
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

        velX = -velX;
        flip.x *= -1;
        transform.localScale = flip;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Obstacle")
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
}
