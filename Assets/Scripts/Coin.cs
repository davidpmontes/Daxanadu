using UnityEngine;
using DarkTonic.MasterAudio;

[RequireComponent(typeof(BoxCollider2D))]
public class Coin : MonoBehaviour
{
    [SoundGroupAttribute] public string coinSound;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        MasterAudio.PlaySoundAndForget(coinSound);
        Destroy(gameObject);
    }
}
