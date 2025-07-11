using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundClass
{
    public string name;
    public AudioClip clip;

    [Range(0, 1f)]
    public float volume;
    [Range(0, 3f)]
    public float pitch;

    [HideInInspector]
    public AudioSource source;
}
