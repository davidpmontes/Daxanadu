using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conversation : MonoBehaviour
{
    private enum STATES
    {
        NORMAL, NEXT, FINISH
    }

    public static Conversation Instance;

    [SerializeField] private GameObject portraitDialogueBox;
    [SerializeField] private GameObject landscapeDialogueBox;

    [SerializeField] private GameObject cursorStart;
    [SerializeField] private GameObject cursor;
    [SerializeField] private GameObject caretFinished;
    [SerializeField] private GameObject caretNext;
    [SerializeField] private GameObject scrollingBackground;
    
    private STATES state = STATES.NORMAL;
    private List<GameObject> letters = new List<GameObject>();
    private IEnumerator caretBlinkCoroutine;

    public delegate void ConversationHandler();
    public event ConversationHandler Ended;

    //20 characters max per line, excluding > and ^
    private string[] convo = new string[] {
        "Glad you could",
        "come.",
        "Disaster has",
        "befallen us.>",
        "The Elf",
        "fountain water,",
        "our life source,",
        "has stopped.>",
        "The wells are",
        "drying up.>",
        "Many men went",
        "out and nobody",
        "came back.>",
        "You are our last",
        "hope.",
        "I shall give you",
        "1500 Golds.>",
        "Prepare",
        "for your journey",
        "with this money.>",
        "It will be a",
        "dangerous",
        "journey.>",
        "Take care",
        "of yourself.^"
    };

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (InputController.Instance.isJumpStart)
        {
            switch(state)
            {
                case STATES.NORMAL:
                    break;
                case STATES.NEXT:
                    StopCoroutine(caretBlinkCoroutine);
                    caretNext.SetActive(false);
                    state = STATES.NORMAL;
                    break;
                case STATES.FINISH:
                    HideConversation();
                    break;
            }
        }

        if (InputController.Instance.isCancel)
        {
            HideConversation();
        }
    }

    public void ShowConversation()
    {
        if (landscapeDialogueBox.activeSelf)
            return;

        scrollingBackground.transform.position = Vector3.zero;
        caretFinished.SetActive(false);
        caretNext.SetActive(false);
        Player.Instance.GetComponent<Player>().Pause();
        portraitDialogueBox.SetActive(true);
        landscapeDialogueBox.SetActive(true);

        StartCoroutine(DrawConversation());
    }

    public void HideConversation()
    {
        StopAllCoroutines();
        caretFinished.SetActive(false);
        caretNext.SetActive(false);
        Player.Instance.GetComponent<Player>().Unpause();
        portraitDialogueBox.SetActive(false);
        landscapeDialogueBox.SetActive(false);
        RecycleLetters();
        Ended.Invoke();
    }

    IEnumerator DrawConversation()
    {
        cursor.transform.position = cursorStart.transform.position;


        //foreach (string line in convo)
        for (int lineIdx = 0; lineIdx < convo.Length; lineIdx++)
        {
            for (int i = 0; i < convo[lineIdx].Length; i++)
            {
                string symbol = convo[lineIdx].Substring(i, 1);
                if ("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789',\"!-.?_".Contains(symbol))
                {
                    switch(symbol)
                    {
                        case "'": symbol = "apostrophe"; break;
                        case ",": symbol = "comma"; break;
                        case "\"": symbol = "doubleQuotes"; break;
                        case "!": symbol = "exclamationPoint"; break;
                        case "-": symbol = "hyphen"; break;
                        case ".": symbol = "period"; break;
                        case "?": symbol = "questionMark"; break;
                        case "_": symbol = "underscore"; break;
                    }

                    AlphabetPool.Pools parsed_enum = (AlphabetPool.Pools)System.Enum.Parse(typeof(AlphabetPool.Pools), "_" + symbol);
                    var letter = AlphabetPool.Instance.GetFromPoolInactive(parsed_enum);
                    letter.transform.position = cursor.transform.position;
                    letter.transform.SetParent(scrollingBackground.transform);
                    letter.SetActive(true);
                    letters.Add(letter);
                }
                else if (symbol == ">") // next
                {
                    state = STATES.NEXT;
                    caretBlinkCoroutine = CaretBlink(caretNext);
                    StartCoroutine(caretBlinkCoroutine);
                    while (state == STATES.NEXT)
                    {
                        yield return null;
                    }
                }
                else if (symbol == "^") // finish
                {
                    state = STATES.FINISH;
                    caretBlinkCoroutine = CaretBlink(caretFinished);
                    StartCoroutine(caretBlinkCoroutine);
                }

                cursor.transform.position += new Vector3(0.5f, 0, 0);
                yield return new WaitForSeconds(0.1f);
            }

            if (state != STATES.FINISH)
            {
                if (lineIdx < 3)
                {
                    cursor.transform.position = new Vector3(cursorStart.transform.position.x, cursor.transform.position.y - 0.6f, cursor.transform.position.z);
                }
                else
                {
                    scrollingBackground.transform.position += new Vector3(0, 0.6f, 0);
                    cursor.transform.position = new Vector3(cursorStart.transform.position.x, cursor.transform.position.y, cursor.transform.position.z);
                }
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
