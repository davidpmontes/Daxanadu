using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] private Sprite[] walking_body;
    [SerializeField] private Sprite[] walking_weapon;

    [SerializeField] private Sprite[] attacking_body;
    [SerializeField] private Sprite[] attacking_weapon_knife;

    [SerializeField] private SpriteRenderer body;
    [SerializeField] private SpriteRenderer weapon;

    private float frameRateWalking = 0.2f;
    private float frameRateAttacking = 0.1f;

    public UnityEvent OnAttackStartFrame;
    public UnityEvent OnAttackEndFrame;

    private void Start()
    {
        StartCoroutine(WalkingFrames());
    }

    public void OnAttack()
    {
        StopAllCoroutines();
        StartCoroutine(AttackingFrames());
    }

    public void UpdateAnimations(Vector2 directionalInput)
    {
        if (directionalInput.x > 0)
        {
            transform.localScale = new Vector3(1, 1);
        }
        else if (directionalInput.x < 0)
        {
            transform.localScale = new Vector3(-1, 1);
        }
    }

    public void SetSpriteRendererVisibility(bool value)
    {
        body.enabled = value;
        weapon.enabled = value;
    }

    public void ToggleSpriteRendererVisibility()
    {
        var value = body.enabled;
        body.enabled = !value;
        weapon.enabled = !value;
    }

    private IEnumerator WalkingFrames()
    {
        int frame = 0;
        while(true)
        {
            body.sprite = walking_body[frame];
            weapon.sprite = walking_weapon[frame];
            frame = (frame + 1) % 4;
            yield return new WaitForSeconds(frameRateWalking);
        }
    }

    private IEnumerator AttackingFrames()
    {
        int frame = 0;
        while (frame < 3)
        {
            if (frame == 2)
            {
                OnAttackStartFrame.Invoke();
            }

            body.sprite = attacking_body[frame];
            weapon.sprite = attacking_weapon_knife[frame];
            frame += 1;
            yield return new WaitForSeconds(frameRateAttacking);
        }
        OnAttackEndFrame.Invoke();
        StartCoroutine(WalkingFrames());
    }
}
