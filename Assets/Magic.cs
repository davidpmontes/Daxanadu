using UnityEngine;

public class Magic : MonoBehaviour
{
    private int direction = 1;
    private float speed = 5;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        direction = (int)Player.Instance.transform.localScale.x;
        transform.position = Player.Instance.transform.position;
        transform.localScale = Player.Instance.transform.localScale;
        Invoke("DeactivateAfterTime", 3);
    }

    private void Update()
    {
        transform.Translate(new Vector2(direction * speed * Time.deltaTime, 0));
    }

    private void DeactivateAfterTime()
    {
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("magic collide");
        if (collision.gameObject.name == "greenSlime")
        {
            collision.gameObject.GetComponent<GreenSlime>().OnReceiveDamage(transform.position);
            CancelInvoke("DeactivateAfterTime");
            gameObject.SetActive(false);
        }
    }
}
