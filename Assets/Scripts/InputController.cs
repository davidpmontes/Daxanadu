using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

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

    public bool onActionPrimary_Down;
    public bool onActionCancel_Down;
    public bool onActionSecondary_Down;
    public bool onActionY_Down;

    public bool onActionPrimary_Up;
    public bool onActionCancel_Up;
    public bool onActionSecondary_Up;
    public bool onActionY_Up;

    public bool actionPrimary;
    public bool actionCancel;
    public bool actionSecondary;
    public bool actionY;

    private Controls controls;
    

    private void Awake()
    {
        Instance = this;
        controls = new Controls();
        controls.Player.Enable();
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
        //horizontal = controls.Player.Move.ReadValue<Vector2>().x;
        //vertical = controls.Player.Move.ReadValue<Vector2>().y;

        horizontal = -Gamepad.current[UnityEngine.InputSystem.LowLevel.GamepadButton.DpadLeft].ReadValue() +
                     Gamepad.current[UnityEngine.InputSystem.LowLevel.GamepadButton.DpadRight].ReadValue();
        vertical = Gamepad.current[UnityEngine.InputSystem.LowLevel.GamepadButton.DpadUp].ReadValue() +
                   -Gamepad.current[UnityEngine.InputSystem.LowLevel.GamepadButton.DpadDown].ReadValue();

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

        if (onActionPrimary_Down)
            onActionPrimary_Down = false;
        if (onActionPrimary_Up)
            onActionPrimary_Up = false;

        onActionPrimary_Down = Gamepad.current[UnityEngine.InputSystem.LowLevel.GamepadButton.East].wasPressedThisFrame;
        onActionPrimary_Up = Gamepad.current[UnityEngine.InputSystem.LowLevel.GamepadButton.East].wasReleasedThisFrame;
        actionPrimary = Gamepad.current[UnityEngine.InputSystem.LowLevel.GamepadButton.East].isPressed;

        onActionCancel_Down = Gamepad.current[UnityEngine.InputSystem.LowLevel.GamepadButton.South].wasPressedThisFrame;
        onActionCancel_Up = Gamepad.current[UnityEngine.InputSystem.LowLevel.GamepadButton.South].wasReleasedThisFrame;
        actionCancel = Gamepad.current[UnityEngine.InputSystem.LowLevel.GamepadButton.South].isPressed;

        onActionSecondary_Down = Gamepad.current[UnityEngine.InputSystem.LowLevel.GamepadButton.North].wasPressedThisFrame;
        onActionSecondary_Up = Gamepad.current[UnityEngine.InputSystem.LowLevel.GamepadButton.North].wasPressedThisFrame;
        actionSecondary = Gamepad.current[UnityEngine.InputSystem.LowLevel.GamepadButton.North].isPressed;

        //onActionPrimary_Down = Input.GetKeyDown(KeyCode.J);
        //onActionCancel_Down = Input.GetKeyDown(KeyCode.N);
        //onActionX_Down = Input.GetKeyDown(KeyCode.U);
        //onActionY_Down = Input.GetKeyDown(KeyCode.H);

        //onActionPrimary_Up = Input.GetKeyUp(KeyCode.J);
        //onActionCancel_Up = Input.GetKeyUp(KeyCode.N);
        //onActionX_Up = Input.GetKeyUp(KeyCode.U);
        //onActionY_Up = Input.GetKeyUp(KeyCode.H);

        //actionCancel = controls.Player.ActionCancel.performed;
        //actionSecondary = controls.Player.ActionSecondary.ReadValue<bool>();

        //actionPrimary = Input.GetKey(KeyCode.J);
        //actionCancel = Input.GetKey(KeyCode.N);
        //actionX = Input.GetKey(KeyCode.U);
        //actionY = Input.GetKey(KeyCode.H);
    }
}