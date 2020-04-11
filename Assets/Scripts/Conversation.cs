using UnityEngine;

public class Conversation : MonoBehaviour
{
    public static Conversation Instance;

    [SerializeField] private GameObject portraitDialogueBox;
    [SerializeField] private GameObject landscapeDialogueBox;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowConversation()
    {
        portraitDialogueBox.SetActive(true);
        landscapeDialogueBox.SetActive(true);
    }

    public void HideConversation()
    {
        portraitDialogueBox.SetActive(false);
        landscapeDialogueBox.SetActive(false);
    }
}
