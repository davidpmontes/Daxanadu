using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Weapon : MonoBehaviour
{
    private void Update()
    {
        transform.position = Player.Instance.transform.position;
        transform.localScale = new Vector3(Player.Instance.spriteRenderer.flipX ? -1 : 1, 1, 1);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "greenSlime")
        {
            collision.gameObject.GetComponent<GreenSlime>().OnReceiveDamage(transform.position);
        }
    }
}
