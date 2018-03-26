using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightHitboxScript : MonoBehaviour {

    private GameObject audioGuy;
    private GameObject daddy;

    // Use this for initialization
    void Start () {
        audioGuy = GameObject.Find("AudioManager");
        daddy = GameObject.Find("WhiteKnight");
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "PunchBox")
        {
            audioGuy.GetComponent<AudioManager>().Play("EnemyHit");
            audioGuy.GetComponent<AudioManager>().Play("Death1");

            GameObject bishoom = (GameObject)Instantiate(Resources.Load("Prefabs/bishoom"));
            bishoom.transform.position = coll.transform.position;

            GameObject damageNumber = (GameObject)Instantiate(Resources.Load("Prefabs/five"));
            damageNumber.transform.position = coll.transform.position;

            GameObject defeatedSelf = (GameObject)Instantiate(Resources.Load("Prefabs/DefeatedEnemy"));
            defeatedSelf.transform.position = transform.position;
            defeatedSelf.transform.localScale = transform.localScale;
            if (coll.gameObject.GetComponent<Rigidbody2D>().velocity.x > 0)
            {
                defeatedSelf.GetComponent<FlyingDefeatScript>().right = 1;
            }
            else
            {
                defeatedSelf.GetComponent<FlyingDefeatScript>().right = 0;
            }
            defeatedSelf.GetComponent<SpriteRenderer>().sprite = daddy.GetComponent<SpriteRenderer>().sprite;


            GameObject key = (GameObject)Instantiate(Resources.Load("Prefabs/Key"));
            key.transform.position = coll.transform.position;

            Destroy(coll.gameObject);
            Destroy(daddy.gameObject);
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
