using UnityEngine;

public class Lifebar : MonoBehaviour
{
    [SerializeField] private GameObject greenBar;

    public void SetLifebarToPlayer()
    {
        greenBar.transform.localScale = new Vector3(Player.Instance.GetLifePercentage(), 1, 1);
    }
}
