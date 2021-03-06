﻿using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] private Sprite[] walking_body = default;
    [SerializeField] private Sprite[] walking_weapon = default;

    [SerializeField] private Sprite[] attacking_body = default;
    [SerializeField] private Sprite[] attacking_weapon_knife = default;

    [SerializeField] private SpriteRenderer body = default;
    [SerializeField] private SpriteRenderer weapon = default;

    private float frameRateWalking = 0.2f;
    private float frameRateAttacking = 0.1f;

    public UnityEvent OnAttackStartFrame;
    public UnityEvent OnAttackEndFrame;

    private void Start()
    {
        StartCoroutine(WalkingFrames());
    }

    public void OnPause()
    {
        StopAllCoroutines();
    }

    public void OnUnpause()
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
