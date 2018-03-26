using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : RaycastController
{

    // passenger layer
    public LayerMask passengerMask;

    // waypoints for platforms to move between
    public Vector3[] localWaypoints;
    Vector3[] globalWaypoints;

    // waypoint related variables
    public float speed;
    public bool cyclic;
    public float waitTime;
    [Range(0, 2)]
    public float easeAmount;
    int fromWaypointIndex;
    float percentBetweenWaypoints;
    float nextMoveTime;

    // list and dictionary for moving objects colliding with the moving platform
    List<PassengerMovement> passengerMovement;
    Dictionary<Transform, Controller2D> passengerDictionary = new Dictionary<Transform, Controller2D>();

    // Use this for initialization
    public override void Start()
    {
        // call base start method
        base.Start();

        // create distances for waypoints
        globalWaypoints = new Vector3[localWaypoints.Length];

        // start global waypoints from local waypoints
        for (int i = 0; i < localWaypoints.Length; i++)
        {
            globalWaypoints[i] = localWaypoints[i] + transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // update raycast for moving platform
        UpdateRaycastOrigins();

        // calculate platform movement 
        Vector3 velocity = CalculatePlatformMovement();

        // calculate object movement with platform movement
        CalculatePassengerMovement(velocity);

        // move objects colliding with moving platform
        MovePassengers(true);
        transform.Translate(velocity);
        MovePassengers(false);
    }

    float Ease(float x)
    {
        // slow down platform when approaching its waypoint
        float a = easeAmount + 1;
        return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
    }

    Vector3 CalculatePlatformMovement()
    {
        // check if platform is at destination
        if (Time.time < nextMoveTime)
        {
            return Vector3.zero;
        }

        // calculate distance between platform and waypoint
        fromWaypointIndex %= globalWaypoints.Length;
        int toWaypointIndex = (fromWaypointIndex + 1) % globalWaypoints.Length;
        float distanceBetweenWaypoints = Vector3.Distance(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex]);
        percentBetweenWaypoints += Time.deltaTime * speed / distanceBetweenWaypoints;
        percentBetweenWaypoints = Mathf.Clamp01(percentBetweenWaypoints);
        float easedPercentBetweenWaypoints = Ease(percentBetweenWaypoints);

        // interpolate between waypoints
        Vector3 newPos = Vector3.Lerp(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex], easedPercentBetweenWaypoints);

        // check if there is distance left to travel
        if (percentBetweenWaypoints >= 1)
        {
            // set percent travel to zero and go to next waypoint
            percentBetweenWaypoints = 0;
            fromWaypointIndex++;

            // check if cycling between wapoints
            if (!cyclic)
            {
                // traverse waypoints in reverse order
                if (fromWaypointIndex >= globalWaypoints.Length - 1)
                {
                    fromWaypointIndex = 0;
                    System.Array.Reverse(globalWaypoints);
                }
            }

            // get time for platform to reach destination
            nextMoveTime = Time.time + waitTime;
        }

        // return position of platform
        return newPos - transform.position;
    }

    void MovePassengers(bool beforeMovePlatform)
    {
        // loop through each object colliding with moving platform
        foreach (PassengerMovement passenger in passengerMovement)
        {
            // add passenger to dictionary
            if (!passengerDictionary.ContainsKey(passenger.transform))
            {
                passengerDictionary.Add(passenger.transform, passenger.transform.GetComponent<Controller2D>());
            }

            // first move passenger than move platform
            if (passenger.moveBeforePlatform == beforeMovePlatform)
            {
                passengerDictionary[passenger.transform].Move(passenger.velocity, passenger.standingOnPlatform);
            }
        }
    }

    void CalculatePassengerMovement(Vector3 velocity)
    {
        // create hash for passangers
        HashSet<Transform> movedPassengers = new HashSet<Transform>();
        passengerMovement = new List<PassengerMovement>();

        // get x and y direction for passengers
        float directionX = Mathf.Sign(velocity.x);
        float directionY = Mathf.Sign(velocity.y);

        // vertically moving platform
        if (velocity.y != 0)
        {
            // get raycasted lines
            float rayLength = Mathf.Abs(velocity.y) + skinWidth;

            // loop through lines
            for (int i = 0; i < verticalRayCount; i++)
            {
                // get if raycasted lines is hitting something
                Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
                rayOrigin += Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, passengerMask);

                // raycast lines hit something
                if (hit)
                {
                    // check if passenger is already in dictionary
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        // add passanger to dictionary
                        movedPassengers.Add(hit.transform);
                        float pushX = (directionY == 1) ? velocity.x : 0;
                        float pushY = velocity.y - (hit.distance - skinWidth) * directionY;
                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), directionY == 1, true));
                    }
                }
            }
        }


        // horizontally moving platform
        if (velocity.x != 0)
        {
            // get raycasted lines
            float rayLength = Mathf.Abs(velocity.x) + skinWidth;

            // loop through lines
            for (int i = 0; i < horizontalRayCount; i++)
            {
                // get if raycasted lines is hitting something
                Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
                rayOrigin += Vector2.up * (horizontalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, passengerMask);

                // raycast lines hit something
                if (hit)
                {
                    // check if passenger is already in dictionary
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        // add passanger to dictionary
                        movedPassengers.Add(hit.transform);
                        float pushX = velocity.x - (hit.distance - skinWidth) * directionX;
                        float pushY = -skinWidth;
                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), false, true));
                    }
                }
            }
        }

        // passenger is on top of a horizontally or downward moving paltform
        if (directionY == -1 || velocity.y == 0 && velocity.x != 0)
        {
            // get length of raycested lines
            float rayLength = skinWidth * 2;

            // loop through lines
            for (int i = 0; i < verticalRayCount; i++)
            {
                // get if raycasted lines is hitting something
                Vector2 rayOrigin = raycastOrigins.topLeft + Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, passengerMask);

                // raycast lines hit something
                if (hit)
                {
                    // check if passenger is already in dictionary
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        // add passanger to dictionary
                        movedPassengers.Add(hit.transform);
                        float pushX = velocity.x;
                        float pushY = velocity.y;
                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), true, false));
                    }
                }
            }
        }
    }

    struct PassengerMovement
    {
        // passenger related variables
        public Transform transform;
        public Vector3 velocity;
        public bool standingOnPlatform;
        public bool moveBeforePlatform;

        // constructor
        public PassengerMovement(Transform _transform, Vector3 _velocity, bool _standingOnPlatform, bool _moveBeforePlatform)
        {
            // assign values
            transform = _transform;
            velocity = _velocity;
            standingOnPlatform = _standingOnPlatform;
            moveBeforePlatform = _moveBeforePlatform;
        }
    }


    void OnDrawGizmos()
    {
        // show waypoints that are not null
        if (localWaypoints != null)
        {
            // coler waypoints red
            Gizmos.color = Color.red;
            float size = 0.3f;

            // loop through waypoints
            for (int i = 0; i < localWaypoints.Length; i++)
            {
                // display waypoints
                Vector3 globalWayPointPos = (Application.isPlaying) ? globalWaypoints[i] : localWaypoints[i] + transform.position;
                Gizmos.DrawLine(globalWayPointPos - Vector3.up * size, globalWayPointPos + Vector3.up * size);
                Gizmos.DrawLine(globalWayPointPos - Vector3.left * size, globalWayPointPos + Vector3.left * size);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" ||
            collision.gameObject.tag == "Player2")
        {
            collision.transform.SetParent(transform);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" ||
            collision.gameObject.tag == "Player2")
        {
            collision.transform.SetParent(null);
        }
    }
}
