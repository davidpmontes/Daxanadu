using System.Collections;
using UnityEngine;

public class Gold : MonoBehaviour
{
    // 43210
    [SerializeField] private GameObject[] digits;
    [SerializeField] private GameObject[] _0To9;

    private int goldCount = 0;

    private void Start()
    {
        SetGoldCount(4135);
    }

    public void SetGoldCount(int value)
    {
        UpdateGoldCountAnimated(value);
    }

    public void UpdateGoldCountAnimated(int newValue)
    {
        if (goldCount < newValue)
        {
            StartCoroutine(AnimateGold(newValue));
        }
    }

    IEnumerator AnimateGold(int newValue)
    {
        Debug.Log("coroutine");
        for (int i = goldCount; i < newValue; i += 5)
        {
            UpdateDigits(i);
            yield return new WaitForSeconds(0.01f);
        }
        goldCount = newValue;
        UpdateDigits(goldCount);
    }

    public void UpdateDigits(int digitValue)
    {
        string value = digitValue.ToString("00000");

        for (int i = 0; i < digits.Length; i++)
        {
            SetDigit(ref digits[i], digits[i], value[i]);
        }
    }

    private void SetDigit(ref GameObject permanentDigit, GameObject digit, char value)
    {
        var newDigit = Instantiate(_0To9[int.Parse(value.ToString())]);
        newDigit.transform.SetParent(transform);
        newDigit.transform.position = digit.transform.position;
        Destroy(digit);
        permanentDigit = newDigit;
    }
}
