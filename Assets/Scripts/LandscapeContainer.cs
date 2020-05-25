using UnityEngine;

public class LandscapeContainer : MonoBehaviour
{
    public static LandscapeContainer Instance { get; private set; }

    [SerializeField] private GameObject frame = default;
    [SerializeField] private GameObject cursorStartPosition = default;
    [SerializeField] private GameObject caretNextPosition = default;
    [SerializeField] private GameObject caretFinishPosition = default;
    [SerializeField] private GameObject choiceAPosition = default;
    [SerializeField] private GameObject choiceBPosition = default;

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