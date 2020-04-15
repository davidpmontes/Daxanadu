using UnityEngine;

public class InputController : MonoBehaviour
{
    public static InputController Instance;

    private float horizontal;
    private float vertical;

    public Vector2 DirectionalInput;
    public bool isJumpStart;
    public bool isJumpStop;
    public bool isCancel;
    public bool isSpaceDown;

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
        DirectionalInput = new Vector2(horizontal, vertical);

        isSpaceDown = Input.GetKey(KeyCode.Space);
        isJumpStart = Input.GetKeyDown(KeyCode.Space);
        isJumpStop = Input.GetKeyUp(KeyCode.Space);
        isCancel = Input.GetKey(KeyCode.Backspace);
    }
}
