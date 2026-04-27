using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public CustomSound[] sounds;

    private void Awake()
    {
        Instance = this;

        foreach (CustomSound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();

            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;

        }
    }

    public void PlaySound(string soundName)
    {
        CustomSound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            s.source.Play();
        }
    }
}
