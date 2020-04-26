using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [SoundGroupAttribute] public string town1;

    private void Start()
    {
        MasterAudio.PlaySoundAndForget(town1);
    }
}
