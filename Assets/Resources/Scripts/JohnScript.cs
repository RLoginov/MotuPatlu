using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JohnScript : MonoBehaviour {

    private Rigidbody2D rb2d;
    private GameObject audioGuy;
    private int right = 0;
    public int phase = 0;
    private int bulletCount = 0;
    public static Animator anim;
    public int hp = 150;
    private int shootCooldown = 0;

    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        audioGuy = GameObject.Find("AudioManager");
        anim = GetComponent<Animator>();
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Rock")
        {
            anim.SetInteger("JohnRun", 0);
            anim.SetInteger("JohnJump", 0);
            anim.SetInteger("JohnShoot", 0);
            anim.SetInteger("JohnDizzy", 1);
            phase = 4;
            Invoke("DizzyStars", 0.3f);
            Invoke("RunNormal", 3.0f);
            audioGuy.GetComponent<AudioManager>().Play("Crashed");
            audioGuy.GetComponent<AudioManager>().Play("JohnHurt");
            audioGuy.GetComponent<AudioManager>().Play("Dizzy");
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            

            hp -= 5;

            GameObject bishoom = (GameObject)Instantiate(Resources.Load("Prefabs/SmallExplosion"));
            bishoom.transform.position = coll.transform.position;

            GameObject damageNumber = (GameObject)Instantiate(Resources.Load("Prefabs/five"));
            damageNumber.transform.position = coll.transform.position;

            Destroy(coll.gameObject);
        }

        if (coll.gameObject.tag == "PunchBox")
        {
            audioGuy.GetComponent<AudioManager>().Play("Hitstun");
            if (phase < 4)
            {
                if (coll.gameObject.GetComponent<Rigidbody2D>().velocity.x < 0)
                {
                    rb2d.velocity = new Vector2(-10, rb2d.velocity.y);
                }
                else
                {
                    rb2d.velocity = new Vector2(10, rb2d.velocity.y);
                }
                phase = 3;
                Invoke("RunNormal", 0.3f);
                anim.SetInteger("JohnRun", 0);
                anim.SetInteger("JohnJump", 0);
                anim.SetInteger("JohnShoot", 0);
                anim.SetInteger("JohnDizzy", 1);
            }
            
            

            GameObject bishoom = (GameObject)Instantiate(Resources.Load("Prefabs/bishoom"));
            bishoom.transform.position = coll.transform.position;

            GameObject damageNumber = (GameObject)Instantiate(Resources.Load("Prefabs/five"));
            damageNumber.transform.position = coll.transform.position;
            hp -= 5;

            Destroy(coll.gameObject);
        }

        if (Time.time > 0.0f &&    // to prevent an immediate re-collision 
            !PlayerOne.invincible && phase < 3) // to make sure player has just not been hit
        {
            if (coll.gameObject.tag == "Player")
            {
                coll.gameObject.GetComponent<PlayerOne>().Invoke("hurtState", 0.1f);
                P1HealthManager.health -= 33f;
            }

            if (coll.gameObject.tag == "Player2")
            {
                coll.gameObject.GetComponent<PlayerTwo>().Invoke("hurtState", 0.1f);
                P2HealthManager.health -= 33f;
            }
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
            audioGuy.GetComponent<AudioManager>().Stop("HermieBoss");
            Invoke("NextStage", 4);
        }
        if (phase == 6)
        {
            phase = 7;
        }

        if (phase == 4)
        {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            return;
        }

        if (phase == 0)
        {
            int moveDecider = Random.Range(1, 80);

            if (moveDecider < 2)
            {
                ChangeDirection();
                return;
            }

            int shootDecider = Random.Range(1, 80);

            if (shootDecider < 2 && shootCooldown == 0)
            {
                phase = 1;
                shootCooldown = 200;
                shootBullet();
                anim.SetInteger("JohnRun", 0);
                anim.SetInteger("JohnShoot", 1);
                audioGuy.GetComponent<AudioManager>().Play("JohnShoot");
            }

            
            
            if (right == 1)
            {
                transform.localScale = new Vector3(1, 1, 1);
                rb2d.velocity = new Vector2(7, rb2d.velocity.y);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
                rb2d.velocity = new Vector2(-7, rb2d.velocity.y);
            }

            if (transform.position.x < -10)
            {
                right = 1;
            }

            if (transform.position.x > 10)
            {
                right = 0;
            }



            int grenadeDecider = Random.Range(1, 150);

            if (grenadeDecider < 2)
            {
                phase = 2;
                if (right == 1)
                {
                    rb2d.velocity = new Vector2(-12, 30);
                }
                else
                {
                    rb2d.velocity = new Vector2(12, 30);
                }
                Invoke("ThrowGrenade", 0.5f);
                anim.SetInteger("JohnRun", 0);
                anim.SetInteger("JohnJump", 1);
                audioGuy.GetComponent<AudioManager>().Play("JohnGrenade");
            }
        }

        if(phase == 1)
        {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }

        if(phase == 2)
        {
            if (transform.position.x < -10)
            {
                rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            }

            if (transform.position.x > 10)
            {
                rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            }
        }

        

        if(shootCooldown > 0)
        {
            shootCooldown--;
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
    }

    void shootBullet()
    {
        if (phase == 7)
        {
            return;
        }
        if (phase == 4)
        {
            return;
        }
        GameObject bomb = (GameObject)Instantiate(Resources.Load("Prefabs/touhouBullet"));
        audioGuy.GetComponent<AudioManager>().Play("Pistol");
        if (right == 0)
        {
            bomb.GetComponent<Rigidbody2D>().velocity = new Vector2(-10, 0);
            bomb.transform.position = new Vector3(transform.position.x - 2, transform.position.y, transform.position.z);
        }
        else
        {
            bomb.GetComponent<Rigidbody2D>().velocity = new Vector2(10, 0);
            bomb.transform.position = new Vector3(transform.position.x + 2, transform.position.y, transform.position.z);
        }
        


        
        if (bulletCount > 2)
        {
            bulletCount = 0;
            phase = 0;
            anim.SetInteger("JohnRun", 1);
            anim.SetInteger("JohnShoot", 0);
        }
        else
        {
            bulletCount++;
            Invoke("shootBullet", 0.2f);
        }
        
    }

    void ThrowGrenade()
    {
        if (phase == 7)
        {
            return;
        }
        if (phase == 4)
        {
            return;
        }
        int bombDecider = Random.Range(1, 100);
        GameObject bomb;
        if (bombDecider > 30)
        {
            bomb = (GameObject)Instantiate(Resources.Load("Prefabs/InvisibleBomb"));
        }
        else
        {
            bomb = (GameObject)Instantiate(Resources.Load("Prefabs/NormalBomb"));
        }
        if (right == 0)
        {

            bomb.GetComponent<Rigidbody2D>().velocity = new Vector2(-10, -7);
            bomb.transform.position = new Vector3(transform.position.x - 2, transform.position.y, transform.position.z);
        }
        else
        {
            bomb.GetComponent<Rigidbody2D>().velocity = new Vector2(10, -7);
            bomb.transform.position = new Vector3(transform.position.x + 2, transform.position.y, transform.position.z);
        }
        Invoke("RunNormal", 0.8f);
    }

    void RunNormal()
    {
        if (phase == 7)
        {
            return;
        }
        phase = 0;
        anim.SetInteger("JohnRun", 1);
        anim.SetInteger("JohnJump", 0);
        anim.SetInteger("JohnShoot", 0);
        anim.SetInteger("JohnDizzy", 0);
    }

    void DizzyStars()
    {
        if (phase == 7)
        {
            return;
        }
        if (phase == 4)
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
