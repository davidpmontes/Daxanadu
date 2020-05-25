using UnityEngine;

public class MagicBar : MonoBehaviour
{
    [SerializeField] private GameObject redBar = default;

    public void SetMagicBarToPlayer()
    {
        redBar.transform.localScale = new Vector3(Player.Instance.GetMagicPercentage(), 1, 1);
    }
}
