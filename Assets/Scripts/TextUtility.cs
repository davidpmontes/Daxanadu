﻿using System.Collections.Generic;
using UnityEngine;

public class TextUtility : MonoBehaviour
{
    private List<GameObject> all;
    private List<List<GameObject>> lines;
    private List<GameObject> currLine;
    private GameObject background;
    private bool shouldReleaseAll;

    public void Initialize(GameObject background, bool shouldReleaseAll)
    {
        this.shouldReleaseAll = shouldReleaseAll;
        all = new List<GameObject>();
        lines = new List<List<GameObject>>();
        currLine = new List<GameObject>();
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

        AlphabetPool.AlphabetPools parsed_enum = (AlphabetPool.AlphabetPools)System.Enum.Parse(typeof(AlphabetPool.AlphabetPools), "_" + symbol);
        var letter = AlphabetPool.Instance.GetFromPoolInactive(parsed_enum);
        letter.transform.position = position;
        letter.transform.SetParent(background.transform);
        letter.SetActive(true);
        if (shouldReleaseAll)
            all.Add(letter);
        else
            currLine.Add(letter);
        return letter;
    }

    public void RecycleAll()
    {
        if (shouldReleaseAll)
        {
            if (all == null)
                return;

            for (int i = all.Count - 1; i >= 0; i--)
            {
                AlphabetPool.Instance.DeactivateAndAddToPool(all[i]);
                all.RemoveAt(i);
            }
        }
        else
        {
            if (lines == null)
                return;

            for(int i = lines.Count - 1; i >= 0; i--)
            {
                RecycleLine(lines[i]);
                lines.RemoveAt(i);
            }
        }
    }

    public void NewLine()
    {
        if (lines.Count > 3)
        {
            RecycleLine(lines[0]);
            lines.RemoveAt(0);
        }
        lines.Add(currLine);
        currLine = new List<GameObject>();
    }

    private void RecycleLine(List<GameObject> line)
    {
        for (int i = line.Count - 1; i >= 0; i--)
        {
            AlphabetPool.Instance.DeactivateAndAddToPool(line[i]);
            line.RemoveAt(i);
        }
    }
}
