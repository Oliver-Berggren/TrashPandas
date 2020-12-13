using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    // Instance reference
    public static AudioManager instance;

    // Sound effect list
    public List<Sound> sounds = new List<Sound>();

    void Awake()
    {
        instance = this;
        foreach(Sound sound in sounds)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.clip;
        }
    }

    void Play(string name)
    {
        foreach(Sound sound in sounds)
        {
            if(sound.name == name)
            {
                sound.audioSource.Play();
                return;
            }
        }
    }

    void Stop(string name)
    {
        foreach(Sound sound in sounds)
        {
            if(sound.name == name)
            {
                sound.audioSource.Stop();
                return;
            }
        }
    }
}
