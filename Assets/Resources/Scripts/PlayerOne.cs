using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent (typeof(Controller2D))]
public class PlayerOne : MonoBehaviour {

    // movement variables
    public float jumpHeight = 4;
    public float timeToJumpApex = 0.4f;
    float moveSpeed = 25;
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
    public KeyCode punch;
    public KeyCode reset;

    // player controller
    Controller2D controller;

    // animation controller
    public static Animator anim;

    // character direction
    public bool facingRight;

    // wall slide when wall jumping
    public float wallSlideSpeedMax = 3;
    public float wallStickTime = 0.25f;

    // wall jump variables
    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;

    // player selecter
    public bool P1Selected = true;

    private GameObject audioGuy;

    private int jumping = 0;
    private int floorChecking = 1;
    private GameObject cameraGuy;

    public GameObject punchHitBox;
    private bool currentlyPunching = false;
    private int punching = 0;

    // used for character interactions
    public static Rigidbody2D rb;
    public static bool invincible = false;

    private int fightingBoss = 0;


    // Use this for initialization
    void Start ()
    {
        // get component for controller, sprite and stamina
        controller = GetComponent<Controller2D>();

        // adjust for gravity
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;

        rb = this.GetComponent<Rigidbody2D>();

        // get animation component
        anim = GetComponent<Animator>();

        audioGuy = GameObject.Find("AudioManager");
        cameraGuy = GameObject.Find("Main Camera");

        if (GameObject.Find("House") != null)
        {
            fightingBoss = 1;
        }

        if (GameObject.Find("Number2") != null)
        {
            fightingBoss = 1;
        }

        if (GameObject.Find("John") != null)
        {
            fightingBoss = 1;
        }


        GameObject fade = (GameObject)Instantiate(Resources.Load("Prefabs/BlackFadeIn"));
        fade.transform.position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(reset))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // get player input for direction and which direction they are colliding inot the wall
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        int wallDirX = (controller.collisions.left) ? -1 : 1;

        // currently not sliding on wall
        bool wallSliding = false;

        GameObject myPartner = GameObject.Find("P2");

        // switch who is being controlled
        if (Input.GetKeyDown(selectPlayer) && P1Selected == true)
        {
            P1Selected = false;
            if (fightingBoss == 1)
            {
                myPartner.transform.position = new Vector3(transform.position.x, transform.position.y+1, transform.position.z);
                transform.position = new Vector3(-1000, 1000, 1000);
            }
            
        }
        // switch who is being controlled
        else if (Input.GetKeyDown(selectPlayer) && P1Selected == false)
        {
            P1Selected = true;
            if (fightingBoss == 1)
            {
                transform.position = myPartner.transform.position;
                myPartner.transform.position = new Vector3(-1000, 1000, 1000);
            }
        }
            

        anim.SetInteger("P1Punch", punching);

        // move if currently selected
        if (P1Selected)
        {
            // end P2 movement
            PlayerTwo.anim.SetTrigger("P2 Idle");

            cameraGuy.GetComponent<CameraController>().player = 1;

            // check if sliding on wall
            if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
            {
                // currently colliding into wall
                wallSliding = true;

                // slide down wall slowly
                if (velocity.y < -wallSlideSpeedMax)
                {
                    velocity.y = -wallSlideSpeedMax;
                }
            }

            // stop when hitting object from above or below
            if (controller.collisions.above || controller.collisions.below)
            {
                velocity.y = 0;
            }

            // player wants to punch
            if (Input.GetKeyDown(punch) && punching == 0 && jumping == 0)
            {
                Invoke("emptyPunch", 0.2f);
                punching = 1;
                audioGuy.GetComponent<AudioManager>().Play("Punching");
            }

            // player wants to move left
            if (Input.GetKey(moveLeft))
            {
                // set right walk cycle
                if (facingRight)
                {
                    Flip();
                }

                // set animation and which way the character is facing
                if (jumping == 0)
                {
                    anim.SetTrigger("P1 Walking");
                }
                else
                {
                    anim.SetInteger("P1Jump", 1);
                }
                
                facingRight = false;

                // move player at normal speed
                input.x = -moveSpeed;
            }

            // player want to move right
            else if (Input.GetKey(moveRight))
            {
                // set right walk cycle
                if (!facingRight)
                {
                    Flip();
                }

                // set animation and which way the character is facing
                if (jumping == 0)
                {
                    anim.SetTrigger("P1 Walking");
                }
                else
                {
                    anim.SetInteger("P1Jump", 1);
                }
                
                facingRight = true;

                // move player at normal speed
                input.x = moveSpeed;
            }

            // player does not want to move
            else
            {
                // do not move player
                input.x = 0;

                // end any movement animation
                if (jumping == 0)
                {
                    anim.SetTrigger("P1 Idle");
                }
                else
                {
                    anim.SetInteger("P1Jump", 1);
                }
                
            }

            // player wants to jump
            if (Input.GetKeyDown(jump) && punching == 0)
            {
                

                // check if sliding on wall
                if (wallSliding)
                {
                    anim.SetInteger("P1Jump", 1);
                    audioGuy.GetComponent<AudioManager>().Play("Jump");
                    jumping = 1;
                    floorChecking = 0;
                    Invoke("enableFloorCheck", 0.5f);

                    // player jumps to same wall they are sliding on
                    if (wallDirX == input.x)
                    {
                        velocity.x = -wallDirX * wallJumpClimb.x;
                        velocity.y = wallJumpClimb.y;
                    }

                    // player jumps off wall
                    else if (input.x == 0)
                    {
                        velocity.x = -wallDirX * wallJumpOff.x;
                        velocity.y = wallJumpOff.y;
                    }

                    // player jumps off wall and goes in opposite direction of wall
                    else
                    {
                        velocity.x = -wallDirX * wallLeap.x;
                        velocity.y = wallLeap.y;
                    }
                }

                // be able to jump when on ground
                if (controller.collisions.below)
                {
                    anim.SetInteger("P1Jump", 1);
                    audioGuy.GetComponent<AudioManager>().Play("Jump");
                    jumping = 1;
                    floorChecking = 0;
                    Invoke("enableFloorCheck", 0.5f);
                    velocity.y = jumpVelocity;
                }
            }
            
        }

        if (controller.collisions.below && floorChecking == 1 && jumping == 1)
        {
            anim.SetInteger("P1Jump", 0);
            jumping = 0;
        }

        // check if player is not selected
        if (P1Selected == false)
        {
            // do not move player
            input.x = 0;
        }

        // move player
        float targetVelocityX = input.x * moveSpeed * Time.deltaTime;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
	}

    // reverse character sprite 
    void Flip()
    {
        Vector3 flip = transform.localScale;
        flip.x *= -1;
        transform.localScale = flip;
    }

    void enableFloorCheck()
    {
        floorChecking = 1;
    }

    void emptyPunch()
    {
        if (facingRight)
        {
            GameObject myBullet = (GameObject)Instantiate(punchHitBox, new Vector3(transform.position.x + 2f, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);
            myBullet.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(5, 0);
        }
        else
        {
            GameObject myBullet = (GameObject)Instantiate(punchHitBox, new Vector3(transform.position.x - 2f, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);
            myBullet.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-5, 0);
        }
        punching = 0;
    }

    public void hurtState()
    {
        audioGuy.GetComponent<AudioManager>().Play("PlayerHit");

        anim.SetInteger("P1Hurt", 1);

        GameObject bishoom = (GameObject)Instantiate(Resources.Load("Prefabs/bishoom"));
        bishoom.transform.position = transform.position;

        invincible = true;

        if(facingRight)
        {
            rb.velocity = new Vector3(-5, 10, 0); 
        }

        else
        {
            rb.velocity = new Vector3(5, 10, 0);
        }

        Invoke("idleState", 1);
    }

    void idleState()
    {
        anim.SetInteger("P1Hurt", 0);

        rb.velocity = new Vector3(0, 0, 0);

        invincible = false;
    }
}
