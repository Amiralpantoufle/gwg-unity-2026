
using System.Collections.Generic;
using UnityEngine;

public class AudioLayer : MonoBehaviour
{
    public enum SoundLayer { Music, Voices, Effects, Interface }
    public SoundLayer layerType;

    [Range(0f, 1f)]
    public float layerVolume = 1f;

    public bool layerEnabled = true;

    public List<CustomSound> sounds = new();

    public void Initialize()
    {
        foreach (CustomSound sound in sounds)
        {
            if (sound.clip == null)
            {
                Debug.LogWarning(
                    $"AudioLayer {layerType}: clip manquant pour {sound.name}");
                continue;
            }

            sound.source = gameObject.AddComponent<AudioSource>();

            sound.source.clip = sound.clip;
            sound.source.loop = sound.loop;
            sound.source.volume = sound.volume * layerVolume;
            sound.source.playOnAwake = false;
        }
    }

    public bool Play(string soundName)
    {
        if (!layerEnabled)
            return false;

        CustomSound sound = sounds.Find(
            s => s.name == soundName);

        if (sound == null || sound.source == null)
            return false;

        sound.source.volume = sound.volume * layerVolume;

        if (sound.loop)
        {
            if (!sound.source.isPlaying)
                sound.source.Play();
        }
        else
        {
            sound.source.PlayOneShot(sound.clip);
        }

        return true;
    }

    public void SetVolume(float volume)
    {
        layerVolume = Mathf.Clamp01(volume);

        foreach (CustomSound sound in sounds)
        {
            if (sound.source != null)
                sound.source.volume = sound.volume * layerVolume;
        }
    }

    public void SetEnabled(bool enabled)
    {
        layerEnabled = enabled;

        if (!enabled)
        {
            foreach (CustomSound sound in sounds)
            {
                if (sound.source != null)
                    sound.source.Stop();
            }
        }
    }
}
