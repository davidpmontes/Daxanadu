using System.Collections;
using UnityEngine;

public class Conversation : MonoBehaviour
{
    public static Conversation Instance;

    [SerializeField] private GameObject portraitDialogueBox;
    [SerializeField] private GameObject landscapeDialogueBox;

    [SerializeField] private GameObject cursorStart;
    [SerializeField] private GameObject cursor;

    //20 characters max per line
    private string[] convo = new string[] {
        "Hello, my friend!",
        "How are you doing??",
        "1500 Gold???...",
        "O.K. fine."
    };

    private void Awake()
    {
        Instance = this;
    }

    public void ShowConversation()
    {
        if (landscapeDialogueBox.activeSelf)
            return;

        portraitDialogueBox.SetActive(true);
        landscapeDialogueBox.SetActive(true);

        StartCoroutine(DrawConversation());
    }

    public void HideConversation()
    {
        portraitDialogueBox.SetActive(false);
        landscapeDialogueBox.SetActive(false);
    }

    IEnumerator DrawConversation()
    {
        cursor.transform.position = cursorStart.transform.position;

        foreach (string line in convo)
        {
            for (int i = 0; i < line.Length; i++)
            {
                string symbol = line.Substring(i, 1);
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
                    letter.SetActive(true);
                }

                cursor.transform.position += new Vector3(0.5f, 0, 0);
                yield return new WaitForSeconds(0.1f);
            }
            cursor.transform.position = new Vector3(cursorStart.transform.position.x, cursor.transform.position.y - 0.7f, cursor.transform.position.z);
        }
    }
}
