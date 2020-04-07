using UnityEngine;

[RequireComponent (typeof (Controller2D))]
[RequireComponent (typeof (SpriteRenderer))]
[RequireComponent (typeof (Animator))]
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField]
    private bool gravityEnabled;

    [SerializeField]
    private bool flyEnabled;

    [Header("Ground Jump Settings")]

    [SerializeField]
    [Tooltip("Horizontal controls enabled while airborne.")]
    private bool AirControlEnabled;

    [SerializeField]
    [Tooltip("Max height possible by holding down jump button.")]
    private float maxJumpHeight = 4;

    [SerializeField]
    [Tooltip("Min height possible when tapping and releasing jump button.")]
    private float minJumpHeight = 1;

    [SerializeField]
    [Tooltip("Time it takes to reach jump height.")]
    private float timeToJumpApex = 0.4f;

    [SerializeField]
    [Tooltip("Movement acceleration when airborne.")]
    float accelerationTimeAirborne = .2f;

    [Header("Grounded Settings")]
    [SerializeField]
    [Tooltip("Movement acceleration when grounded.")]
    private float accelerationTimeGrounded = .1f;

    [SerializeField]
    [Tooltip("Maximum movement speed on the ground.")]
    private float moveSpeed = 6;

    [Header("Wall jumps enabled.")]
    [SerializeField]
    private bool wallJumpsEnabled;

    [SerializeField]
    [Tooltip("Initial velocity vector when against a wall, " +
        "you are pressing toward the wall, and you jump.")]
    private Vector2 wallJumpClimb = new Vector2(7.5f, 16f);

    [SerializeField]
    [Tooltip("Initial velocity vector when against a wall, " +
    "you are not pressing a direction, and you jump.")]
    private Vector2 wallJumpOff = new Vector2(8.5f, 7f);

    [SerializeField]
    [Tooltip("Initial velocity vector when against a wall, " +
        "you are pressing away from the wall, and you jump.")]
    private Vector2 wallLeap = new Vector2(18, 17);

    [SerializeField]
    [Tooltip("How fast you slide down a wall.")]
    private float wallSlideSpeedMax = 3;

    [SerializeField]
    [Tooltip("How long you remain attached to a wall when " +
        "trying to move away.")]
    private  float wallStickTime = 0.25f;

    private Controller2D controller;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Vector2 velocity;
    private Vector2 directionalInput;
    private float timeToWallUnstick;
    private float gravity;
    private float maxJumpVelocity;
    private float minJumpVelocity;
    private float velocityXSmoothing;
    private float velocityYSmoothing;
    private bool wallSliding;
    private int wallDirX;

    public float horizontal;
    public float vertical;

    private void Awake()
    {
        Instance = this;
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
        UpdateAnimations();
        CalculateVelocity();
        if (wallJumpsEnabled) HandleWallSliding();

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

    private void CalculateVelocity()
    {
        float targetVelocityX = directionalInput.x * moveSpeed;
        float targetVelocityY = directionalInput.y * moveSpeed;


        if (AirControlEnabled)
        {
            velocity.x = Mathf.SmoothDamp(velocity.x,
                                          targetVelocityX,
                                          ref velocityXSmoothing,
                                          controller.collisions.below ?
                                                accelerationTimeGrounded * controller.collisions.frictionFactor :
                                                accelerationTimeAirborne);
        }
        else
        {
            if (controller.collisions.below)
            {
                velocity.x = Mathf.SmoothDamp(velocity.x,
                                              targetVelocityX,
                                              ref velocityXSmoothing,
                                              accelerationTimeGrounded*
                                                    controller.collisions.frictionFactor);
            }
        }

        if (flyEnabled)
        {
            velocity.y = Mathf.SmoothDamp(velocity.y,
                              targetVelocityY,
                              ref velocityYSmoothing,
                              controller.collisions.below ?
                                    accelerationTimeGrounded * controller.collisions.frictionFactor :
                                    accelerationTimeAirborne);
        }

        if (gravityEnabled)
            velocity.y += gravity * Time.deltaTime;
    }

    private void HandleWallSliding()
    {
        // detects if the wall is on the left or right side
        wallDirX = controller.collisions.left ? -1 : 1;
        wallSliding = false;

        /*
         * Conditions for wall slide:
         *   Must have a wall on the immediate left or right AND
         *   Cannot have a collider below AND
         *   Must be actively moving downwards
         */
        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
        {
            wallSliding = true;

            velocity.y = Mathf.Max(velocity.y, -wallSlideSpeedMax);

            // Only unstick if moving away from the wall for long enough
            if (timeToWallUnstick > 0)
            {
                velocityXSmoothing = 0;
                velocity.x = 0;

                // if the player is actively trying to move away from the wall
                if (directionalInput.x != wallDirX && directionalInput.x != 0)
                {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            }
            else
            {
                timeToWallUnstick = wallStickTime;
            }
        }
    }

    private void GetInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        directionalInput = new Vector2(horizontal, vertical);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJumpInputDown();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            OnJumpInputUp();
        }
    }

    private void UpdateAnimations()
    {
        if (horizontal > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (horizontal < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    public void OnJumpInputDown()
    {
        if (wallSliding)
        {
            if (wallDirX == directionalInput.x)
            {
                velocity.x = -wallDirX * wallJumpClimb.x;
                velocity.y = wallJumpClimb.y;
            }
            else if (directionalInput.x == 0)
            {
                velocity.x = -wallDirX * wallJumpOff.x;
                velocity.y = wallJumpOff.y;
            }
            else
            {
                velocity.x = -wallDirX * wallLeap.x;
                velocity.y = wallLeap.y;
            }
        }

        if (controller.collisions.below)
        {
            if (controller.collisions.slidingDownMaxSlope)
            {
                /*
                 * Not jumping while on a max slope
                 * 
                 */
                if (directionalInput.x != -Mathf.Sign(controller.collisions.slopeNormal.x))
                {
                    velocity.y = maxJumpVelocity * controller.collisions.slopeNormal.y;
                    velocity.x = maxJumpVelocity * controller.collisions.slopeNormal.x;
                }
            } else {
                velocity.y = maxJumpVelocity;
            }
        }
    }

    public void OnJumpInputUp()
    {
        velocity.y = Mathf.Min(velocity.y, minJumpVelocity);
    }

    public void PauseForTransitions()
    {
        enabled = false;
        animator.enabled = false;
    }

    public void UnpauseForTransitions()
    {
        enabled = true;
        animator.enabled = true;
    }
}