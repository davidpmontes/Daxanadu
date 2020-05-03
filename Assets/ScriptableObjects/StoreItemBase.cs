using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/StoreItem", order = 1)]
public class StoreItemBase : ScriptableObject
{
    public string menuName;
    public string[] description;
    public Sprite sprite;
    public int cost;
    public int attackPower;
    public int defensePower;
    public int healingPower;
    public int durationTime;
}
