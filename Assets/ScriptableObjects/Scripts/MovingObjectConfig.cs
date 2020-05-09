using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/MovingObjectConfig", order = 1)]
public class MovingObjectConfig : ScriptableObject
{
    public bool gravityEnabled = true;

    public bool flyEnabled = false;

    [Header("Ground Jump Settings")]

    [Tooltip("Horizontal controls enabled while airborne.")]
    public bool AirControlEnabled;

    [Tooltip("Max height possible by holding down jump button.")]
    public float maxJumpHeight = 4;

    [Tooltip("Min height possible when tapping and releasing jump button.")]
    public float minJumpHeight = 1;

    [Tooltip("Time it takes to reach jump height.")]
    public float timeToJumpApex = 0.4f;

    [Tooltip("Movement acceleration when airborne.")]
    public float accelerationTimeAirborne = .2f;

    [Header("Grounded Settings")]
    [Tooltip("Movement acceleration when grounded.")]
    public float accelerationTimeGrounded = .1f;

    [Tooltip("Maximum movement speed on the ground.")]
    public float moveSpeed;

    [Header("Wall jumps enabled.")]
    public bool wallJumpsEnabled;

    [Tooltip("Initial velocity vector when against a wall, " +
        "you are pressing toward the wall, and you jump.")]
    public Vector2 wallJumpClimb = new Vector2(7.5f, 16f);

    [Tooltip("Initial velocity vector when against a wall, " +
    "you are not pressing a direction, and you jump.")]
    public Vector2 wallJumpOff = new Vector2(8.5f, 7f);

    [Tooltip("Initial velocity vector when against a wall, " +
        "you are pressing away from the wall, and you jump.")]
    public Vector2 wallLeap = new Vector2(18, 17);

    [Tooltip("How fast you slide down a wall.")]
    public float wallSlideSpeedMax = 3;

    [Tooltip("How long you remain attached to a wall when " +
        "trying to move away.")]
    public float wallStickTime = 0.25f;
}
