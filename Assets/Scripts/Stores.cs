using UnityEngine;
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

            greetingInstance.GetComponent<Conversation>().Finished += OnGreetingFinished;
            greetingInstance.GetComponent<Conversation>().Canceled += OnGreetingCanceled;

            Player.Instance.GetComponent<Player>().Pause();
            PortraitContainer.Instance.Show();
            LandscapeContainer.Instance.Show();
        }
    }

    private void OnGreetingCanceled()
    {
        greetingInstance.GetComponent<Conversation>().Finished -= OnGreetingFinished;
        greetingInstance.GetComponent<Conversation>().Canceled -= OnGreetingCanceled;
        greetingInstance.GetComponent<Conversation>().HideConversation();

        Player.Instance.GetComponent<Player>().Unpause();
        PortraitContainer.Instance.Hide();
        LandscapeContainer.Instance.Hide();
        started = false;
    }

    private void OnGreetingFinished()
    {
        greetingInstance.GetComponent<Conversation>().Finished -= OnGreetingFinished;
        greetingInstance.GetComponent<Conversation>().ClearConversation();
        buySellPickerInstance = TextPool.Instance.GetFromPoolInactive(TextPool.TextPools.ChoicePicker);
        buySellPickerInstance.SetActive(true);
        buySellPickerInstance.GetComponent<ChoicePicker>().ShowChoicePicker
                                    (LandscapeContainer.Instance.GetChoiceAPosition(),
                                     LandscapeContainer.Instance.GetChoiceBPosition(),
                                     "How may I help?",
                                     "Buy", "Sell");

        buySellPickerInstance.GetComponent<ChoicePicker>().choiceA += OnConfirmBuy;
        buySellPickerInstance.GetComponent<ChoicePicker>().choiceB += OnConfirmSell;
        buySellPickerInstance.GetComponent<ChoicePicker>().canceled += OnBuySellCancelled;
    }

    private void OnConfirmBuy()
    {
        buySellPickerInstance.GetComponent<ChoicePicker>().choiceA -= OnConfirmBuy;
        buySellPickerInstance.GetComponent<ChoicePicker>().choiceB -= OnConfirmSell;
        buySellPickerInstance.GetComponent<ChoicePicker>().canceled -= OnBuySellCancelled;
        TextPool.Instance.DeactivateAndAddToPool(buySellPickerInstance);
        StoreContainer.Instance.Show();

        itemLister = TextPool.Instance.GetFromPoolInactive(TextPool.TextPools.ItemLister);
        itemLister.SetActive(true);
        itemLister.GetComponent<ItemLister>().DisplayItems(StoreContainer.Instance.GetCaretStartPosition(),
                                                           storeItems);
        itemLister.GetComponent<ItemLister>().itemSelected += OnItemSelected;
        itemLister.GetComponent<ItemLister>().canceled += OnItemSelectionCancelled;
    }

    private void OnConfirmSell()
    {

    }

    private void OnBuySellCancelled()
    {
        buySellPickerInstance.GetComponent<ChoicePicker>().choiceA -= OnConfirmBuy;
        buySellPickerInstance.GetComponent<ChoicePicker>().choiceB -= OnConfirmSell;
        buySellPickerInstance.GetComponent<ChoicePicker>().canceled -= OnBuySellCancelled;
        TextPool.Instance.DeactivateAndAddToPool(buySellPickerInstance);
        Player.Instance.GetComponent<Player>().Unpause();
        PortraitContainer.Instance.Hide();
        LandscapeContainer.Instance.Hide();
        started = false;
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
        confirmPurchasePickerInstance.GetComponent<ChoicePicker>().choiceA += OnConfirmPurchase;
        confirmPurchasePickerInstance.GetComponent<ChoicePicker>().choiceB += OnCancelPurchase;
    }

    private void OnItemSelectionCancelled(int itemIdx)
    {
        itemLister.GetComponent<ItemLister>().itemSelected -= OnItemSelected;
        itemLister.GetComponent<ItemLister>().canceled -= OnItemSelectionCancelled;
        Player.Instance.GetComponent<Player>().Unpause();
        PortraitContainer.Instance.Hide();
        LandscapeContainer.Instance.Hide();
        StoreContainer.Instance.Hide();
        started = false;
    }

    private void OnConfirmPurchase()
    {

    }

    private void OnCancelPurchase()
    {

    }
}