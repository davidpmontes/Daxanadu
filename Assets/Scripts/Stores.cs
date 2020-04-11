using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Stores : MonoBehaviour
{
    private BoxCollider2D boxCollider2D;
    public Vector2 playerDestination;
    public Vector2 cameraDestination;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (Player.Instance.vertical > 0.5f)
        {
            Conversation.Instance.ShowConversation();
        }
    }
}