using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    Animator anim;
    public static bool doorOpen;
    public string sceneToLoad;

    // Use this for initialization
    void Start ()
    {
        anim = GetComponent<Animator>();
        doorOpen = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!doorOpen)
        {
            anim.SetBool("Unlocked", false);
        }

        else
        {
            anim.SetBool("Unlocked", true);
        }

	}


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Player2")
        {
            if (doorOpen)
            {
                GetComponent<BoxCollider2D>().enabled = false;
                Invoke("NextStage", 1);
                GameObject bomb = (GameObject)Instantiate(Resources.Load("Prefabs/BlackFadeOut"));
                bomb.transform.position = transform.position;
            }
        }
    }

    void NextStage()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
