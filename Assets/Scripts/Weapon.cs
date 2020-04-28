using UnityEngine;

public class Weapon : MonoBehaviour
{
    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        transform.position = Player.Instance.transform.position;
        transform.localScale = Player.Instance.transform.localScale;
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.name == "greenSlime")
    //    {
    //        //collision.gameObject.GetComponent<GreenSlime>().OnReceiveDamage(transform.position);
    //    }
    //}
}
