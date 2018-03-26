using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button4Platform : MonoBehaviour
{
    public static Animator anim;
    public GameObject platform;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Player2" ||
            collision.gameObject.tag == "Rock")
        {
            anim.SetInteger("Activated", 1);
            platform.GetComponent<PlatformController>().enabled = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Player2" ||
            collision.gameObject.tag == "Rock")
        {
            platform.GetComponent<PlatformController>().enabled = false;
            anim.SetInteger("Activated", 0);
        }
    }
}
