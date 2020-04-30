using System.Collections;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public static Shop Instance;

    [SerializeField] private GameObject cursor;
    [SerializeField] private GameObject cursorStart;
    [SerializeField] private GameObject caretFinished;
    [SerializeField] private GameObject caretNext;

    [SerializeField] private GameObject scrollingBackground;

    [SerializeField] private TextUtility textUtility;

    private IEnumerator caretBlinkCoroutine;
    private bool started;
    private bool userCanInteract;
    private float speed;
    private STATES state;

    //public delegate void ConversationHandler();
    //public event ConversationHandler Ended;

    //20 characters max per line, excluding > and ^
    private string[] convo = new string[] {
        "Welcome!^-->",
        "Welcome!^-->",
        "How may I help?^-->",
        "Buy or Sell?^NXT",
        "BUY?^BUY"
    };

    private string[] storeList = new string[]
    {
        "Key^-->",
        "Sword^-->",
        "Axe^-->"
    };

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        GetInput();
    }

    public void GetInput()
    {
        if (!userCanInteract)
            return;
    }

    public void ShowStoreBox()
    {
        if (started)
            return;

        started = true;

        PortraitContainer.Instance.Show();
        StoreContainer.Instance.Show();

        Player.Instance.GetComponent<Player>().Pause();
        scrollingBackground.transform.position = Vector3.zero;
        userCanInteract = false;

        DrawConversation(convo);
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
