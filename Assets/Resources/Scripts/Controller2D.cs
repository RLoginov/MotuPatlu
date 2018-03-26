using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller2D : RaycastController
{
    // angle to go down and climb up
    float maxClimbAngle = 80;
    float maxDescendAngle = 80;

    // collison information
    public CollisionInfo collisions;

    public override void Start()
    {
        // call base start method and set collision face direction to one
        base.Start();
        collisions.faceDirection = 1;
    }

    public void Move(Vector3 velocity, bool standingOnPlatform = false)
    {
        // calculate collisions and raycast lines
        UpdateRaycastOrigins();
        collisions.Reset();
        collisions.velocityOld = velocity;

        // game object is moving left or right
        if(velocity.x != 0)
        {
            collisions.faceDirection = (int)Mathf.Sign(velocity.x);
        }

        // game object is falling
        if (velocity.y < 0)
        {
            DescendSlope(ref velocity);
        }

        // detect collisions left and right
        HorizontalCollisions(ref velocity);

        // game object s jumping
        if (velocity.y != 0)
        { 
            VerticalCollsions(ref velocity);
        }

        // move game object
        transform.Translate(velocity);

        // check if game object is standing on ground
        if(standingOnPlatform)
        {
            collisions.below = true;
        }
    }

    void HorizontalCollisions(ref Vector3 velocity)
    {
        // get x direction and length of raycasted lines
        float directionX = collisions.faceDirection;
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        // calculate raylength if moving left or right
        if(Mathf.Abs(velocity.x) < skinWidth)
        {
            rayLength = skinWidth * 2;
        }

        // loop through lines
        for (int i = 0; i < horizontalRayCount; i++)
        {
            // get if raycasted lines is hitting something
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            // debugging purposes only (display raycasted lines)
            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            // raycast lines hit something
            if (hit)
            {
                // get angle of slope climbing on
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                // check angle of slope
                if (i == 0 && slopeAngle<= maxClimbAngle)
                {
                    // descend slope
                    if(collisions.descendingSlope)
                    {
                        collisions.descendingSlope = false;
                        velocity = collisions.velocityOld;
                    }

                    // variable to have game object look like its climbing on the slope
                    float distanceToSlopeStart = 0;

                    // check if going up new slope
                    if(slopeAngle != collisions.slopeAngleOld)
                    {
                        distanceToSlopeStart = hit.distance - skinWidth;
                        velocity.x -= distanceToSlopeStart * directionX;
                    }

                    // go up or down slope
                    ClimbSlope(ref velocity, slopeAngle);
                    velocity.x += distanceToSlopeStart * directionX;
                }

                // check if slope is not to steep
                if (!collisions.climbingSlope || slopeAngle > maxClimbAngle)
                {
                    // get length of raycasted lines and speed of object
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;

                    // climb up slope
                    if(collisions.climbingSlope)
                    {
                        velocity.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
                    }

                    // assign which direction the slope is being climbed
                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;
                }
            }
        }
    }

    void VerticalCollsions(ref Vector3 velocity)
    {
        // get y direction and length of raycasted lines
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        // loop through lines
        for (int i = 0; i < verticalRayCount; i++)
        {
            // get if raycasted lines is hitting something
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            // debugging purposes only (display raycasted lines)
            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            // raycast lines hit something
            if (hit)
            {
                // get length of raycasted lines and the y velocity of the object
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                // check if climbing slope
                if(collisions.climbingSlope)
                {
                    // go up slope
                    velocity.x = velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                }

                // assign which direction the slope is being climbed
                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
        }

        // check if climbing slope
        if(collisions.climbingSlope)
        {
            // get if raycasted lines is hitting something
            float directionX = Mathf.Sign(velocity.x);
            rayLength = Mathf.Abs(velocity.x) + skinWidth;
            Vector2 rayOrigin = ((directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector2.up * velocity.y;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            // raycast lines hit something
            if (hit)
            {
                // get angle of slope climbing on
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                // check if old slope angle is same angle as current slope
                if(slopeAngle != collisions.slopeAngle)
                {
                    // assign new x velocity and slope angle
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    collisions.slopeAngle = slopeAngle;
                }
            }
        }
    }

    void ClimbSlope(ref Vector3 velocity, float slopeAngle)
    {
        // get distance of slope get climbed on
        float moveDistance = Mathf.Abs(velocity.x);
        float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        // check if slope can be climbed
        if (velocity.y <= climbVelocityY)
        {
            // get x and y velocity of object
            velocity.y = climbVelocityY;
            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);

            // set climbing slope to true, get slope angle, and set ground to true
            collisions.below = true;
            collisions.climbingSlope = true;
            collisions.slopeAngle = slopeAngle;
        }
    }

    void DescendSlope(ref Vector3 velocity)
    {
        // get if raycasted lines is hitting something
        float directionX = Mathf.Sign(velocity.x);
        Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

        // raycast lines hit something
        if (hit)
        {
            // get slope angle
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

            // check if slope is descendable
            if(slopeAngle != 0 && slopeAngle <= maxDescendAngle)
            {
                // check slope direction
                if(Mathf.Sign(hit.normal.x) == directionX)
                {
                    // check distance of object from slope
                    if(hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
                    {
                        // get x and y velocity of object
                        float moveDistance = Mathf.Abs(velocity.x);
                        float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
                        velocity.y -= descendVelocityY;

                        // set climbing slope to true, get slope angle, and set ground to true
                        collisions.slopeAngle = slopeAngle;
                        collisions.descendingSlope = true;
                        collisions.below = true;
                    }
                }
            }
        }
    }

    public struct CollisionInfo
    {
        // collision direction variables
        public bool above, below;
        public bool left, right;

        // slope variables
        public bool climbingSlope;
        public bool descendingSlope;
        public float slopeAngle, slopeAngleOld;
        public Vector3 velocityOld;
        public int faceDirection;

        // default values
        public void Reset()
        {
            above = false;
            below = false;
            left = false;
            right = false;
            climbingSlope = false;
            descendingSlope = false;

            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
        }
    }
}﻿
























