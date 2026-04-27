using UnityEngine;

/// <summary>
/// garder le token même après fermeture du jeu
/// </summary>
public static class TokenStorage
{
    private const string ACCESS_KEY = "ACCESS_TOKEN";
    private const string REFRESH_KEY = "REFRESH_TOKEN";

    public static void Save(string access, string refresh)
    {
        PlayerPrefs.SetString(ACCESS_KEY, access);
        PlayerPrefs.SetString(REFRESH_KEY, refresh);
    }

    public static string GetAccess()
    {
        return PlayerPrefs.GetString(ACCESS_KEY, "");
    }

    public static string GetRefresh()
    {
        return PlayerPrefs.GetString(REFRESH_KEY, "");
    }

    public static void Clear()
    {
        PlayerPrefs.DeleteKey(ACCESS_KEY);
        PlayerPrefs.DeleteKey(REFRESH_KEY);
    }
}