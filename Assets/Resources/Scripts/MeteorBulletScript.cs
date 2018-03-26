using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorBulletScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
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
                Destroy(gameObject);
            }

            if (collision.gameObject.tag == "Player2")
            {
                collision.gameObject.GetComponent<PlayerTwo>().Invoke("hurtState", 0.1f);
                P2HealthManager.health -= 33f;
                Destroy(gameObject);
            }
        }
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
