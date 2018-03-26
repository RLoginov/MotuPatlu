using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public bool fakeBox;
    private GameObject audioGuy;

    void Start()
    {
        audioGuy = GameObject.Find("AudioManager");

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Rock")
        {
            GameObject explosion = (GameObject)Instantiate(Resources.Load("Prefabs/SmallExplosion"));
            explosion.transform.position = collision.transform.position;

            if (!fakeBox)
            {
                GameObject key = (GameObject)Instantiate(Resources.Load("Prefabs/Key"));
                key.transform.position = collision.transform.position;
            }

            Destroy(collision.gameObject);
            Destroy(gameObject);
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
}
