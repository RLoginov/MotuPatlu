using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HouseHitboxScript : MonoBehaviour {

    public int hp = 0;
    private GameObject audioGuy;
    public int phase = 0;

    // Use this for initialization
    void Start () {
        audioGuy = GameObject.Find("AudioManager");
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "PunchBox")
        {
            audioGuy.GetComponent<AudioManager>().Play("Hitstun");

            GameObject bishoom = (GameObject)Instantiate(Resources.Load("Prefabs/bishoom"));
            bishoom.transform.position = coll.transform.position;

            GameObject damageNumber = (GameObject)Instantiate(Resources.Load("Prefabs/five"));
            damageNumber.transform.position = coll.transform.position;
            hp -= 5;

            Destroy(coll.gameObject);
        }
    }



    // Update is called once per frame
    void Update () {
        if (hp < 1 && phase < 7)
        {
            phase = 6;
            GameObject bishoom = (GameObject)Instantiate(Resources.Load("Prefabs/BigExplosion"));
            audioGuy.GetComponent<AudioManager>().Play("EnemyHit");
            audioGuy.GetComponent<AudioManager>().Stop("Hitstun");
            audioGuy.GetComponent<AudioManager>().Play("Crashed");
            bishoom.transform.position = transform.position;
            transform.position = new Vector3(-1000, -100, 0);
            audioGuy.GetComponent<AudioManager>().Play("VictoryTheme");
            audioGuy.GetComponent<AudioManager>().Stop("BossTheme");


            GameObject boss = GameObject.Find("House");
            boss.transform.position = new Vector3(-1000, -100, 0);

            boss.GetComponent<HouseScript>().phase = 7;

            Invoke("NextStage", 4);
        }

        if (phase == 6)
        {
            phase = 7;
        }
    }


    void NextStage()
    {
        GameObject bomb = (GameObject)Instantiate(Resources.Load("Prefabs/BlackFadeOut"));
        bomb.transform.position = transform.position;
        Invoke("TransitionStage", 1);
    }

    void TransitionStage()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
