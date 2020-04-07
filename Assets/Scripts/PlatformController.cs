using System.Collections.Generic;
using UnityEngine;

public class PlatformController : RaycastController
{
    [SerializeField]
    [Tooltip("Players on this layer can interact the platform.")]
    private LayerMask passengerMask;

    [SerializeField]
    [Tooltip("Waypoints that are defined relative to the platform.")]
    private Vector2[] localWaypoints;

    [SerializeField]
    [Tooltip("Max speed.")]
    private float speed;

    [SerializeField]
    [Tooltip("If true, the platform will move through its waypoints " +
        "in a cycle.")]
    private bool cyclic;

    [SerializeField]
    [Tooltip("How long the platform stays at a waypoint before moving on.")]
    private float waitTime;

    [SerializeField]
    [Tooltip("Platform speeds up and slows down upon approaching next waypoint.")]
    [Range(0, 2)]
    private float easeAmount;

    private int fromWaypointIndex;
    private float percentBetweenWaypoints;
    private float nextMoveTime;
    private Vector2[] globalWaypoints;
    private List<PassengerMovement> passengerMovements;
    private Dictionary<Transform, Controller2D> passengerDictionary = new Dictionary<Transform, Controller2D>();

    override public void Awake()
    {
        base.Awake();

        globalWaypoints = new Vector2[localWaypoints.Length];
        for (int i = 0; i < localWaypoints.Length; i++)
        {
            globalWaypoints[i] = localWaypoints[i] + new Vector2(transform.position.x, transform.position.y);
        }
    }

    private void Update()
    {
        UpdateRaycastOrigins();
        Vector2 velocity = CalculatePlatformMovement();
        CalculatePassengerMovement(velocity);
        MovePassengers(true);
        transform.Translate(velocity);
        MovePassengers(false);
    }

    Vector2 CalculatePlatformMovement()
    {
        if (Time.time < nextMoveTime)
        {
            return Vector2.zero;
        }

        fromWaypointIndex %= globalWaypoints.Length;
        int toWaypointIndex = (fromWaypointIndex + 1) % globalWaypoints.Length;
        float distanceBetweenWaypoints = Vector2.Distance(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex]);
        percentBetweenWaypoints += Mathf.Clamp01(Time.deltaTime * speed / distanceBetweenWaypoints);
        float easedPercentBetweenWaypoints = Ease(percentBetweenWaypoints);

        Vector2 newPosition = Vector2.Lerp(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex], easedPercentBetweenWaypoints);

        if (percentBetweenWaypoints >= 1)
        {
            percentBetweenWaypoints = 0;
            fromWaypointIndex++;

            if (!cyclic)
            {
                if (fromWaypointIndex >= globalWaypoints.Length - 1)
                {
                    fromWaypointIndex = 0;
                    System.Array.Reverse(globalWaypoints);
                }
            }
            nextMoveTime = Time.time + waitTime;
        }

        return newPosition - new Vector2(transform.position.x, transform.position.y);
    }

    private float Ease(float x)
    {
        float a = easeAmount + 1;
        return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
    }

    void MovePassengers(bool beforeMovePlatform)
    {
        foreach(PassengerMovement passenger in passengerMovements)
        {
            if (!passengerDictionary.ContainsKey(passenger.transform))
            {
                passengerDictionary.Add(passenger.transform, passenger.transform.GetComponent<Controller2D>());
            }

            if (passenger.moveBeforePlatform == beforeMovePlatform)
            {
                passengerDictionary[passenger.transform].Move(passenger.velocity, passenger.standingOnPlatform);
            }
        }
    }

    void CalculatePassengerMovement(Vector2 velocity)
    {
        HashSet<Transform> movedPassengers = new HashSet<Transform>();
        passengerMovements = new List<PassengerMovement>();

        float directionX = Mathf.Sign(velocity.x);
        float directionY = Mathf.Sign(velocity.y);

        // Vertically moving platform
        if (velocity.y != 0)
        {
            float rayLength = Mathf.Abs(velocity.y) + skinWidth;

            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
                rayOrigin += Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, passengerMask);

                /*
                 * hit.distance != 0 -> prevents platform from affect player when player is inside
                 */
                if (hit && hit.distance != 0)
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);
                        float pushX = (directionY == 1) ? velocity.x : 0;
                        float pushY = velocity.y - (hit.distance - skinWidth) * directionY;
                        //hit.transform.Translate(new Vector2(pushX, pushY));
                        passengerMovements.Add(new PassengerMovement(hit.transform, new Vector2(pushX, pushY),
                            directionY == 1, true));
                    }
                }
            }
        }

        // Horizontally moving platform
        if (velocity.x != 0)
        {
            float rayLength = Mathf.Abs(velocity.x) + skinWidth;

            for (int i = 0; i < horizontalRayCount; i++)
            {
                Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
                rayOrigin += Vector2.up * (horizontalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, passengerMask);

                /*
                 * hit.distance != 0 -> prevents platform from affect player when player is inside
                 */
                if (hit && hit.distance != 0)
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);
                        float pushX = velocity.x - (hit.distance - skinWidth) * directionX;
                        float pushY = -skinWidth;

                        passengerMovements.Add(new PassengerMovement(hit.transform, new Vector2(pushX, pushY),
                                                    false, true));
                    }
                }
            }
        }

        // Passenger on top of a horizontally or downward moving platform
        if (directionY == -1 || velocity.y == 0 && velocity.x != 0)
        {
            float rayLength = skinWidth * 2;

            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = raycastOrigins.topLeft + Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, passengerMask);

                /*
                 * hit.distance != 0 -> prevents platform from affect player when player is inside
                 */
                if (hit && hit.distance != 0)
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);
                        float pushX = velocity.x;
                        float pushY = velocity.y;

                        passengerMovements.Add(new PassengerMovement(hit.transform, new Vector2(pushX, pushY),
                                                    true, false));
                    }
                }
            }
        }
    }

    struct PassengerMovement
    {
        public Transform transform;
        public Vector2 velocity;
        public bool standingOnPlatform;
        public bool moveBeforePlatform;

        public PassengerMovement(Transform transform, Vector2 velocity, bool standingOnPlatform, bool moveBeforePlatform)
        {
            this.transform = transform;
            this.velocity = velocity;
            this.standingOnPlatform = standingOnPlatform;
            this.moveBeforePlatform = moveBeforePlatform;
        }
    }

    private void OnDrawGizmos()
    {
        if (localWaypoints != null)
        {
            Gizmos.color = Color.red;
            float size = 0.3f;

            for (int i = 0; i < localWaypoints.Length; i++)
            {
                Vector2 globalWaypointPos = Application.isPlaying ? globalWaypoints[i] : localWaypoints[i] + new Vector2(transform.position.x, transform.position.y);
                Gizmos.DrawLine(globalWaypointPos - Vector2.up * size, globalWaypointPos + Vector2.up * size);
                Gizmos.DrawLine(globalWaypointPos - Vector2.left * size, globalWaypointPos + Vector2.left * size);
            }
        }
    }
}
