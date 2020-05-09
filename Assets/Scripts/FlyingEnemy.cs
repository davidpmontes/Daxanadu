using UnityEngine;

[RequireComponent(typeof(MovingObject))]
public class FlyingEnemy : MonoBehaviour
{
    [SerializeField]
    private MovingObjectConfig config;

    private float NextActionTime;
    private MovingObject movingObject;
    private Controller2D controller;

    private Vector2 inputDirection;

    private void Awake()
    {
        controller = GetComponent<Controller2D>();
        movingObject = GetComponent<MovingObject>();
        movingObject.AssignConfiguration(config);
        movingObject.CalculateMovementVariables();
    }

    private void Update()
    {
        GetInput();
        Move();
        UpdateAnimations();

        movingObject.CalculateVelocity();
        movingObject.Move();
    }

    private void GetInput()
    {
        if (NextActionTime > Time.time)
            return;

        inputDirection = new Vector2(Player.Instance.transform.position.x > transform.position.x ? 1 : -1, 0);
    }

    private void UpdateAnimations()
    {
        if (movingObject.GetDirectionalInput().x > 0)
        {
            movingObject.SetSpriteRendererFlipX(false);
        }
        else if (movingObject.GetDirectionalInput().x < 0)
        {
            movingObject.SetSpriteRendererFlipX(true);
        }
    }

    private void Move()
    {
        if (NextActionTime > Time.time)
            return;

        NextActionTime = Time.time + 2;

        movingObject.SetDirectionalInput(inputDirection);
        if (controller.collisions.below)
        {
            movingObject.Jump();
        }
    }
}
