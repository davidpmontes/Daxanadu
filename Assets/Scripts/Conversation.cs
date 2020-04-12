using System.Collections;
using UnityEngine;

public class Conversation : MonoBehaviour
{
    public static Conversation Instance;

    [SerializeField] private GameObject portraitDialogueBox;
    [SerializeField] private GameObject landscapeDialogueBox;

    [SerializeField] private GameObject[] alphabetLower;
    [SerializeField] private GameObject[] alphabetUpper;
    [SerializeField] private GameObject[] digits;
    [SerializeField] private GameObject[] specialCharacters;

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
                if ("abcdefghijklmnopqrstuvwxyz".Contains(symbol))
                {
                    var letter = Instantiate(alphabetLower["abcdefghijklmnopqrstuvwxyz".IndexOf(symbol)]);
                    letter.transform.position = cursor.transform.position;
                }
                else if ("ABCDEFGHIJKLMNOPQRSTUVWXYZ".Contains(symbol))
                {
                    var letter = Instantiate(alphabetUpper["ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(symbol)]);
                    letter.transform.position = cursor.transform.position;
                }
                else if ("0123456789".Contains(symbol))
                {
                    var letter = Instantiate(digits["0123456789".IndexOf(symbol)]);
                    letter.transform.position = cursor.transform.position;
                }
                else if ("',\"!-.?_".Contains(symbol))
                {
                    var letter = Instantiate(specialCharacters["',\"!-.?_".IndexOf(symbol)]);
                    letter.transform.position = cursor.transform.position;
                }
                cursor.transform.position += new Vector3(0.5f, 0, 0);
                yield return new WaitForSeconds(0.1f);
            }
            cursor.transform.position = new Vector3(cursorStart.transform.position.x, cursor.transform.position.y - 0.7f, cursor.transform.position.z);
        }
    }
}
