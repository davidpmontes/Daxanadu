using DarkTonic.MasterAudio;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EnemyDamageConfig", order = 1)]
public class EnemyDamageConfig : ScriptableObject
{

    [SoundGroupAttribute]
    public string damageSound;

    [SoundGroupAttribute]
    public string deathSound;

    public GameObject coinPrefab;

    public int life;
}
