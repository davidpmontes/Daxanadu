using UnityEngine;

public class LandscapeContainer : MonoBehaviour
{
    public static LandscapeContainer Instance { get; private set; }

    [SerializeField] private GameObject frame;

    private void Awake()
    {
        Instance = this;
        Hide();
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
