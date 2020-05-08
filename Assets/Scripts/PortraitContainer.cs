using UnityEngine;
using UnityEngine.Assertions;

public class PortraitContainer : MonoBehaviour
{
    public static PortraitContainer Instance { get; private set; }

    [SerializeField] private GameObject frame;

    private void Awake()
    {
        Instance = this;
        HideFrame();
    }

    public void ShowPortrait(string name)
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

    public void HideAllPortraits()
    {
        foreach (Transform child in frame.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void ShowFrame()
    {
        frame.SetActive(true);
    }

    public void HideFrame()
    {
        frame.SetActive(false);
    }
}
