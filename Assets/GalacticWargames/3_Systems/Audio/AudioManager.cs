
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField]
    private List<AudioLayer> audioLayers = new();

    private Dictionary<AudioLayer.SoundLayer, AudioLayer> layerLookup = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioLayers = new List<AudioLayer>(GetComponentsInChildren<AudioLayer>(true));

        foreach (AudioLayer layer in audioLayers)
        {
            if (layerLookup.ContainsKey(layer.layerType))
            {
                Debug.LogError($"AudioM: couche dupliquée : {layer.layerType}");
                continue;
            }

            layer.Initialize();
            layerLookup.Add(layer.layerType, layer);
        }
    }

    public void Play(string soundName)
    {
        foreach (AudioLayer layer in audioLayers)
        {
            if (layer.Play(soundName))
                return;
        }

        Debug.LogWarning(
            $"AudioM: son introuvable ou indisponible : {soundName}");

        return;
    }

    public bool Play(string soundName, AudioLayer.SoundLayer layerType)
    {
        if (!layerLookup.TryGetValue(layerType, out AudioLayer layer))
        {
            Debug.LogWarning($"AudioM: couche introuvable : {layerType}");
            return false;
        }

        if (layer.sounds.Exists(s => s.name == soundName))
            return layer.Play(soundName);

        Debug.LogWarning($"AudioM: son {soundName} absent de la couche {layerType}");

        return false;
    }

    public void SetLayerVolume(AudioLayer.SoundLayer layerType,float volume)
    {
        if (layerLookup.TryGetValue(layerType, out AudioLayer layer))
            layer.SetVolume(volume);
    }

    public void SetLayerEnabled(AudioLayer.SoundLayer layerType,bool enabled)
    {
        if (layerLookup.TryGetValue(layerType, out AudioLayer layer))
            layer.SetEnabled(enabled);
    }
}
