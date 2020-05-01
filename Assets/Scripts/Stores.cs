using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Stores : MonoBehaviour
{
    public Vector2 playerDestination;
    public Vector2 cameraDestination;

    private string[] greetingText = new string[] {
        "Welcome!^-->",
        "Welcome!^-->",
        "How may I help?^-->",
        "Buy or Sell?^END",
    };

    private string choiceA = "Buy";
    private string choiceB = "Sell";
    private bool started;
    private GameObject greetingInstance;
    private GameObject choicePickerInstance;

    private string[] storeList = new string[]
    {
        "Key^-->",
        "Sword^-->",
        "Axe^-->"
    };

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (InputController.Instance.DirectionalInput.y > 0.5f)
        {
            if (started)
                return;

            started = true;
            greetingInstance = TextPool.Instance.GetFromPoolInactive(TextPool.TextPools.ScrollingConversation);
            greetingInstance.SetActive(true);
            greetingInstance.GetComponent<Conversation>().ShowConversation(LandscapeContainer.Instance.GetCursorStartPosition(),
                                                                LandscapeContainer.Instance.GetCaretNextPosition(),
                                                                LandscapeContainer.Instance.GetCaretFinishPosition(),
                                                                greetingText);

            greetingInstance.GetComponent<Conversation>().Ended += OnGreetingEnded;

            Player.Instance.GetComponent<Player>().Pause();
            PortraitContainer.Instance.Show();
            LandscapeContainer.Instance.Show();
        }
    }

    private void OnGreetingEnded()
    {
        greetingInstance.GetComponent<Conversation>().Ended -= OnGreetingEnded;
        TextPool.Instance.DeactivateAndAddToPool(greetingInstance);
        choicePickerInstance = TextPool.Instance.GetFromPoolInactive(TextPool.TextPools.ChoicePicker);
        choicePickerInstance.SetActive(true);
        choicePickerInstance.GetComponent<ChoicePicker>().ShowChoicePicker(LandscapeContainer.Instance.GetChoiceAPosition(),
                                                                   LandscapeContainer.Instance.GetChoiceBPosition(),
                                                                   choiceA,
                                                                   choiceB);

        choicePickerInstance.GetComponent<ChoicePicker>().Ended += OnChoicePicked;
    }

    private void OnChoicePicked(string choice)
    {
        choicePickerInstance.GetComponent<ChoicePicker>().Ended -= OnChoicePicked;
        TextPool.Instance.DeactivateAndAddToPool(choicePickerInstance);
        Debug.Log(choice);
        Player.Instance.GetComponent<Player>().Unpause();
        PortraitContainer.Instance.Hide();
        LandscapeContainer.Instance.Hide();
    }
}