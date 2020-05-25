using DarkTonic.MasterAudio;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class GreenSlime : MonoBehaviour
{
    [SerializeField]
    private bool gravityEnabled = default;

    [SerializeField]
    private bool flyEnabled = default;

    [Header("Ground Jump Settings")]

    [SerializeField]
    [Tooltip("Horizontal controls enabled while airborne.")]
    private bool AirControlEnabled = default;

    [SerializeField]
    [Tooltip("Max height possible by holding down jump button.")]
    private float maxJumpHeight = default;

    [SerializeField]
    [Tooltip("Min height possible when tapping and releasing jump button.")]
    private float minJumpHeight = default;

    [SerializeField]
    [Tooltip("Time it takes to reach jump height.")]
    private float timeToJumpApex = default;

    [SerializeField]
    [Tooltip("Movement acceleration when airborne.")]
    float accelerationTimeAirborne = default;

    [Header("Grounded Settings")]
    [SerializeField]
    [Tooltip("Movement acceleration when grounded.")]
    private float accelerationTimeGrounded = default;

    [SerializeField]
    [Tooltip("Maximum movement speed on the ground.")]
    private float moveSpeed = default;

    [Header("Wall jumps enabled.")]
    [SerializeField]
    private bool wallJumpsEnabled = default;

    [SerializeField]
    [Tooltip("Initial velocity vector when against a wall, " +
        "you are pressing toward the wall, and you jump.")]
    private Vector2 wallJumpClimb = default;

    [SerializeField]
    [Tooltip("Initial velocity vector when against a wall, " +
    "you are not pressing a direction, and you jump.")]
    private Vector2 wallJumpOff = default;

    [SerializeField]
    [Tooltip("Initial velocity vector when against a wall, " +
        "you are pressing away from the wall, and you jump.")]
    private Vector2 wallLeap = default;

    [SerializeField]
    [Tooltip("How fast you slide down a wall.")]
    private float wallSlideSpeedMax = default;

    [SerializeField]
    [Tooltip("How long you remain attached to a wall when " +
        "trying to move away.")]
    private float wallStickTime = default;

    private Controller2D controller;
    public SpriteRenderer spriteRenderer;
    private Animator animator;
    private Vector2 velocity;
    private float timeToWallUnstick;
    private float gravity;
    private float maxJumpVelocity;
    private float minJumpVelocity;
    private float velocityXSmoothing;
    private float velocityYSmoothing;
    private bool wallSliding;
    private int wallDirX;
    private Vector2 directionalInput;
    private float NextActionTime;

    private void Awake()
    {
        controller = GetComponent<Controller2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        gravity = -2 * maxJumpHeight / Mathf.Pow(timeToJumpApex, 2);
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
    }

    private void Update()
    {
        GetInput();
        CalculateVelocity();
        Move();
    }

    private void GetInput()
    {
        if (NextActionTime < Time.time)
        {
            directionalInput = new Vector2(Player.Instance.transform.position.x > transform.position.x ? 1 : -1, 0);
            OnJumpInputDown();
            NextActionTime = Time.time + 2;
        }
    }

    private void CalculateVelocity()
    {
        float targetVelocityX = directionalInput.x * moveSpeed;
        

        if (controller.collisions.below)
        {
            velocity.x = targetVelocityX;
        }
        
        if (gravityEnabled)
            velocity.y += gravity * Time.deltaTime;
    }

    public void OnJumpInputDown()
    {
        if (controller.collisions.below)
        {
            velocity.y = maxJumpVelocity;
        }
    }

    private void Move()
    {
        controller.Move(velocity * Time.deltaTime, directionalInput);

        /*
         * NOTE: Important that the "controller.move" is done prior to below if statement
         * 
         */
        if (controller.collisions.above || controller.collisions.below)
        {
            if (controller.collisions.slidingDownMaxSlope)
                velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
            else
                velocity.y = 0;
        }
    }
}
