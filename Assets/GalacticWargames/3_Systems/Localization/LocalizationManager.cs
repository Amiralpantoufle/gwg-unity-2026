using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stocke la langue active. Retourne le bon texte. Fallback si clé manquante
/// Changement de langue : Refresh tous les textes via EventBus =>  LocalizationManager.Instance.LoadLanguage("fr")
/// </summary>
public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance;
    private const string ACCESS_KEY = "LANGUAGE";

    public List<LocalizationData> languages;
    public string currentLanguage;

    private Dictionary<string, string> localizedDict;

    private void Awake()
    {
        Instance = this;

        string prefs = PlayerPrefs.GetString(ACCESS_KEY, "en");
        LoadLanguage(prefs);
    }

    /// <summary>
    /// Récupère la langue à charger dans la liste _languages et construit le dictionnaire de langue à utiliser dans le moteur
    /// </summary>
    /// <param name="languageCode"></param>
    public void LoadLanguage(string languageCode)
    {
        var data = languages.Find(l => l.languageCode == languageCode);

        if (data == null)
        {
            Debug.LogError($"Language not found: {languageCode}");
            return;
        }

        localizedDict = new Dictionary<string, string>();

        foreach (var entry in data.entries)
        {
            localizedDict[entry.key] = entry.value;
        }

        SaveLanguagePrefs();
    }
    private void SaveLanguagePrefs()
    {
        PlayerPrefs.SetString(ACCESS_KEY,currentLanguage);
    }
    /// <summary>
    /// Récupère une valeur text traduite par sa clé d'accès
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public string Get(string key)
    {
        if (localizedDict.TryGetValue(key, out var value))
            return value;

        return $"[{key}]"; // fallback debug
    }
}
