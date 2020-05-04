using UnityEngine;

public class InputController : MonoBehaviour
{
    public static InputController Instance;

    private float horizontal;
    private float vertical;

    private bool horizontalReleased;
    private bool verticalReleased;

    public bool onDown_Down;
    public bool onUp_Down;
    public bool onLeft_Down;
    public bool onRight_Down;

    public Vector2 DirectionalInput;

    public bool onActionA_Down;
    public bool onActionB_Down;
    public bool onActionX_Down;
    public bool onActionY_Down;

    public bool onActionA_Up;
    public bool onActionB_Up;
    public bool onActionX_Up;
    public bool onActionY_Up;

    public bool actionA;
    public bool actionB;
    public bool actionX;
    public bool actionY;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        GetInput();
    }

    /*
     *       UP                                     ACTION_X
     *
     * LEFT      RIGHT      SELECT START      ACTION_Y   ACTION_A
     *
     *      DOWN                                    ACTION_B
     */

    private void GetInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if (Mathf.Approximately(horizontal, 0f)) horizontalReleased = true;
        if (Mathf.Approximately(vertical, 0f)) verticalReleased = true;

        onUp_Down = onDown_Down = onLeft_Down = onRight_Down = false;

        if (horizontalReleased)
        {
            if (Mathf.Approximately(horizontal, 1))
            {
                onRight_Down = true;
                horizontalReleased = false;
            }
            if (Mathf.Approximately(horizontal, -1))
            {
                onLeft_Down = true;
                horizontalReleased = false;
            }
        }

        if (verticalReleased)
        {
            if (Mathf.Approximately(vertical, 1))
            {
                onUp_Down = true;
                verticalReleased = false;
            }
            if (Mathf.Approximately(vertical, -1))
            {
                onDown_Down = true;
                verticalReleased = false;
            }
        }

        DirectionalInput = new Vector2(horizontal, vertical);

        onActionA_Down = Input.GetKeyDown(KeyCode.J);
        onActionB_Down = Input.GetKeyDown(KeyCode.N);
        onActionX_Down = Input.GetKeyDown(KeyCode.U);
        onActionY_Down = Input.GetKeyDown(KeyCode.H);

        onActionA_Up = Input.GetKeyUp(KeyCode.J);
        onActionB_Up = Input.GetKeyUp(KeyCode.N);
        onActionX_Up = Input.GetKeyUp(KeyCode.U);
        onActionY_Up = Input.GetKeyUp(KeyCode.H);

        actionA = Input.GetKey(KeyCode.J);
        actionB = Input.GetKey(KeyCode.N);
        actionX = Input.GetKey(KeyCode.U);
        actionY = Input.GetKey(KeyCode.H);
    }
}
