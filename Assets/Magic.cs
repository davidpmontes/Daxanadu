using UnityEngine;

public class Magic : MonoBehaviour
{
    [SerializeField] private GameObject fireball;

    public void OnMagicStart()
    {
        Instantiate(fireball, transform);
    }
}
