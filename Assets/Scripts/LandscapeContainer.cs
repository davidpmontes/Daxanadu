using UnityEngine;

public class LandscapeContainer : MonoBehaviour
{
    public static LandscapeContainer Instance { get; private set; }

    [SerializeField] private GameObject frame;
    [SerializeField] private GameObject cursorStartPosition;
    [SerializeField] private GameObject caretNextPosition;
    [SerializeField] private GameObject caretFinishPosition;
    [SerializeField] private GameObject choiceAPosition;
    [SerializeField] private GameObject choiceBPosition;

    private void Awake()
    {
        Instance = this;
        Hide();
    }

    public Vector2 GetCursorStartPosition() => cursorStartPosition.transform.position;
    public Vector2 GetCaretNextPosition() => caretNextPosition.transform.position;
    public Vector2 GetCaretFinishPosition() => caretFinishPosition.transform.position;
    public Vector2 GetChoiceAPosition() => choiceAPosition.transform.position;
    public Vector2 GetChoiceBPosition() => choiceBPosition.transform.position;

    public void Show()
    {
        frame.SetActive(true);
    }

    public void Hide()
    {
        frame.SetActive(false);
    }
}