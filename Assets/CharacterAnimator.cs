using System.Collections;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] private Sprite[] walking_body;
    [SerializeField] private Sprite[] walking_weapon;

    [SerializeField] private Sprite[] attacking_body;
    [SerializeField] private Sprite[] attacking_weapon;

    [SerializeField] private SpriteRenderer body;
    [SerializeField] private SpriteRenderer weapon;

    private float frameRate = 0.2f;

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
            body.flipX = false;
            weapon.flipX = false;
        }
        else if (directionalInput.x < 0)
        {
            body.flipX = true;
            weapon.flipX = true;
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
            yield return new WaitForSeconds(frameRate);
        }
    }

    private IEnumerator AttackingFrames()
    {
        int frame = 0;
        while (frame < 3)
        {
            body.sprite = attacking_body[frame];
            weapon.sprite = attacking_weapon[frame];
            frame += 1;
            yield return new WaitForSeconds(frameRate);
        }
        StartCoroutine(WalkingFrames());
    }
}
