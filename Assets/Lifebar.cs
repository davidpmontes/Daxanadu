using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifebar : MonoBehaviour
{
    [SerializeField] private GameObject greenBar;

    public void OnPlayerDamage()
    {
        greenBar.transform.localScale = new Vector3(Player.Instance.GetLifePercentage(), 1, 1);
    }
}
