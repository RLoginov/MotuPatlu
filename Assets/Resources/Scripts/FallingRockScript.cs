using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRockScript : MonoBehaviour {

    private int checkMe = 0;

	// Use this for initialization
	void Start () {

	}


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            checkMe = 1;
            GetComponent<FallingRockScript>().enabled = false;
            return;
        }


        if (Time.time > 0.0f &&    // to prevent an immediate re-collision 
            !PlayerOne.invincible && checkMe == 0) // to make sure player has just not been hit
        {
            if (collision.gameObject.tag == "Player")
            {
                collision.gameObject.GetComponent<PlayerOne>().Invoke("hurtState", 0.1f);
                P1HealthManager.health -= 33f;

                GameObject bishoom = (GameObject)Instantiate(Resources.Load("Prefabs/bishoom"));
                bishoom.transform.position = collision.transform.position;

                GetComponent<BoxCollider2D>().enabled = false;
                Invoke("EnableHitbox", 2);

                Destroy(gameObject);
            }

            if (collision.gameObject.tag == "Player2")
            {
                collision.gameObject.GetComponent<PlayerTwo>().Invoke("hurtState", 0.1f);
                P2HealthManager.health -= 33f;

                GameObject bishoom = (GameObject)Instantiate(Resources.Load("Prefabs/bishoom"));
                bishoom.transform.position = collision.transform.position;

                GetComponent<BoxCollider2D>().enabled = false;
                Invoke("EnableHitbox", 2);

                Destroy(gameObject);
            }
        }
    }


    // Update is called once per frame
    void Update () {
		
	}
}
