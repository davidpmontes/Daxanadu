// *** BACK ***
// Sorting Layer: 
//
// Sorting Layer: Background objects
// 0: Sky
// 10: Very Far background: distant mountains
// 20: Background Buildings
// 30: Background Trees
// 40: Physical Ground Walls and platforms
//
// Sorting Layer: MovingObjects: Characters, Enemies, Magic, Weapons, SFX
// 0: All
//
// Sorting Layer: Foreground objects: Foreground Trees, wells
// 0: All
//
// Sorting Layer: Menus Background: Black backgrounds, Frames
// 0: Frames and Backdrops
//
// Sorting Layer: Menus Foreground
// ***spritemasks back -1 -> 1 front
//
// 0: Images main, Words, Graphics, cursors
// 1: Images accessories
//
// Sorting Layer: BlackFade
//
// *** FRONT ***

using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using DarkTonic.MasterAudio;

[RequireComponent(typeof(CharacterAnimator))]
[RequireComponent(typeof(MovingObject))]
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField]
    private MovingObjectConfig config;

    [SoundGroupAttribute] public string damageSound;
    [SoundGroupAttribute] public string magicSound;

    [SerializeField]
    private GameObject Weapon;

    private MovingObject movingObject;

    private CharacterAnimator characterAnimator;
    private Controller2D controller;
    private Vector2 velocity;
    private Vector2 inputDirection;
    private bool isInvincible;
    private float maxLife = 100;
    private float currLife = 100;
    private float maxMagic = 100;
    private float currMagic = 100;

    public UnityEvent OnAttackEvent;
    public UnityEvent OnDamageReceiveEvent;
    public UnityEvent OnMagicUsedEvent;

    private void Awake()
    {
        Instance = this;
        characterAnimator = GetComponent<CharacterAnimator>();
        controller = GetComponent<Controller2D>();
        movingObject = GetComponent<MovingObject>();
        movingObject.AssignConfiguration(config);
        movingObject.CalculateMovementVariables();
    }

    private void Update()
    {
        GetInput();
        Move();
        characterAnimator.UpdateAnimations(inputDirection);

        movingObject.CalculateVelocity();
        movingObject.Move();
    }

    private void GetInput()
    {
        inputDirection = InputController.Instance.DirectionalInput;
        //if (InputController.Instance.onActionCancel_Down) OnJumpInputDown();
        //if (InputController.Instance.onActionCancel_Up) OnJumpInputUp();
        //if (InputController.Instance.onActionPrimary_Down) OnAttack();
        //if (InputController.Instance.onActionSecondary_Down) OnMagicUsed();
    }

    private void Move()
    {
        movingObject.SetDirectionalInput(inputDirection);
    }

    public void OnJumpInputUp()
    {
        velocity.y = Mathf.Min(velocity.y, movingObject.GetMinJumpVelocity());
    }

    private void OnAttack()
    {
        OnAttackEvent.Invoke();
    }

    public void Pause()
    {
        enabled = false;
        characterAnimator.OnPause();
    }

    public void Unpause()
    {
        enabled = true;
        characterAnimator.OnUnpause();
    }

    public float GetLifePercentage()
    {
        return currLife / maxLife;
    }

    public float GetMagicPercentage()
    {
        return currMagic / maxMagic;
    }

    public void OnReceiveDamage(Vector2 enemyPosition)
    {
        if (isInvincible)
            return;

        currLife -= 5;
        velocity.x = Mathf.Sign(transform.position.x - enemyPosition.x) * 15;
        StartCoroutine(TemporaryInvincible());
        MasterAudio.PlaySoundAndForget(damageSound);
        OnDamageReceiveEvent.Invoke();
    }

    public void OnMagicUsed()
    {
        if (currMagic < 5)
            return;

        MasterAudio.PlaySoundAndForget(magicSound);
        currMagic -= 5;
        OnMagicUsedEvent.Invoke();
    }

    IEnumerator TemporaryInvincible()
    {
        float startTime = Time.time + 1;
        isInvincible = true;
        while (Time.time < startTime)
        {
            //spriteRenderer.enabled = !spriteRenderer.enabled;
            characterAnimator.ToggleSpriteRendererVisibility();
            yield return new WaitForSeconds(0.1f);
        }
        isInvincible = false;
        //spriteRenderer.enabled = true;
        characterAnimator.SetSpriteRendererVisibility(true);
    }
}