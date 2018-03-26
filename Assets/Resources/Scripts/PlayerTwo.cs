using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Controller2D))]
public class PlayerTwo : MonoBehaviour
{

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
    public KeyCode punch;

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

    // health script for player one 
    PlayerOne P1ControlScript;

    private GameObject cameraGuy;
    private GameObject audioGuy;
    private int jumping = 0;
    private int floorChecking = 0;
    private int pickup = 0;
    public int carrying = 0;

    // used for character interactions
    public static Rigidbody2D rb;
    public static bool invincible = false;


    // Use this for initialization
    void Start()
    {
        // get component for controller, sprite and stamina
        controller = GetComponent<Controller2D>();

        // get script for player one health
        playerOne = GameObject.Find("P1");
        P1ControlScript = playerOne.GetComponent<PlayerOne>();

        // adjust for gravity
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;

        // get rigid body
        rb = this.GetComponent<Rigidbody2D>();

        // get animation component
        anim = GetComponent<Animator>();

        cameraGuy = GameObject.Find("Main Camera");
        audioGuy = GameObject.Find("AudioManager");
    }


    // Update is called once per frame
    void Update()
    {
        // get player input for direction and which direction they are colliding inot the wall
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        int wallDirX = (controller.collisions.left) ? -1 : 1;

        // currently not sliding on wall
        bool wallSliding = false;

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

        anim.SetInteger("P2Jump", jumping);
        anim.SetInteger("P2Pickup", pickup);

        // move if currently selected
        if (P1ControlScript.P1Selected == false)
        {
            // end P1 movement
            PlayerOne.anim.SetTrigger("P1 Idle");



            cameraGuy.GetComponent<CameraController>().player = 2;

            // stop when hitting object from above or below
            if (controller.collisions.above || controller.collisions.below)
            {
                velocity.y = 0;
            }

            if (Input.GetKeyDown(punch) && pickup == 0)
            {
                Invoke("emptyPickup", 0.1f);
                pickup = 1;
                

                if (carrying == 0)
                {
                    GameObject myBullet = (GameObject)Instantiate(Resources.Load("Prefabs/GrabHitbox"));
                    
                    if (facingRight)
                    {
                        myBullet.GetComponent<GrabHitboxScript>().right = 1;
                        myBullet.transform.position = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
                    }
                    else
                    {
                        myBullet.GetComponent<GrabHitboxScript>().right = 0;
                        myBullet.transform.position = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
                    }
                }
                else
                {
                    GameObject myBullet = (GameObject)Instantiate(Resources.Load("Prefabs/ThrowHitbox"));
                    
                    if (facingRight)
                    {
                        myBullet.GetComponent<GrabHitboxScript>().right = 1;
                        myBullet.transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
                    }
                    else
                    {
                        myBullet.GetComponent<GrabHitboxScript>().right = 0;
                        myBullet.transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
                    }
                }
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
                    anim.SetTrigger("P2 Walking");
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
                    anim.SetTrigger("P2 Walking");
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
                    anim.SetTrigger("P2 Idle");
                }
                
            }

            // player wants to jump
            if (Input.GetKey(jump))
            {

                // check if sliding on wall
                if (wallSliding)
                {
                    jumping = 1;
                    floorChecking = 0;
                    Invoke("enableFloorCheck", 0.1f);
                    audioGuy.GetComponent<AudioManager>().Play("Jump");

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
                    jumping = 1;
                    floorChecking = 0;
                    Invoke("enableFloorCheck", 0.5f);
                    audioGuy.GetComponent<AudioManager>().Play("Jump");
                    velocity.y = jumpVelocity;
                }
            }
        }

        if (controller.collisions.below && floorChecking == 1 && jumping == 1)
        {
            jumping = 0;
        }

        // check if player is not selected
        if (P1ControlScript.P1Selected == true)
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

    void emptyPickup()
    {
        pickup = 0;
    }

    public void hurtState()
    {
        audioGuy.GetComponent<AudioManager>().Play("PlayerHit");

        anim.SetInteger("P2Hurt", 1);

        GameObject bishoom = (GameObject)Instantiate(Resources.Load("Prefabs/bishoom"));
        bishoom.transform.position = transform.position;

        invincible = true;

        if (facingRight)
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
        anim.SetInteger("P2Hurt", 0);

        rb.velocity = new Vector3(0, 0, 0);

        invincible = false;
    }
}
