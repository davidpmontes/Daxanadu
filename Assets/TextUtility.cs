using System.Collections.Generic;
using UnityEngine;

public class TextUtility : MonoBehaviour
{
    private List<GameObject> letters;
    private GameObject background;

    public void Initialize(GameObject background)
    {
        letters = new List<GameObject>();
        this.background = background;
    }

    public GameObject DrawSymbol(string symbol, Vector3 position)
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
            case " ": return null;
        }

        AlphabetPool.Pools parsed_enum = (AlphabetPool.Pools)System.Enum.Parse(typeof(AlphabetPool.Pools), "_" + symbol);
        var letter = AlphabetPool.Instance.GetFromPoolInactive(parsed_enum);
        letter.transform.position = position;
        letter.transform.SetParent(background.transform);
        letter.SetActive(true);
        letters.Add(letter);
        return letter;
    }

    public void RecycleLetters()
    {
        for (int i = letters.Count - 1; i >= 0; i--)
        {
            AlphabetPool.Instance.DeactivateAndAddToPool(letters[i]);
            letters.RemoveAt(i);
        }
    }
}
