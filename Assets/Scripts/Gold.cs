using System.Collections;
using UnityEngine;

public class Gold : MonoBehaviour
{
    public static Gold Instance { get; private set; }
    // 43210
    [SerializeField] private SpriteRenderer[] digits;
    [SerializeField] private Sprite[] _0To9;

    private int visibleGoldCount = 0;
    private int actualGoldCount = 0;
    private bool isAnimating;

    private void Awake()
    {
        Instance = this;
    }

    public void ChangeGoldAmount(int newAmount)
    {
        actualGoldCount += newAmount;
        if (!isAnimating)
            StartCoroutine(AnimateGoldCount());
    }

    private IEnumerator AnimateGoldCount()
    {
        isAnimating = true;
        while(visibleGoldCount != actualGoldCount)
        {
            if (visibleGoldCount < actualGoldCount)
                visibleGoldCount += 5;
            else
                visibleGoldCount -= 5;
            UpdateAllDigits();
            yield return new WaitForSeconds(0.1f);
        }
        isAnimating = false;
    }

    private void UpdateAllDigits()
    {
        string value = visibleGoldCount.ToString("00000");

        for (int i = 0; i < digits.Length; i++)
        {
            digits[i].sprite = _0To9[int.Parse(value[i].ToString())];
        }
    }
}
