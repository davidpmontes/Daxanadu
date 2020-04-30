using System.Collections;
using UnityEngine;

public enum STATES
{
    NORMAL, NEXT, FINISH
}

public class Conversation : MonoBehaviour
{
    public static Conversation Instance;

    [SerializeField] private GameObject cursor;
    [SerializeField] private GameObject cursorStart;
    [SerializeField] private GameObject scrollingBackground;
    [SerializeField] private GameObject caretFinished;
    [SerializeField] private GameObject caretNext;

    [SerializeField] private TextUtility textUtility;

    private IEnumerator caretBlinkCoroutine;
    private STATES state;
    private bool started;
    private float speed;

    private readonly float FAST_SPEED = 0.03f;
    private readonly float SLOW_SPEED = 0.1f;
    private readonly float INSTANT_SPEED = 0.0f;

    public delegate void ConversationHandler();
    public event ConversationHandler Ended;

    //20 characters max per line, excluding > and ^
    private string[] convo = new string[] {
        "Glad you could^-->",
        "come.^-->",
        "Disaster has^-->",
        "befallen us.^NXT",
        "The Elf^-->",
        "fountain water,^-->",
        "our life source,^-->",
        "has stopped.^NXT",
        "The wells are^-->",
        "drying up.^NXT",
        "Many men went^-->",
        "out and nobody^-->",
        "came back.^NXT",
        "You are our last^-->",
        "hope.^-->",
        "I shall give you^-->",
        "1500 Golds.^NXT",
        "Prepare^-->",
        "for your journey^-->",
        "with this money.^NXT",
        "It will be a^-->",
        "dangerous^-->",
        "journey.^NXT",
        "Take care^-->",
        "of yourself.^END"
    };

    private void Awake()
    {
        Instance = this;
        textUtility.Initialize(scrollingBackground);
    }

    private void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        if (InputController.Instance.isJumpStart)
        {
            switch (state)
            {
                case STATES.NORMAL:
                    break;
                case STATES.NEXT:
                    Continue();
                    break;
                case STATES.FINISH:
                    HideConversation();
                    break;
            }
        }

        if (InputController.Instance.isSpaceDown)
        {
            speed = FAST_SPEED;
        }
        else
        {
            speed = SLOW_SPEED;
        }

        if (InputController.Instance.isCancel)
        {
            HideConversation();
        }
    }

    public void ShowConversation(Vector2 cursorStartPosition,
                                 Vector2 caretNextPosition,
                                 Vector2 caretFinishedPosition)
    {
        if (started)
        {
            return;
        }

        started = true;
        state = STATES.NORMAL;
        scrollingBackground.transform.position = Vector3.zero;
        cursorStart.transform.position = cursorStartPosition;
        caretNext.transform.position = caretNextPosition;
        caretFinished.transform.position = caretFinishedPosition;
        cursor.transform.position = cursorStart.transform.position;
        Player.Instance.GetComponent<Player>().Pause();
        PortraitContainer.Instance.Show();
        LandscapeContainer.Instance.Show();
        DrawConversation(convo);
    }

    public void HideConversation()
    {
        started = false;
        StopAllCoroutines();
        caretFinished.SetActive(false);
        caretNext.SetActive(false);
        Player.Instance.GetComponent<Player>().Unpause();
        PortraitContainer.Instance.Hide();
        LandscapeContainer.Instance.Hide();
        textUtility.RecycleLetters();
        Ended.Invoke();
        TextPool.Instance.DeactivateAndAddToPool(gameObject);
    }

    public void Continue()
    {
        StopCoroutine(caretBlinkCoroutine);
        caretNext.SetActive(false);
        state = STATES.NORMAL;
    }

    public void DrawConversation(string[] dialogue)
    {
        StartCoroutine(DrawConversationCoroutine(dialogue));
    }

    private IEnumerator DrawConversationCoroutine(string[] dialogue)
    {
        for (int lineIdx = 0; lineIdx < dialogue.Length; lineIdx++)
        {
            for (int i = 0; i < dialogue[lineIdx].Length; i++)
            {
                string symbol = dialogue[lineIdx].Substring(i, 1);

                if ("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789',\"!-.?_ ".Contains(symbol))
                {
                    textUtility.DrawSymbol(symbol, cursor.transform.position);
                    cursor.transform.position += new Vector3(0.5f, 0, 0);
                    yield return new WaitForSeconds(speed);
                }
                else if (symbol == "^")
                {
                    string cmd = dialogue[lineIdx].Substring(i + 1, 3);

                    if (cmd == "CLR")
                    {
                        ClearConversation();
                        break;
                    }
                    else if (cmd == "END")
                    {
                        state = STATES.FINISH;
                        caretBlinkCoroutine = CaretBlink(caretFinished);
                        StartCoroutine(caretBlinkCoroutine);
                        break;
                    }
                    else if (cmd == "NXT")
                    {
                        state = STATES.NEXT;
                        caretBlinkCoroutine = CaretBlink(caretNext);
                        StartCoroutine(caretBlinkCoroutine);
                        while (state == STATES.NEXT)
                        {
                            yield return null;
                        }
                        EndOfLine(lineIdx);
                        break;
                    }
                    else if (cmd == "-->")
                    {
                        EndOfLine(lineIdx);
                        break;
                    }
                    else
                    {
                        Debug.LogError("Unrecognized character");
                    }
                }
            }
        }
    }

    private void EndOfLine(int lineIdx)
    {
        if (state != STATES.FINISH)
        {
            if (lineIdx < 3)
            {
                cursor.transform.position = new Vector3(cursorStart.transform.position.x,
                                                        cursor.transform.position.y - 0.6f,
                                                        cursor.transform.position.z);
            }
            else
            {
                scrollingBackground.transform.position += new Vector3(0, 0.6f, 0);
                cursor.transform.position = new Vector3(cursorStart.transform.position.x,
                                                        cursor.transform.position.y,
                                                        cursor.transform.position.z);
            }
        }
    }

    IEnumerator CaretBlink(GameObject caret)
    {
        while (true)
        {
            caret.SetActive(!caret.activeSelf);
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void ClearConversation()
    {
        scrollingBackground.transform.position = Vector3.zero;
        cursor.transform.position = cursorStart.transform.position;
        state = STATES.NORMAL;
        textUtility.RecycleLetters();
    }
}
