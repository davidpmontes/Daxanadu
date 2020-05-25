using UnityEngine;

public class Magic : MonoBehaviour
{
    [SerializeField] private GameObject fireball = default;

    public void OnMagicStart()
    {
        Instantiate(fireball, transform);
    }
}
