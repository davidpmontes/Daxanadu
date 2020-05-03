﻿using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;

[RequireComponent(typeof(BoxCollider2D))]
public class Stores : MonoBehaviour
{
    public Vector2 playerDestination;
    public Vector2 cameraDestination;
    public StoreItemBase[] storeItems;

    private string[] greetingText = new string[] {
        "Welcome!^-->",
        "Welcome!^-->",
        "How may I help?^-->",
        "Buy or Sell?^END",
    };

    private bool started;
    private GameObject greetingInstance;
    private GameObject buySellPickerInstance;
    private GameObject itemLister;
    private GameObject confirmPurchasePickerInstance;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (InputController.Instance.DirectionalInput.y > 0.5f)
        {
            if (started)
                return;

            started = true;
            greetingInstance = TextPool.Instance.GetFromPoolInactive(TextPool.TextPools.ScrollingConversation);
            greetingInstance.SetActive(true);
            greetingInstance.GetComponent<Conversation>().ShowConversation(
                                    LandscapeContainer.Instance.GetCursorStartPosition(),
                                    LandscapeContainer.Instance.GetCaretNextPosition(),
                                    LandscapeContainer.Instance.GetCaretFinishPosition(),
                                    greetingText);

            greetingInstance.GetComponent<Conversation>().Ended += OnGreetingEnded;

            Player.Instance.GetComponent<Player>().Pause();
            PortraitContainer.Instance.Show();
            LandscapeContainer.Instance.Show();
        }
    }

    private void OnGreetingEnded()
    {
        greetingInstance.GetComponent<Conversation>().Ended -= OnGreetingEnded;
        TextPool.Instance.DeactivateAndAddToPool(greetingInstance);
        buySellPickerInstance = TextPool.Instance.GetFromPoolInactive(TextPool.TextPools.ChoicePicker);
        buySellPickerInstance.SetActive(true);
        buySellPickerInstance.GetComponent<ChoicePicker>().ShowChoicePicker
                                    (LandscapeContainer.Instance.GetChoiceAPosition(),
                                     LandscapeContainer.Instance.GetChoiceBPosition(),
                                     "How may I help?",
                                     "Buy", "Sell");

        buySellPickerInstance.GetComponent<ChoicePicker>().Ended += OnBuySellSelected;
    }

    private void OnBuySellSelected(string choice)
    {
        buySellPickerInstance.GetComponent<ChoicePicker>().Ended -= OnBuySellSelected;
        TextPool.Instance.DeactivateAndAddToPool(buySellPickerInstance);
        StoreContainer.Instance.Show();

        itemLister = TextPool.Instance.GetFromPoolInactive(TextPool.TextPools.ItemLister);
        itemLister.SetActive(true);
        itemLister.GetComponent<ItemLister>().DisplayItems(StoreContainer.Instance.GetCaretStartPosition(),
                                                           storeItems);
        itemLister.GetComponent<ItemLister>().ItemSelected += OnItemSelected;
    }

    private void OnItemSelected(int itemIdx)
    {
        confirmPurchasePickerInstance = TextPool.Instance.GetFromPoolInactive(
                                                TextPool.TextPools.ChoicePicker);
        confirmPurchasePickerInstance.SetActive(true);
        confirmPurchasePickerInstance.GetComponent<ChoicePicker>().ShowChoicePicker(
                                    LandscapeContainer.Instance.GetChoiceAPosition(),
                                    LandscapeContainer.Instance.GetChoiceBPosition(),
                                    "Buy this?",
                                    "Yes", "No");
    }
}