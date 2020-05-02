using System.Collections.Generic;
using UnityEngine;

public class ItemLister : MonoBehaviour
{
    [SerializeField] private GameObject cursor;
    [SerializeField] private GameObject cursorStart;
    [SerializeField] private TextUtility textUtility;

    private bool started;

    public delegate void ItemListerHandler(int itemIdx);
    public event ItemListerHandler ItemSelected;

    private int itemIdx;
    private int numItems;
    private Vector2 itemStartPosition;
    private GameObject largeImage;
    private List<GameObject> images;
    private StoreItemBase[] itemList;

    private void Awake()
    {
        textUtility.Initialize(gameObject);
    }

    private void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        if (InputController.Instance.onUp)
        {
            itemIdx = Mathf.Max(itemIdx - 1, 0);
            cursor.transform.position = new Vector2(cursor.transform.position.x,
                                                    cursorStart.transform.position.y - itemIdx);
            SetLargeImage(itemIdx);
        }
        else if (InputController.Instance.onDown)
        {
            itemIdx = Mathf.Min(itemIdx + 1, numItems - 1);
            cursor.transform.position = new Vector2(cursor.transform.position.x,
                                                    cursorStart.transform.position.y - itemIdx);
            SetLargeImage(itemIdx);
        }

        if (InputController.Instance.onSpaceDown)
        {
            ItemSelected.Invoke(itemIdx);
        }
    }

    public void DisplayItems(Vector2 cursorStartPosition,
                             Vector2 largeImagePosition,
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
        itemStartPosition = cursor.transform.position + new Vector3(1f, 0, 0);

        largeImage = ObjectPool.Instance.GetFromPoolInactive(ObjectPool.ObjectPools.ImageObject);
        largeImage.transform.position = largeImagePosition;
        largeImage.SetActive(true);
        SetLargeImage(0);
        DrawItemList();
    }

    private void SetLargeImage(int idx)
    {
        largeImage.GetComponent<SpriteRenderer>().sprite = itemList[idx].largeSprite;
    }

    public void DrawItemList()
    {
        Vector2 nextItemPosition = itemStartPosition;
        for (int lineIdx = 0; lineIdx < itemList.Length; lineIdx++)
        {
            var smallImage = ObjectPool.Instance.GetFromPoolInactive(ObjectPool.ObjectPools.ImageObject);
            smallImage.transform.position = nextItemPosition + new Vector2(-0.5f, 0);
            smallImage.SetActive(true);
            smallImage.GetComponent<SpriteRenderer>().sprite = itemList[lineIdx].smallSprite;
            images.Add(smallImage);

            for (int i = 0; i < itemList[lineIdx].menuName.Length; i++)
            {
                string symbol = itemList[lineIdx].menuName.Substring(i, 1);

                if ("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789',\"!-.?_ ".Contains(symbol))
                {
                    textUtility.DrawSymbol(symbol, nextItemPosition);
                    nextItemPosition += new Vector2(0.5f, 0);
                }
            }
            nextItemPosition = new Vector2(itemStartPosition.x,
                                           nextItemPosition.y - 1.0f);
        }
    }
}
