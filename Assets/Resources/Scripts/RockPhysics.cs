using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Controller2D))]
public class RockPhysics : MonoBehaviour
{
    // bullet speed
    private int punchedSpeed = 30;

    // bullet direction
    private Rigidbody2D rockMove;

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

    private Vector3 start;
    private Vector3 destination;

    private GameObject audioGuy;

    // Use this for initialization
    void Start()
    {
        // get component for controller, sprite and stamina
        controller = GetComponent<Controller2D>();

        // get rigid body component
        rockMove = GetComponent<Rigidbody2D>();

        // get script for player one health
        playerOne = GameObject.Find("P1");
        P1ControlScript = playerOne.GetComponent<PlayerOne>();

        // adjust for gravity
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;

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

        // rock is moving to the right
        if (rockMove.velocity.x > 0)
        {
            // slow down the rock
            float x = rockMove.velocity.x;
            x = x - 1f;
            rockMove.velocity = new Vector3(x, 0, 0);
        }

        // rock is moving to the left
        else if (rockMove.velocity.x < 0)
        {
            // slow down the rock
            float x = rockMove.velocity.x;
            x = x + 1f;
            rockMove.velocity = new Vector3(x, 0, 0);
        }

        // rock is moving up
        if (rockMove.velocity.y > 0)
        {
            // slow down the rock
            float y = rockMove.velocity.y;
            y = y - 1f;
            rockMove.velocity = new Vector3(0, y, 0);
        }


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
            // move player at normal speed
            input.x = -moveSpeed;
        }

        // player want to move right
        else if (Input.GetKey(moveRight))
        {
            // move player at normal speed
            input.x = moveSpeed;
        }

        // player does not want to move
        else
        {
            // do not move player
            input.x = 0;
        }

        // player wants to jump
        if (Input.GetKey(jump))
        {
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

        

        // move player
        float targetVelocityX = input.x * moveSpeed * Time.deltaTime;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // get tag of object colliding into
        string name = other.gameObject.tag;
        
        // rock is hit by punch
        if (name == "PunchBox")
        {
            audioGuy.GetComponent<AudioManager>().Play("Punched");
            // move rock to the right 
            if (P1ControlScript.facingRight)
                rockMove.velocity = new Vector3(punchedSpeed, 0, 0);

            // move rock to the left
            else
                rockMove.velocity = new Vector3(-punchedSpeed, 0, 0);
        }
    }
}
