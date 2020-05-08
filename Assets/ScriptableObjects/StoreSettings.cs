using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Store", order = 1)]
public class StoreSettings : ScriptableObject
{
    public StoreItem[] items;
    public string owner;
    public Vector2 playerDestination;
    public Vector2 cameraDestination;
    public string[] initialGreetingText = new string[]
    {
        //"Welcome!^-->",
        //"Welcome!^END",
    };

    public string[] buySellGreetingText = new string[]
    {
        //"How may I help?"
    };

    public string[] purchaseConfirmedText = new string[]
    {
        //"Purchased!"
    };

    public string[] purchaseCanceledText = new string[]
    {
        //"Cancelled"
    };

    public string[] purchaseUnaffordableText = new string[]
    {
        //"I am sorry, you",
        //"do not have enough",
        //"money."
    };
}
