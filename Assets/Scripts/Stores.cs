using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Stores : MonoBehaviour
{
    public Vector2 playerDestination;
    public Vector2 cameraDestination;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (InputController.Instance.DirectionalInput.y > 0.5f)
        {
            //Conversation.Instance.ShowConversation();
            Shop.Instance.ShowConversation();
            //Shop.Instance.ShowStoreBox();
        }
    }
}