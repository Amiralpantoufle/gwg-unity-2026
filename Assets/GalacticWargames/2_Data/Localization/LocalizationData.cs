using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Constructeur de la logique Multi-Langues. || Label = ScreeName_Function_stringID
/// </summary>
[CreateAssetMenu(menuName = "Localization/Data")]
public class LocalizationData : ScriptableObject
{
    public string languageCode; // "en", "fr" ...
    public List<LocalizationEntry> entries;
}
[System.Serializable]
public class LocalizationEntry
{
    public string key;
    public string value;
}
