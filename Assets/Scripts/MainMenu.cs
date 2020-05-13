using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private GameObject letterPickerInstance;
    [SerializeField] private GameObject cursorStartPosition;

    void Start()
    {
        letterPickerInstance = TextPool.Instance.GetFromPoolInactive(TextPool.TextPools.LetterPicker);
        letterPickerInstance.SetActive(true);
        letterPickerInstance.GetComponent<LetterPicker>().Display(cursorStartPosition.transform.position);
    }
}
