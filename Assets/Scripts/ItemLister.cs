using System.Collections.Generic;
using UnityEngine;

public class ItemLister : MonoBehaviour
{
    [SerializeField] private GameObject cursor;
    [SerializeField] private GameObject cursorStart;
    [SerializeField] private TextUtility textUtilityStore;
    [SerializeField] private TextUtility textUtilityLandscape;

    private bool started;

    public delegate void ItemListerHandler(int itemIdx);
    public event ItemListerHandler itemSelected;
    public event ItemListerHandler canceled;

    private bool enforcedInputBreak;
    private int itemIdx;
    private int numItems;
    private Vector2 itemStartPosition;
    private List<GameObject> images;
    private StoreItemBase[] itemList;

    public bool canReceiveInput;

    private void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        if (!enforcedInputBreak)
        {
            enforcedInputBreak = true;
            return;
        }

        if (!canReceiveInput)
            return;

        if (InputController.Instance.onUp)
        {
            itemIdx = Mathf.Max(itemIdx - 1, 0);
            cursor.transform.position = new Vector2(cursor.transform.position.x,
                                                    cursorStart.transform.position.y - itemIdx);
            DrawItemDescription(LandscapeContainer.Instance.GetCursorStartPosition(),
                                itemList[itemIdx].description);
        }
        else if (InputController.Instance.onDown)
        {
            itemIdx = Mathf.Min(itemIdx + 1, numItems - 1);
            cursor.transform.position = new Vector2(cursor.transform.position.x,
                                                    cursorStart.transform.position.y - itemIdx);
            DrawItemDescription(LandscapeContainer.Instance.GetCursorStartPosition(),
                                itemList[itemIdx].description);
        }

        if (InputController.Instance.onSpaceDown)
        {
            canReceiveInput = false;
            textUtilityLandscape.RecycleAll();
            itemSelected.Invoke(itemIdx);
        }

        if (InputController.Instance.isCancel)
        {
            canReceiveInput = false;
            canceled.Invoke(-1);
        }
    }

    public void DisplayItems(Vector2 cursorStartPosition,
                             StoreItemBase[] itemList)
    {
        if (started)
        {
            return;
        }

        this.itemList = itemList;
        images = new List<GameObject>();
        started = true;
        numItems = itemList.Length;
        itemIdx = 0;
        cursorStart.transform.position = cursorStartPosition;
        cursor.transform.position = cursorStart.transform.position;
        itemStartPosition = cursor.transform.position + new Vector3(2f, 0, 0);
        textUtilityStore.Initialize(gameObject, false);
        textUtilityLandscape.Initialize(gameObject, true);
        DrawItemDescription(LandscapeContainer.Instance.GetCursorStartPosition(),
                            itemList[0].description);
        DrawItemList();
        canReceiveInput = true;
        enforcedInputBreak = false;
    }

    private void DrawItemDescription(Vector2 position, string[] description)
    {
        textUtilityLandscape.RecycleAll();
        Vector2 startPosition = position;
        for (int i = 0; i < description.Length; i++)
        {
            for (int j = 0; j < description[i].Length; j++)
            {
                string symbol = description[i].Substring(j, 1);

                if ("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789',\"!-.?_ ".Contains(symbol))
                {
                    textUtilityLandscape.DrawSymbol(symbol, position);
                    position += new Vector2(0.5f, 0);
                }
            }
            position = new Vector2(startPosition.x, position.y - 0.6f);
        }
    }

    public void DrawItemList()
    {
        Vector2 nextItemPosition = itemStartPosition;
        for (int lineIdx = 0; lineIdx < itemList.Length; lineIdx++)
        {
            DrawItemImage(nextItemPosition + new Vector2(-1f, 0), itemList[lineIdx].sprite);
            DrawItemName(nextItemPosition, itemList[lineIdx].menuName);
            DrawItemCost(nextItemPosition, itemList[lineIdx].cost.ToString());

            nextItemPosition = new Vector2(itemStartPosition.x,
                                           nextItemPosition.y - 1.0f);
        }
    }

    private void DrawItemImage(Vector2 position, Sprite sprite)
    {
        var smallImage = ObjectPool.Instance.GetFromPoolInactive(ObjectPool.ObjectPools.ImageObject);
        smallImage.transform.position = position;
        smallImage.SetActive(true);
        smallImage.GetComponent<SpriteRenderer>().sprite = sprite;
        images.Add(smallImage);
    }

    private void DrawItemName(Vector2 position, string name)
    {
        for (int i = 0; i < name.Length; i++)
        {
            string symbol = name.Substring(i, 1);

            if ("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789',\"!-.?_ ".Contains(symbol))
            {
                textUtilityStore.DrawSymbol(symbol, position);
                position += new Vector2(0.5f, 0);
            }
        }
    }

    private void DrawItemCost(Vector2 position, string goldCost)
    {
        position = new Vector2(itemStartPosition.x + 5.5f, position.y);
        for (int i = 0; i < goldCost.Length; i++)
        {
            string symbol = goldCost.Substring(i, 1);

            if ("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789',\"!-.?_ ".Contains(symbol))
            {
                textUtilityStore.DrawSymbol(symbol, position);
                position += new Vector2(0.5f, 0);
            }
        }
    }



    public void RecycleAll()
    {
        textUtilityLandscape.RecycleAll();
        textUtilityStore.RecycleAll();

        for (int i = images.Count - 1; i >= 0; i--)
        {
            ObjectPool.Instance.DeactivateAndAddToPool(images[i]);
            images.RemoveAt(i);
        }
    }
}
