using UnityEngine;

public class InputController : MonoBehaviour
{
    public static InputController Instance;

    private float horizontal;
    private float vertical;

    public Vector2 DirectionalInput;
    public bool onAttackDown;
    public bool onJumpDown;
    public bool onMagicDown;
    public bool onSpaceDown;

    public bool onJumpUp;

    public bool isSpaceDown;
    public bool isCancel;


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

        onAttackDown = Input.GetKeyDown(KeyCode.J);
        onJumpDown = Input.GetKeyDown(KeyCode.Space);
        onMagicDown = Input.GetKeyDown(KeyCode.K);
        onSpaceDown = Input.GetKeyDown(KeyCode.Space);

        onJumpUp = Input.GetKeyUp(KeyCode.Space);

        isSpaceDown = Input.GetKey(KeyCode.Space);
        isCancel = Input.GetKey(KeyCode.Backspace);
    }
}
