using UnityEngine;

public class ChoicePicker : MonoBehaviour
{
    [SerializeField] private GameObject caret;
    [SerializeField] private GameObject choiceAPosition;
    [SerializeField] private GameObject choiceBPosition;

    [SerializeField] private TextUtility textUtility;

    public delegate void ChoicePickerHandler(string choice);
    public event ChoicePickerHandler Ended;

    private string choiceA;
    private string choiceB;
    private string choice;

    private bool started;

    private void Awake()
    {
        textUtility.Initialize(gameObject);
    }

    private void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        float input = InputController.Instance.DirectionalInput.x;

        if (input > 0)
        {
            choice = choiceB;
            caret.transform.position = choiceBPosition.transform.position;
        }
        else if (input < 0)
        {
            choice = choiceA;
            caret.transform.position = choiceAPosition.transform.position;
        }

        if (InputController.Instance.onSpaceDown)
        {
            textUtility.RecycleAll();
            Ended.Invoke(choice);
        }
    }

    public void ShowChoicePicker(Vector2 choiceAPosition, Vector2 choiceBPosition,
                                 string description, string choiceA, string choiceB)
    {
        if (started)
        {
            return;
        }

        started = true;
        this.choice = choiceA;
        this.choiceA = choiceA;
        this.choiceB = choiceB;
        this.choiceAPosition.transform.position = choiceAPosition;
        this.choiceBPosition.transform.position = choiceBPosition;
        caret.transform.position = choiceAPosition;
        DrawText(description, LandscapeContainer.Instance.GetCursorStartPosition());
        DrawText(choiceA, choiceAPosition);
        DrawText(choiceB, choiceBPosition);
    }

    private void DrawText(string choice, Vector2 choicePosition)
    {
        for (int i = 0; i < choice.Length; i++)
        {
            string symbol = choice.Substring(i, 1);

            if ("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789',\"!-.?_ ".Contains(symbol))
            {
                choicePosition += new Vector2(0.5f, 0);
                textUtility.DrawSymbol(symbol, choicePosition);
            }
        }
    }
}
