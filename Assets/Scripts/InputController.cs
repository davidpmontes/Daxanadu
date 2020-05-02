using UnityEngine;

public class InputController : MonoBehaviour
{
    public static InputController Instance;

    private float horizontal;
    private float vertical;

    public bool onDown;
    public bool onUp;
    public bool onLeft;
    public bool onRight;

    public Vector2 DirectionalInput;
    public bool onAttackDown;
    public bool onJumpDown;
    public bool onMagicDown;
    public bool onSpaceDown;

    public bool onJumpUp;

    public bool isSpaceDown;
    public bool isCancel;

    private bool horizontalReleased;
    private bool verticalReleased;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if (Mathf.Approximately(horizontal, 0f)) horizontalReleased = true;
        if (Mathf.Approximately(vertical, 0f)) verticalReleased = true;

        onUp = onDown = onLeft = onRight = false;

        if (horizontalReleased)
        {
            if (Mathf.Approximately(horizontal, 1))
            {
                onRight = true;
                horizontalReleased = false;
            }
            if (Mathf.Approximately(horizontal, -1))
            {
                onLeft = true;
                horizontalReleased = false;
            }
        }

        if (verticalReleased)
        {
            if (Mathf.Approximately(vertical, 1))
            {
                onUp = true;
                verticalReleased = false;
            }
            if (Mathf.Approximately(vertical, -1))
            {
                onDown = true;
                verticalReleased = false;
            }
        }

        DirectionalInput = new Vector2(horizontal, vertical);

        onAttackDown = Input.GetKeyDown(KeyCode.J);
        onJumpDown = Input.GetKeyDown(KeyCode.Space);
        onMagicDown = Input.GetKeyDown(KeyCode.K);
        onSpaceDown = Input.GetKeyDown(KeyCode.Space);

        onJumpUp = Input.GetKeyUp(KeyCode.Space);

        isSpaceDown = Input.GetKey(KeyCode.Space);
        isCancel = Input.GetKey(KeyCode.Backspace);
    }
}
