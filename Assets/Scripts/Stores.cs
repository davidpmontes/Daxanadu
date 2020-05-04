﻿using UnityEngine;

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
    private GameObject conversationInstance;
    private GameObject choicePickerInstance;
    private GameObject itemListerInstance;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (InputController.Instance.DirectionalInput.y > 0.5f)
        {
            if (started)
                return;

            started = true;

            EnterStore();
            BeginGreeting();
        }
    }

    private void EnterStore()
    {
        conversationInstance = TextPool.Instance.GetFromPoolInactive(TextPool.TextPools.ScrollingConversation);
        choicePickerInstance = TextPool.Instance.GetFromPoolInactive(TextPool.TextPools.ChoicePicker);
        itemListerInstance = TextPool.Instance.GetFromPoolInactive(TextPool.TextPools.ItemLister);

        conversationInstance.SetActive(true);
        choicePickerInstance.SetActive(true);
        itemListerInstance.SetActive(true);

        Player.Instance.GetComponent<Player>().Pause();
        PortraitContainer.Instance.Show();
        LandscapeContainer.Instance.Show();
    }

    private void BeginGreeting()
    {
        conversationInstance.GetComponent<Conversation>().ShowConversation(
                                    LandscapeContainer.Instance.GetCursorStartPosition(),
                                    LandscapeContainer.Instance.GetCaretNextPosition(),
                                    LandscapeContainer.Instance.GetCaretFinishPosition(),
                                    greetingText);

        conversationInstance.GetComponent<Conversation>().finished += OnGreetingFinished;
        conversationInstance.GetComponent<Conversation>().canceled += OnGreetingCanceled;
    }

    private void OnGreetingFinished()
    {
        conversationInstance.GetComponent<Conversation>().finished -= OnGreetingFinished;
        conversationInstance.GetComponent<Conversation>().canceled -= OnGreetingCanceled;
        conversationInstance.GetComponent<Conversation>().HideConversation();

        choicePickerInstance.GetComponent<ChoicePicker>().ShowChoicePicker
                                    (LandscapeContainer.Instance.GetChoiceAPosition(),
                                     LandscapeContainer.Instance.GetChoiceBPosition(),
                                     "How may I help?",
                                     "Buy", "Sell");

        choicePickerInstance.GetComponent<ChoicePicker>().choiceA += OnConfirmBuy;
        choicePickerInstance.GetComponent<ChoicePicker>().choiceB += OnConfirmSell;
        choicePickerInstance.GetComponent<ChoicePicker>().canceled += OnBuySellCancelled;
    }

    private void OnGreetingCanceled()
    {
        conversationInstance.GetComponent<Conversation>().finished -= OnGreetingFinished;
        conversationInstance.GetComponent<Conversation>().canceled -= OnGreetingCanceled;
        conversationInstance.GetComponent<Conversation>().HideConversation();

        ExitStore();
    }

    private void OnConfirmBuy()
    {
        choicePickerInstance.GetComponent<ChoicePicker>().choiceA -= OnConfirmBuy;
        choicePickerInstance.GetComponent<ChoicePicker>().choiceB -= OnConfirmSell;
        choicePickerInstance.GetComponent<ChoicePicker>().canceled -= OnBuySellCancelled;
        choicePickerInstance.GetComponent<ChoicePicker>().Hide();


        StoreContainer.Instance.Show();

        itemListerInstance.GetComponent<ItemLister>().DisplayItems(
                                StoreContainer.Instance.GetCaretStartPosition(),
                                storeItems);
        itemListerInstance.GetComponent<ItemLister>().itemSelected += OnItemSelected;
        itemListerInstance.GetComponent<ItemLister>().canceled += OnItemSelectionCancelled;
    }

    private void OnConfirmSell()
    {
        choicePickerInstance.GetComponent<ChoicePicker>().choiceA -= OnConfirmBuy;
        choicePickerInstance.GetComponent<ChoicePicker>().choiceB -= OnConfirmSell;
        choicePickerInstance.GetComponent<ChoicePicker>().canceled -= OnBuySellCancelled;

        ExitStore();
    }

    private void OnBuySellCancelled()
    {
        choicePickerInstance.GetComponent<ChoicePicker>().choiceA -= OnConfirmBuy;
        choicePickerInstance.GetComponent<ChoicePicker>().choiceB -= OnConfirmSell;
        choicePickerInstance.GetComponent<ChoicePicker>().canceled -= OnBuySellCancelled;

        ExitStore();
    }

    private void OnItemSelected(int itemIdx)
    {
        choicePickerInstance.GetComponent<ChoicePicker>().ShowChoicePicker(
                                    LandscapeContainer.Instance.GetChoiceAPosition(),
                                    LandscapeContainer.Instance.GetChoiceBPosition(),
                                    "Buy this?",
                                    "Yes", "No");
        choicePickerInstance.GetComponent<ChoicePicker>().choiceA += OnConfirmPurchase;
        choicePickerInstance.GetComponent<ChoicePicker>().choiceB += OnCancelPurchase;
    }

    private void OnItemSelectionCancelled(int itemIdx)
    {
        itemListerInstance.GetComponent<ItemLister>().itemSelected -= OnItemSelected;
        itemListerInstance.GetComponent<ItemLister>().canceled -= OnItemSelectionCancelled;
        itemListerInstance.GetComponent<ItemLister>().RecycleAll();

        ExitStore();
    }

    private void OnConfirmPurchase()
    {

    }

    private void OnCancelPurchase()
    {

    }

    private void ExitStore()
    {
        TextPool.Instance.DeactivateAndAddToPool(conversationInstance);
        TextPool.Instance.DeactivateAndAddToPool(choicePickerInstance);
        TextPool.Instance.DeactivateAndAddToPool(itemListerInstance);

        Player.Instance.GetComponent<Player>().Unpause();
        PortraitContainer.Instance.Hide();
        LandscapeContainer.Instance.Hide();
        StoreContainer.Instance.Hide();
        started = false;
    }
}