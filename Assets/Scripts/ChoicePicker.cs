using UnityEngine;

public class ChoicePicker : MonoBehaviour
{
    [SerializeField] private GameObject caret = default;
    [SerializeField] private GameObject choiceAPosition = default;
    [SerializeField] private GameObject choiceBPosition = default;

    [SerializeField] private TextUtility textUtility = default;

    public delegate void ChoicePickerHandler();
    public event ChoicePickerHandler canceled;
    public event ChoicePickerHandler choiceA;
    public event ChoicePickerHandler choiceB;
    private bool isChoiceA;

    private bool canReceiveInput;
    private bool started;

    private void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        if (!canReceiveInput)
            return;

        float input = InputController.Instance.DirectionalInput.x;

        if (input > 0)
        {
            isChoiceA = false;
            caret.transform.position = choiceBPosition.transform.position;
        }
        else if (input < 0)
        {
            isChoiceA = true;
            caret.transform.position = choiceAPosition.transform.position;
        }

        if (InputController.Instance.onActionPrimary_Down)
        {
            canReceiveInput = false;
            if (isChoiceA)
                choiceA.Invoke();
            else
                choiceB.Invoke();
        }

        if (InputController.Instance.onActionCancel_Down)
        {
            canReceiveInput = false;
            canceled.Invoke();
        }
    }

    public void ShowChoicePicker(Vector2 choiceAPosition, Vector2 choiceBPosition,
                                 string description, string choiceAText, string choiceBText)
    {
        if (started)
        {
            return;
        }

        started = true;
        canReceiveInput = true;
        this.choiceAPosition.transform.position = choiceAPosition;
        this.choiceBPosition.transform.position = choiceBPosition;
        caret.SetActive(true);
        caret.transform.position = choiceAPosition;
        textUtility.Initialize(gameObject, true);
        isChoiceA = true;
        DrawText(description, LandscapeContainer.Instance.GetCursorStartPosition());
        DrawText(choiceAText, choiceAPosition);
        DrawText(choiceBText, choiceBPosition);
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

    public void Hide()
    {
        started = false;
        caret.SetActive(false);
        textUtility.RecycleAll();
    }
}
