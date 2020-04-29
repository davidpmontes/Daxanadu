using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    private enum STATES
    {
        NORMAL, NEXT, FINISH, BUY
    }

    public static Shop Instance;

    [SerializeField] private GameObject portraitBox;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject storeBox;

    [SerializeField] private GameObject dialogueCursorStart;
    [SerializeField] private GameObject storeCursorStart;
    [SerializeField] private GameObject cursor;
    [SerializeField] private GameObject caretFinished;
    [SerializeField] private GameObject caretNext;
    [SerializeField] private GameObject scrollingBackground;

    private STATES state = STATES.NORMAL;
    private List<GameObject> letters = new List<GameObject>();
    private IEnumerator caretBlinkCoroutine;

    private readonly float INSTANT_SPEED = 0f;
    private readonly float SLOW_SPEED = 0.1f;
    private readonly float FAST_SPEED = 0.03f;
    private float speed;
    private bool started;

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
        speed = SLOW_SPEED;
    }

    private void Update()
    {
        if (InputController.Instance.isJumpStart)
        {
            switch (state)
            {
                case STATES.NORMAL:
                    break;
                case STATES.NEXT:
                    StopCoroutine(caretBlinkCoroutine);
                    caretNext.SetActive(false);
                    state = STATES.NORMAL;
                    break;
                case STATES.FINISH:
                    HideDialogue();
                    EndAll();
                    break;
                case STATES.BUY:
                    HideDialogue();
                    ShowStoreBox();
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
            HideDialogue();
        }
    }

    public void ShowStoreBox()
    {
        if (started)
            return;

        started = true;

        state = STATES.NORMAL;

        portraitBox.SetActive(true);
        dialogueBox.SetActive(false);
        storeBox.SetActive(true);

        caretFinished.SetActive(false);
        caretNext.SetActive(false);
        Player.Instance.GetComponent<Player>().Pause();

        scrollingBackground.transform.position = Vector3.zero;
        cursor.transform.position = storeCursorStart.transform.position;

        StartCoroutine(DrawConversation(storeList));
    }

    public void ShowConversation()
    {
        if (started)
            return;

        started = true;

        state = STATES.NORMAL;

        portraitBox.SetActive(true);
        dialogueBox.SetActive(true);
        storeBox.SetActive(false);

        caretFinished.SetActive(false);
        caretNext.SetActive(false);
        Player.Instance.GetComponent<Player>().Pause();

        scrollingBackground.transform.position = Vector3.zero;
        cursor.transform.position = dialogueCursorStart.transform.position;

        StartCoroutine(DrawConversation(convo));
    }

    public void HideDialogue()
    {
        StopAllCoroutines();
        caretFinished.SetActive(false);
        caretNext.SetActive(false);
        portraitBox.SetActive(false);
        dialogueBox.SetActive(false);
        RecycleLetters();
        //if (Ended != null) Ended.Invoke();
    }

    private void EndAll()
    {
        Player.Instance.GetComponent<Player>().Unpause();
        started = false;
    }

    private void ClearConversation()
    {
        scrollingBackground.transform.position = Vector3.zero;
        cursor.transform.position = dialogueCursorStart.transform.position;
        state = STATES.NORMAL;
        RecycleLetters();
    }

    IEnumerator DrawConversation(string[] dialogue)
    {
        for (int lineIdx = 0; lineIdx < dialogue.Length; lineIdx++)
        {
            for (int i = 0; i < dialogue[lineIdx].Length; i++)
            {
                string symbol = dialogue[lineIdx].Substring(i, 1);

                if ("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789',\"!-.?_ ".Contains(symbol))
                {
                    DrawLetter(symbol);
                    cursor.transform.position += new Vector3(0.5f, 0, 0);
                    yield return new WaitForSeconds(speed);
                }
                else if (symbol == "^")
                {
                    string cmd = dialogue[lineIdx].Substring(i + 1, 3);

                    if (cmd == "BUY")
                    {
                        state = STATES.BUY;
                        break;
                    }
                    else if (cmd == "CLR")
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
                }
            }
        }
    }



    private void DrawLetter(string symbol)
    {
        switch (symbol)
        {
            case "'": symbol = "apostrophe"; break;
            case ",": symbol = "comma"; break;
            case "\"": symbol = "doubleQuotes"; break;
            case "!": symbol = "exclamationPoint"; break;
            case "-": symbol = "hyphen"; break;
            case ".": symbol = "period"; break;
            case "?": symbol = "questionMark"; break;
            case "_": symbol = "underscore"; break;
            case " ": return;
        }

        AlphabetPool.Pools parsed_enum = (AlphabetPool.Pools)System.Enum.Parse(typeof(AlphabetPool.Pools), "_" + symbol);
        var letter = AlphabetPool.Instance.GetFromPoolInactive(parsed_enum);
        letter.transform.position = cursor.transform.position;
        letter.transform.SetParent(scrollingBackground.transform);
        letter.SetActive(true);
        letters.Add(letter);
    }

    private void EndOfLine(int lineIdx)
    {
        if (state != STATES.FINISH)
        {
            if (lineIdx < 3)
            {
                cursor.transform.position = new Vector3(dialogueCursorStart.transform.position.x,
                                                        cursor.transform.position.y - 0.6f,
                                                        cursor.transform.position.z);
            }
            else
            {
                scrollingBackground.transform.position += new Vector3(0, 0.6f, 0);
                cursor.transform.position = new Vector3(dialogueCursorStart.transform.position.x,
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

    private void RecycleLetters()
    {
        for (int i = letters.Count - 1; i >= 0; i--)
        {
            AlphabetPool.Instance.DeactivateAndAddToPool(letters[i]);
            letters.RemoveAt(i);
        }
    }
}
