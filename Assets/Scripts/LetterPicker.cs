using UnityEngine;

public class LetterPicker : MonoBehaviour
{
    [SerializeField] private GameObject cursor = default;
    [SerializeField] private TextUtility textUtilityAlphabet = default;

    private bool started;

    public delegate void ItemListerHandler(int itemIdx);
    public event ItemListerHandler itemSelected;
    public event ItemListerHandler canceled;

    private bool enforcedActionB_Released;
    private bool enforcedActionA_Released;
    private readonly int itemIdx = 0;
    public bool canReceiveInput;

    private void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        if (!canReceiveInput)
            return;

        if (!InputController.Instance.actionPrimary)
            enforcedActionA_Released = true;

        if (!InputController.Instance.actionCancel)
            enforcedActionB_Released = true;

        if (InputController.Instance.onActionPrimary_Down)
        {
            if (!enforcedActionA_Released)
                return;

            canReceiveInput = false;
            //textUtilityLandscape.RecycleAll();
            itemSelected.Invoke(itemIdx);
        }

        if (InputController.Instance.onActionCancel_Down)
        {
            if (!enforcedActionB_Released)
                return;

            canReceiveInput = false;
            canceled.Invoke(-1);
        }
    }

    public void EnableReceivingInput()
    {
        canReceiveInput = true;
        enforcedActionA_Released = false;
        enforcedActionB_Released = false;
    }

    public void Display(Vector2 startPosition)
    {
        cursor.transform.position = startPosition + new Vector2(-0.5f, 0);

        textUtilityAlphabet.GetComponent<TextUtility>().Initialize(gameObject, false);

        string[] description = new string[] {
            "A B C D E F G H I J K",
            "",
            "L M N O P Q R S T U V",
            "",
            "W X Y Z - ' ! ?",
            "",
            "a b c d e f g h i j k",
            "",
            "l m n o p q r s t u v",
            "",
            "w x y z"
        };

        Vector2 position = startPosition;
        Vector2 startPositionX = startPosition;

        for (int i = 0; i < description.Length; i++)
        {
            for (int j = 0; j < description[i].Length; j++)
            {
                string symbol = description[i].Substring(j, 1);
                if ("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789',\"!-.?_ ".Contains(symbol))
                {
                    textUtilityAlphabet.DrawSymbol(symbol, position);
                    position += new Vector2(0.5f, 0);
                }
            }
            position = new Vector2(startPositionX.x, position.y - 0.6f);
        }
    }

    public void Hide()
    {
        started = false;
        cursor.SetActive(false);
        textUtilityAlphabet.RecycleAll();
    }
}
