using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUp : MonoBehaviour
{
    private GameObject audioGuy;

    void Start()
    {
        audioGuy = GameObject.Find("AudioManager");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && P1HealthManager.health < 99f)
        {
            audioGuy.GetComponent<AudioManager>().Play("MotuSamosa");
            P1HealthManager.health += 33f;
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Player2" && P2HealthManager.health < 99f)
        { 
            audioGuy.GetComponent<AudioManager>().Play("PatluSamosa");
            P2HealthManager.health += 33f;
            Destroy(gameObject);
        }
    }
}
