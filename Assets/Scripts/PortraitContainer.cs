using UnityEngine;
using UnityEngine.Assertions;

public class PortraitContainer : MonoBehaviour
{
    public static PortraitContainer Instance { get; private set; }

    [SerializeField] private GameObject frame;

    private void Awake()
    {
        Instance = this;
        Hide();
    }

    public void SetPortrait(string name)
    {
        bool childfound = false;
        foreach (Transform child in frame.transform)
        {
            if (child.gameObject.name == name)
            {
                childfound = true;
                child.gameObject.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
        Assert.IsTrue(childfound);
    }

    public void Show()
    {
        frame.SetActive(true);
    }

    public void Hide()
    {
        frame.SetActive(false);
    }
}
