using System.Collections;
using UnityEngine;

public class Conversation : MonoBehaviour
{
    private enum STATES
    {
        NORMAL, NEXT, FINISH
    }

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
    public event ConversationHandler finished;
    public event ConversationHandler canceled;

    public bool canReceiveInput;

    private void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        if (!canReceiveInput)
            return;

        if (InputController.Instance.onJumpDown)
        {
            switch (state)
            {
                case STATES.NORMAL:
                    break;
                case STATES.NEXT:
                    Continue();
                    break;
                case STATES.FINISH:
                    canReceiveInput = false;
                    finished.Invoke();
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
            canReceiveInput = false;
            canceled.Invoke();
        }
    }

    public void ShowConversation(Vector2 cursorStartPosition,
                                 Vector2 caretNextPosition,
                                 Vector2 caretFinishedPosition,
                                 string[] convo)
    {
        if (started)
        {
            return;
        }

        canReceiveInput = true;
        started = true;
        state = STATES.NORMAL;
        scrollingBackground.transform.position = Vector3.zero;
        cursorStart.transform.position = cursorStartPosition;
        caretNext.transform.position = caretNextPosition;
        caretFinished.transform.position = caretFinishedPosition;
        cursor.transform.position = cursorStart.transform.position;
        textUtility.Initialize(scrollingBackground, false);
        DrawConversation(convo);
    }

    public void HideConversation()
    {
        started = false;
        StopAllCoroutines();
        caretFinished.SetActive(false);
        caretNext.SetActive(false);
        textUtility.NewLine();
        textUtility.RecycleAll();
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
                        Debug.LogError(cmd + ": Unrecognized character");
                    }
                }
            }
            textUtility.NewLine();
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

    public void ClearConversation()
    {
        scrollingBackground.transform.position = Vector3.zero;
        cursor.transform.position = cursorStart.transform.position;
        state = STATES.NORMAL;
        textUtility.RecycleAll();
    }
}
