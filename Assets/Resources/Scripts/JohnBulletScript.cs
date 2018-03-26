using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JohnBulletScript : MonoBehaviour {

    private Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
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
        if (transform.position.x < -14 && rb2d.velocity.x < 0)
        {
            rb2d.velocity = new Vector2(-rb2d.velocity.x, rb2d.velocity.y +2);
            GetComponent<CircleCollider2D>().enabled = false;
            Invoke("enableCollision", 0.7f);
        }

        if (transform.position.x > 14 && rb2d.velocity.x > 0)
        {
            rb2d.velocity = new Vector2(-rb2d.velocity.x, rb2d.velocity.y + 2);
            GetComponent<CircleCollider2D>().enabled = false;
            Invoke("enableCollision", 0.7f);
        }
    }


    void enableCollision()
    {
        GetComponent<CircleCollider2D>().enabled = true;
    }
}
