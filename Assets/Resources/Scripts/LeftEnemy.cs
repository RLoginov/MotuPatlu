using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Controller2D))]
public class LeftEnemy : MonoBehaviour
{
    // bullet speed
    private int punchedSpeed = 30;

    // bullet direction
    private Rigidbody2D rb;

    // movement variables
    public float jumpHeight = 4;
    public float timeToJumpApex = 0.4f;
    public float moveSpeed = 1;
    float accelationTimeAirborne = 0.2f;
    float accelerationTimeGrounded = 0.1f;

    // movement variable adjusters
    float gravity;
    Vector3 velocity;
    float jumpVelocity;
    float velocityXSmoothing;

    // movement and action controls
    Vector2 input;
    public KeyCode moveLeft;
    public KeyCode moveRight;
    public KeyCode jump;
    public KeyCode selectPlayer;

    // player controller
    Controller2D controller;

    // animation controller
    public static Animator anim;

    // character direction
    bool facingRight;

    // wall slide when wall jumping
    public float wallSlideSpeedMax = 3;
    public float wallStickTime = 0.25f;

    // wall jump variables
    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;

    // player one and two
    private GameObject playerOne;
    private GameObject playerTwo;

    // health script for player one 
    PlayerOne P1ControlScript;
    PlayerTwo P2ControlScript;

    private Vector3 start;
    private Vector3 destination;

    private GameObject audioGuy;

    public float velX = 1f;
    public float velY = -1f;
    public static float flipRate = 3f;
    private float nextFlip = 0.0F;
    bool onGround = false;


    // Use this for initialization
    void Start()
    {
        // get component for controller, sprite and stamina
        controller = GetComponent<Controller2D>();

        // get rigid body component
        rb = GetComponent<Rigidbody2D>();

        // get script for player one 
        playerOne = GameObject.Find("P1");
        P1ControlScript = playerOne.GetComponent<PlayerOne>();

        // get script for player 2
        playerTwo = GameObject.Find("P2");
        P2ControlScript = playerTwo.GetComponent<PlayerTwo>();

        // adjust for gravity
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;

        audioGuy = GameObject.Find("AudioManager");
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x - playerOne.transform.position.x <= 10f)
        {
            rb.velocity = new Vector3(-moveSpeed, 0, 0);
        }
        else if (transform.position.x - playerTwo.transform.position.x <= 10f)
        {
            rb.velocity = new Vector3(-moveSpeed, 0, 0);
        }

        else
        {
            rb.velocity = new Vector3(0, 0, 0);
        }

    }

    void OnCollisionStay2D(Collision2D coll)
    {
        if ((coll.gameObject.tag == "Rock" || coll.gameObject.tag == "PunchBox") && coll.gameObject.GetComponent<Rigidbody2D>().velocity.x != 0)
        {
            audioGuy.GetComponent<AudioManager>().Play("EnemyHit");
            audioGuy.GetComponent<AudioManager>().Play("Death6");
            if (coll.gameObject.tag == "PunchBox")
            {
                audioGuy.GetComponent<AudioManager>().Play("Punched");
            }

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
            defeatedSelf.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;

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

            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
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
    }
}
