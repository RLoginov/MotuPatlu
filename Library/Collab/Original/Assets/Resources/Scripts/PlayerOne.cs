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
    public KeyCode punch;
    public KeyCode selectPlayer;
    
    // player controller
    Controller2D controller;

    // animation controller
    public static Animator anim;

    bool facingRight;

    // wall slide when wall jumping
    public float wallSlideSpeedMax = 3;
    public float wallStickTime = 0.25f;

    // wall jump variables
    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;

    // player selecter
    public bool P1Selected = true;

    // Use this for initialization
    void Start () {

        // get component for controller, sprite and stamina
        controller = GetComponent<Controller2D>();

        // adjust for gravity
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;

        // get animation component
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {

        // get player input for direction and which direction they are colliding inot the wall
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        int wallDirX = (controller.collisions.left) ? -1 : 1;

        // currently not sliding on wall
        bool wallSliding = false;

        // switch who is being controlled
        if (Input.GetKeyDown(selectPlayer) && P1Selected == true)
            P1Selected = false;

        // switch who is being controlled
        else if (Input.GetKeyDown(selectPlayer) && P1Selected == false)
            P1Selected = true;

        // move if currently selected
        if (P1Selected)
        {
            // end P2 movement
            PlayerTwo.anim.SetTrigger("P2 Idle");

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

            // player wants to move left
            if (Input.GetKey(moveLeft))
            {
                // set right walk cycle
                if (facingRight)
                {
                    Flip();
                }

                // set animation and which way the character is facing
                anim.SetTrigger("P1 Walking");
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
                anim.SetTrigger("P1 Walking");
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
                anim.SetTrigger("P1 Idle");
            }

            // player wants to jump
            if (Input.GetKey(jump))
            {
                anim.SetTrigger("P1 Jumping");

                // check if sliding on wall
                if (wallSliding)
                {
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
                    velocity.y = jumpVelocity;
                }
            }

            // player wants to punch
            if(Input.GetKey(punch))
            {
                anim.SetTrigger("P1 Punching");

                anim.SetTrigger("P1 Idle");
            }

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
}
