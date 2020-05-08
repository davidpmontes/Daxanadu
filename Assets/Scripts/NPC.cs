using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
public class NPC : MonoBehaviour
{
    private GameObject conversationInstance;
    //20 characters max per line, excluding > and ^
    private string[] conversation = new string[] {
        "Glad you could^-->",
        "come.^-->",
        "Disaster has^-->",
        "befallen us.^NXT",
        "The Elf^-->",
        "fountain water,^-->",
        "our life source,^-->",
        "has stopped.^NXT",
        "The wells are^-->",
        "drying up.^NXT",
        "Many men went^-->",
        "out and nobody^-->",
        "came back.^NXT",
        "You are our last^-->",
        "hope.^-->",
        "I shall give you^-->",
        "1500 Golds.^NXT",
        "Prepare^-->",
        "for your journey^-->",
        "with this money.^NXT",
        "It will be a^-->",
        "dangerous^-->",
        "journey.^NXT",
        "Take care^-->",
        "of yourself.^END"
    };

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
    private Animator animator;

    public void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        globalWaypoints = new Vector2[localWaypoints.Length];
        for (int i = 0; i < localWaypoints.Length; i++)
        {
            globalWaypoints[i] = localWaypoints[i] + new Vector2(
                                                        transform.position.x,
                                                        transform.position.y);
        }
    }

    private void Update()
    {
        velocity = CalculatePlatformMovement();
        transform.Translate(velocity);
        FlipSprite();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        ConversationStart();
    }

    private void ConversationStart()
    {
        if (InputController.Instance.DirectionalInput.y > 0.5f)
        {
            if (!animator.enabled)
                return;

            animator.enabled = false;
            enabled = false;

            conversationInstance = TextPool.Instance.GetFromPoolInactive(TextPool.TextPools.ScrollingConversation);
            conversationInstance.SetActive(true);
            conversationInstance.GetComponent<Conversation>().ShowConversation(
                LandscapeContainer.Instance.GetCursorStartPosition(),
                LandscapeContainer.Instance.GetCaretNextPosition(),
                LandscapeContainer.Instance.GetCaretFinishPosition(),
                conversation);

            Player.Instance.GetComponent<Player>().Pause();
            PortraitContainer.Instance.ShowFrame();
            LandscapeContainer.Instance.Show();

            conversationInstance.GetComponent<Conversation>().finished += ConversationEnd;
            conversationInstance.GetComponent<Conversation>().canceled += ConversationEnd;

        }
    }

    private void ConversationEnd()
    {
        conversationInstance.GetComponent<Conversation>().finished -= ConversationEnd;
        conversationInstance.GetComponent<Conversation>().canceled -= ConversationEnd;
        conversationInstance.GetComponent<Conversation>().Hide();
        TextPool.Instance.DeactivateAndAddToPool(conversationInstance);

        Player.Instance.GetComponent<Player>().Unpause();
        PortraitContainer.Instance.HideFrame();
        LandscapeContainer.Instance.Hide();
        enabled = true;
        animator.enabled = true;
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