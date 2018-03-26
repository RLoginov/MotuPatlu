using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour {

    public int explodeUponGround = 0;
    public int touchedGround = 0;

    private GameObject audioGuy;

    // Use this for initialization
    void Start () {
        GetComponent<CircleCollider2D>().enabled = false;
        Invoke("EnableCollider", 0.5f);
        audioGuy = GameObject.Find("AudioManager");
    }




    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Obstacle")
        {
            touchedGround = 1;
            if (explodeUponGround == 1)
            {
                GameObject explosion = (GameObject)Instantiate(Resources.Load("Prefabs/SmallExplosion"));
                explosion.transform.position = transform.position;
                audioGuy.GetComponent<AudioManager>().Play("Crashed");
                Destroy(gameObject);
            }
        }


        if (Time.time > 0.0f &&    // to prevent an immediate re-collision 
            !PlayerOne.invincible && touchedGround == 0) // to make sure player has just not been hit
        {
            if (collision.gameObject.tag == "Player")
            {
                collision.gameObject.GetComponent<PlayerOne>().Invoke("hurtState", 0.1f);
                P1HealthManager.health -= 33f;

                GameObject bishoom = (GameObject)Instantiate(Resources.Load("Prefabs/SmallExplosion"));
                bishoom.transform.position = collision.transform.position;


                Invoke("EnableHitbox", 2);

                Destroy(gameObject);
            }

            if (collision.gameObject.tag == "Player2")
            {
                collision.gameObject.GetComponent<PlayerTwo>().Invoke("hurtState", 0.1f);
                P2HealthManager.health -= 33f;

                GameObject bishoom = (GameObject)Instantiate(Resources.Load("Prefabs/SmallExplosion"));
                bishoom.transform.position = collision.transform.position;

                Invoke("EnableHitbox", 2);

                Destroy(gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update () {
		
	}

    void EnableCollider()
    {
        GetComponent<CircleCollider2D>().enabled = true;
    }
}
