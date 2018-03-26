using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseScript : MonoBehaviour {

    private Rigidbody2D rb2d;
    private GameObject audioGuy;
    private int right = 1;
    public int phase = 0;
    public static Animator anim;
    public int hp = 40;

    // Use this for initialization
    void Start () {
        audioGuy = GameObject.Find("AudioManager");

        rb2d = GetComponent<Rigidbody2D>();
        Invoke("TurnAround", 3);
    }



    void OnCollisionEnter2D(Collision2D collision)
    {

        if (Time.time > 0.0f &&    // to prevent an immediate re-collision 
            !PlayerOne.invincible) // to make sure player has just not been hit
        {
            if (collision.gameObject.tag == "Player")
            {
                collision.gameObject.GetComponent<PlayerOne>().Invoke("hurtState", 0.1f);
                P1HealthManager.health -= 33f;

                GetComponent<BoxCollider2D>().enabled = false;
                Invoke("EnableHitbox", 2);
            }

            if (collision.gameObject.tag == "Player2")
            {
                collision.gameObject.GetComponent<PlayerTwo>().Invoke("hurtState", 0.1f);
                P2HealthManager.health -= 33f;


                GetComponent<BoxCollider2D>().enabled = false;
                Invoke("EnableHitbox", 2);
            }
        }

        if (collision.gameObject.tag == "PunchBox")
        {
            audioGuy.GetComponent<AudioManager>().Play("Deflect");

            GameObject damageNumber = (GameObject)Instantiate(Resources.Load("Prefabs/zero"));
            damageNumber.transform.position = collision.transform.position;

            GameObject deflection = (GameObject)Instantiate(Resources.Load("Prefabs/DeflectEffect"));
            deflection.transform.position = collision.transform.position;



            Destroy(collision.gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Rock")
        {
            audioGuy.GetComponent<AudioManager>().Play("Crashed");

            if(coll.gameObject.GetComponent<Rigidbody2D>().velocity.x != 0)
            {
                GameObject damageNumber = (GameObject)Instantiate(Resources.Load("Prefabs/zero"));
                damageNumber.transform.position = coll.transform.position;
            }

            GameObject bishoom = (GameObject)Instantiate(Resources.Load("Prefabs/SmallExplosion"));
            bishoom.transform.position = coll.transform.position;
            Destroy(coll.gameObject);
        }
    }


    // Update is called once per frame
    void Update () {
        if (right == 1)
        {
            transform.localScale = new Vector3(1.7f, 1.7f, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1.7f, 1.7f, 1);
        }

        if (phase == 1)
        {
            if (right == 1)
            {
                rb2d.velocity = new Vector2(15, 0);
            }
            else
            {
                rb2d.velocity = new Vector2(-15, 0);
            }


            if (right == 1 && transform.position.x > 11)
            {
                audioGuy.GetComponent<AudioManager>().Play("Crashed");
                GameObject bishoom = (GameObject)Instantiate(Resources.Load("Prefabs/bishoom"));
                bishoom.transform.position = transform.position;


                GameObject rock1 = (GameObject)Instantiate(Resources.Load("Prefabs/blockRock"));
                rock1.transform.position = new Vector3(0, 10, transform.position.z);


                GameObject rock2 = (GameObject)Instantiate(Resources.Load("Prefabs/blockRock"));
                rock2.transform.position = new Vector3(-5, 10, transform.position.z);


                GameObject rock3 = (GameObject)Instantiate(Resources.Load("Prefabs/blockRock"));
                rock3.transform.position = new Vector3(-10, 10, transform.position.z);

                rb2d.velocity = new Vector2(0, 0);
                Invoke("TurnAround", 8);
                phase = 0;
            }
            if (right == 0 && transform.position.x < -11)
            {
                audioGuy.GetComponent<AudioManager>().Play("Crashed");
                GameObject bishoom = (GameObject)Instantiate(Resources.Load("Prefabs/bishoom"));
                bishoom.transform.position = transform.position;


                GameObject rock1 = (GameObject)Instantiate(Resources.Load("Prefabs/blockRock"));
                rock1.transform.position = new Vector3(0, 10, transform.position.z);


                GameObject rock2 = (GameObject)Instantiate(Resources.Load("Prefabs/blockRock"));
                rock2.transform.position = new Vector3(5, 10, transform.position.z);



                GameObject rock3 = (GameObject)Instantiate(Resources.Load("Prefabs/blockRock"));
                rock3.transform.position = new Vector3(10, 10, transform.position.z);

                rb2d.velocity = new Vector2(0, 0);
                Invoke("TurnAround", 5);
                phase = 0;
            }

        }

        if (phase == 0)
        {
            if(transform.position.x > 11)
            {
                transform.position = new Vector3(11, transform.position.y, transform.position.z);
            }
            if (transform.position.x < -11)
            {
                transform.position = new Vector3(-11, transform.position.y, transform.position.z);
            }
            rb2d.velocity = new Vector2(0, 0);
        }
    }


    void TurnAround()
    {
        if (phase > 6)
        {
            return;
        }
        if (right == 1)
        {
            right = 0;
        }
        else
        {
            right = 1;
        }
        Invoke("ChargeAttack", 3);
    }

    void ChargeAttack()
    {
        if (phase > 6)
        {
            return;
        }
        if (right == 1)
        {
            rb2d.velocity = new Vector2(15, 0);
        }
        else
        {
            rb2d.velocity = new Vector2(-15, 0);
        }
        phase = 1;
    }

    void EnableHitbox()
    {
        GetComponent<BoxCollider2D>().enabled = true;
    }
}
