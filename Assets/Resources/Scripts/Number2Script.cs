using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Number2Script : MonoBehaviour {

    private Rigidbody2D rb2d;
    private GameObject audioGuy;
    private int right = 0;
    public int phase = 0;
    public static Animator anim;
    public int hp = 40;

    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        audioGuy = GameObject.Find("AudioManager");
        int randomNumber = Random.Range(1, 4);
        Invoke("ChangeDirection", randomNumber);

        Invoke("DropBomb", 1);
        Invoke("DropInvisibleBomb", 4.5f);

        anim = GetComponent<Animator>();
    }


    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Rock" && coll.gameObject.GetComponent<Rigidbody2D>().velocity.y > 0 && phase == 0)
        {
            anim.SetInteger("Number2Jump", 1);
            anim.SetInteger("Number2Walk", 0);
            phase = 1;
            audioGuy.GetComponent<AudioManager>().Play("Hitstun");
            rb2d.velocity = new Vector2(0, -20);
            if (coll.gameObject.tag == "PunchBox")
            {
                audioGuy.GetComponent<AudioManager>().Play("Punched");
            }

            hp -= 5;

            GameObject bishoom = (GameObject)Instantiate(Resources.Load("Prefabs/bishoom"));
            bishoom.transform.position = coll.transform.position;

            GameObject damageNumber = (GameObject)Instantiate(Resources.Load("Prefabs/five"));
            damageNumber.transform.position = coll.transform.position;

            if ((coll.gameObject.GetComponent("GrabbableScript") as GrabbableScript) != null)
            {
                if (coll.gameObject.GetComponent<GrabbableScript>().invincible == 0)
                {
                    GameObject defeatedOther = (GameObject)Instantiate(Resources.Load("Prefabs/DefeatedObject"));
                    defeatedOther.transform.position = coll.transform.position;
                    defeatedOther.transform.localScale = coll.transform.localScale;

                    if (coll.gameObject.GetComponent<Rigidbody2D>().velocity.x > 0)
                    {
                        defeatedOther.GetComponent<FlyingDefeatScript>().right = 0;
                    }
                    else
                    {
                        defeatedOther.GetComponent<FlyingDefeatScript>().right = 1;
                    }
                    defeatedOther.GetComponent<SpriteRenderer>().sprite = coll.gameObject.GetComponent<SpriteRenderer>().sprite;
                    Destroy(coll.gameObject);
                }
            }
        }

        if (coll.gameObject.tag == "PunchBox" && phase == 2)
        {
            audioGuy.GetComponent<AudioManager>().Play("Hitstun");
            if (coll.gameObject.tag == "PunchBox")
            {
                //audioGuy.GetComponent<AudioManager>().Play("Punched");
            }

            GameObject bishoom = (GameObject)Instantiate(Resources.Load("Prefabs/bishoom"));
            bishoom.transform.position = coll.transform.position;

            GameObject damageNumber = (GameObject)Instantiate(Resources.Load("Prefabs/five"));
            damageNumber.transform.position = coll.transform.position;
            hp -= 5;

            Destroy(coll.gameObject);
        }

        if (coll.gameObject.tag == "Obstacle" && phase == 1)
        {
            anim.SetInteger("Number2Jump", 0);
            anim.SetInteger("Number2Stun", 1);
            rb2d.velocity = new Vector2(0, 0);
            phase = 2;
            Invoke("DizzyStars", 0.3f);
            audioGuy.GetComponent<AudioManager>().Play("Dizzy");
            Invoke("JumpUp", 6);
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

            Invoke("NextStage", 4);
        }
        if (phase == 6)
        {
            phase = 7;
        }

        if (phase == 0)
        {
            if (right == 1)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                rb2d.velocity = new Vector2(7, 0);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
                rb2d.velocity = new Vector2(-7, 0);
            }

            if (transform.position.x < -13)
            {
                right = 1;
            }

            if (transform.position.x > 13)
            {
                right = 0;
            }
        }
		if (phase == 2)
        {
            rb2d.velocity = new Vector2(0, 0);
        }
        if (phase == 1)
        {
            rb2d.velocity = new Vector2(0, -20);
        }
        
	}

    void ChangeDirection()
    {
        if (right == 0)
        {
            right = 1;
        }
        else
        {
            right = 0;
        }

        int randomNumber = Random.Range(1, 6);
        Invoke("ChangeDirection", randomNumber);
    }

    void DropBomb()
    {
        Invoke("DropBomb", 1);
        if (phase > 0)
        {
            return;
        }
        GameObject bomb = (GameObject)Instantiate(Resources.Load("Prefabs/NormalBomb"));
        bomb.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -7);
        bomb.transform.position = transform.position;
    }

    void DropInvisibleBomb()
    {
        Invoke("DropInvisibleBomb", 4.5f);
        if (phase > 0)
        {
            return;
        }
        GameObject bomb = (GameObject)Instantiate(Resources.Load("Prefabs/InvisibleBomb"));
        bomb.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -7);
        bomb.transform.position = transform.position;
    }

    void JumpUp()
    {
        if (phase == 7)
        {
            return;
        }
        anim.SetInteger("Number2Jump", 1);
        anim.SetInteger("Number2Stun", 0);
        rb2d.velocity = new Vector2(0, 20);
        phase = 3;
        Invoke("FinishJumping", 0.5f);
    }

    void FinishJumping()
    {
        if (phase == 7)
        {
            return;
        }
        anim.SetInteger("Number2Walk", 1);
        anim.SetInteger("Number2Jump", 0);
        rb2d.velocity = new Vector2(0, 0);
        phase = 0;
    }

    void DizzyStars()
    {
        if (phase == 2)
        {
            GameObject bomb = (GameObject)Instantiate(Resources.Load("Prefabs/DizzyStar"));
            bomb.transform.position = transform.position;
            Invoke("DizzyStars", 0.3f);
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
