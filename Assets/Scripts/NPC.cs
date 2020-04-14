using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class NPC : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Waypoints that are defined relative to the platform.")]
    private Vector2[] localWaypoints;

    [SerializeField]
    [Tooltip("Max speed.")]
    private float speed;

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
    private Vector2 velocity;
    private SpriteRenderer spriteRenderer;

    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        globalWaypoints = new Vector2[localWaypoints.Length];
        for (int i = 0; i < localWaypoints.Length; i++)
        {
            globalWaypoints[i] = localWaypoints[i] + new Vector2(transform.position.x, transform.position.y);
        }
    }

    private void Update()
    {
        velocity = CalculatePlatformMovement();
        transform.Translate(velocity);
        FlipSprite();
    }

    private void FlipSprite()
    {
        spriteRenderer.flipX = velocity.x <= 0;
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

            if (fromWaypointIndex >= globalWaypoints.Length - 1)
            {
                fromWaypointIndex = 0;
                System.Array.Reverse(globalWaypoints);
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
