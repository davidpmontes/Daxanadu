using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/StoreItem", order = 1)]
public class StoreItemBase : ScriptableObject
{
    public string menuName;
    public Sprite smallSprite;
    public Sprite largeSprite;
}
