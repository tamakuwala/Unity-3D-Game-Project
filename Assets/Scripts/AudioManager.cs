using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public SoundClass[] sounds;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(instance);


        foreach (SoundClass s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.pitch = s.pitch;
            s.source.volume = s.volume;
        }
    }

    public void Play(string name)
    {
        var s = Array.Find(sounds, soundClass => soundClass.name == name);
        s.source.Play();
    }

    public void PlayAndPitch(string name, float pitch)
    {
        var s = Array.Find(sounds, soundClass => soundClass.name == name);
        s.source.pitch = pitch;
        s.source.Play();
    }
}
