using UnityEngine;

public class MagicProjectile : MonoBehaviour
{
    private int direction = 1;
    private float speed = 5;
    public float lifespan;

    private void Start()
    {
        transform.position = Player.Instance.transform.position;
        transform.localScale = Player.Instance.transform.localScale;
        direction = (int)Player.Instance.transform.localScale.x;
        lifespan = 2;
        Debug.Log(direction);
    }

    private void Update()
    {
        transform.Translate(new Vector2(direction * speed * Time.deltaTime, 0));
        lifespan -= Time.deltaTime;
        if (lifespan <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "greenSlime")
        {
            collision.gameObject.GetComponent<GreenSlime>().OnReceiveDamage(transform.position);
            Destroy(gameObject);
        }
    }
}
