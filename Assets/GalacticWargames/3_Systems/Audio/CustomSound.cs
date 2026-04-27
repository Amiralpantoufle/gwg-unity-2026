using UnityEngine;

[System.Serializable]
public class CustomSound
{
    public string name;
    public AudioClip clip;
    public enum SoundLayer { Music, Voices, Effects, misc}
    public SoundLayer soundLayer;

    [Range(0f,1f)]
    public float volume;
    public bool loop;

    [HideInInspector]
    public AudioSource source;

}
