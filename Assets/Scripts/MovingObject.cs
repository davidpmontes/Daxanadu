using DarkTonic.MasterAudio;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Controller2D))]
[RequireComponent(typeof(Rigidbody2D))]

public class MovingObject : MonoBehaviour
{
    private MovingObjectConfig config;
    private Controller2D controller;
    private SpriteRenderer spriteRenderer;
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

    private void Awake()
    {
        controller = GetComponent<Controller2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void AssignConfiguration(MovingObjectConfig newConfig)
    {
        config = newConfig;
    }

    public void CalculateMovementVariables()
    {
        gravity = -2 * config.maxJumpHeight / Mathf.Pow(config.timeToJumpApex, 2);
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * config.minJumpHeight);
        maxJumpVelocity = Mathf.Abs(gravity) * config.timeToJumpApex;
    }

    public Vector2 GetDirectionalInput() => directionalInput;
    public float GetMaxJumpVelocity() => maxJumpVelocity;
    public void SetDirectionalInput(Vector2 newDirectionalInput)
    {
        directionalInput = newDirectionalInput;
    }
    public void SetDirectionalInputX(float x)
    {
        directionalInput.x = x;
    }
    public void SetDirectionalInputY(float y)
    {
        directionalInput.y = y;
    }
    public void Jump()
    {
        velocity.y = maxJumpVelocity;
    }
    public void SetSpriteRendererFlipX(bool value)
    {
        spriteRenderer.flipX = value;
    }

    public void CalculateVelocity()
    {
        float targetVelocityX = directionalInput.x * config.moveSpeed;
        float targetVelocityY = directionalInput.y * config.moveSpeed;

        if (config.AirControlEnabled)
        {
            velocity.x = Mathf.SmoothDamp(velocity.x,
                                          targetVelocityX,
                                          ref velocityXSmoothing,
                                          controller.collisions.below ?
                                                config.accelerationTimeGrounded * controller.collisions.frictionFactor :
                                                config.accelerationTimeAirborne);
        }
        else
        {
            if (controller.collisions.below)
            {
                velocity.x = Mathf.SmoothDamp(velocity.x,
                                              targetVelocityX,
                                              ref velocityXSmoothing,
                                              config.accelerationTimeGrounded *
                                                    controller.collisions.frictionFactor);
            }
        }

        if (config.flyEnabled)
        {
            velocity.x = Mathf.SmoothDamp(velocity.x,
                              targetVelocityX,
                              ref velocityXSmoothing,
                              controller.collisions.below ?
                                    config.accelerationTimeGrounded * controller.collisions.frictionFactor :
                                    config.accelerationTimeAirborne);

            velocity.y = Mathf.SmoothDamp(velocity.y,
                              targetVelocityY,
                              ref velocityYSmoothing,
                              controller.collisions.below ?
                                    config.accelerationTimeGrounded * controller.collisions.frictionFactor :
                                    config.accelerationTimeAirborne);
        }

        if (config.gravityEnabled)
            velocity.y += gravity * Time.deltaTime;
    }

    public void Move()
    {
        if (config.wallJumpsEnabled) HandleWallSliding();

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

            velocity.y = Mathf.Max(velocity.y, -config.wallSlideSpeedMax);

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
                    timeToWallUnstick = config.wallStickTime;
                }
            }
            else
            {
                timeToWallUnstick = config.wallStickTime;
            }
        }
    }
}
