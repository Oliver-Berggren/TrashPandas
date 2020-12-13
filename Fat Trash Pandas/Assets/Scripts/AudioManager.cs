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
            sound.audioSource.loop = sound.loop;
        }
    }

    public void Play(string name)
    {
        foreach(Sound sound in sounds)
        {
            if(sound.name == name)
            {
                if(sound.audioSource.clip != null)
                {
                    sound.audioSource.Play();
                }
                else
                {
                    Debug.Log(name + " audio clip null");
                }
                return;
            }
        }
        Debug.Log(name + " sound object not found in audio manager");
    }

    public void Stop(string name)
    {
        foreach(Sound sound in sounds)
        {
            if(sound.name == name)
            {
                if(sound.audioSource.clip != null)
                {
                    sound.audioSource.Stop();
                }
                else
                {
                    Debug.Log(name + " audio clip null");
                }
                return;
            }
        }
        Debug.Log(name + " sound object not found in audio manager");
    }
}
