using UnityEngine;
using DarkTonic.MasterAudio;
using System.Collections;

public class Coin : MonoBehaviour
{
    [SoundGroupAttribute] public string coinSound;
    [SerializeField] private Animator animator;
    public LayerMask collisionMask;

    public void Initialize(Vector3 position)
    {
        animator.enabled = false;
        transform.position = position;
        StartCoroutine(FindGround());
    }

    IEnumerator FindGround()
    {
        float velocity = 0;
        while (true)
        {
            var hit = Physics2D.Raycast(transform.position, Vector2.down, 100, collisionMask);
            if (hit)
            {
                if (transform.position.y - hit.point.y > 0)
                {
                    if ((transform.position + Vector3.down * velocity).y - hit.point.y > 0)
                    {
                        transform.position += Vector3.down * velocity;
                        velocity += Time.deltaTime * 0.5f;
                        yield return null;
                    }
                    else
                    {
                        transform.position = hit.point;
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }
        animator.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        MasterAudio.PlaySoundAndForget(coinSound);
        Destroy(gameObject);
    }
}
