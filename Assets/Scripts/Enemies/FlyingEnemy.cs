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

    private float leftBoundary;
    private float rightBoundary;
    private float topBoundary;
    private float bottomBoundary;

    private float speed;

    private void Awake()
    {
        controller = GetComponent<Controller2D>();
        movingObject = GetComponent<MovingObject>();
        movingObject.AssignConfiguration(config);
        movingObject.CalculateMovementVariables();
    }

    private void Start()
    {
        leftBoundary = ViewSwitcher.Instance.transform.position.x - 7.5f;
        rightBoundary = ViewSwitcher.Instance.transform.position.x + 7.5f;
        topBoundary = ViewSwitcher.Instance.transform.position.y + 5.5f;
        bottomBoundary = ViewSwitcher.Instance.transform.position.y - 6f;

        inputDirection = new Vector2(-1, 1 / 3f);
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
        if (transform.position.x < leftBoundary)
        {
            inputDirection.x = 1;
        }
        if (transform.position.x > rightBoundary)
        {
            inputDirection.x = -1;
        }
        if (transform.position.y > topBoundary)
        {
            inputDirection.y = -1/3f;
        }
        if (transform.position.y < bottomBoundary)
        {
            inputDirection.y = 1/3f;
        }

        speed = Mathf.Max(0, Mathf.Clamp(Mathf.Sin(Time.time), -0.25f, 0.25f)) * 25;
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
        movingObject.SetDirectionalInput(inputDirection * speed);
    }
}
