using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightScript : MonoBehaviour {


    private GameObject audioGuy;

    // Use this for initialization
    void Start () {
        audioGuy = GameObject.Find("AudioManager");
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


            }

            if (collision.gameObject.tag == "Player2")
            {
                collision.gameObject.GetComponent<PlayerTwo>().Invoke("hurtState", 0.1f);
                P2HealthManager.health -= 33f;


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



    // Update is called once per frame
    void Update () {
		
	}
}
