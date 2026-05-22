using System;
using UnityEngine;

public static class DeviceManager
{
    private const string DEVICE_KEY = "DEVICE_ID";

    /// <summary>
    /// Retourne l'identifiant persistant de cette installation.
    /// </summary>
    public static string GetDeviceId()
    {
        if (!PlayerPrefs.HasKey(DEVICE_KEY))
        {
            string newId = Guid.NewGuid().ToString();

            PlayerPrefs.SetString(DEVICE_KEY, newId);
            PlayerPrefs.Save();
        }

        return PlayerPrefs.GetString(DEVICE_KEY);
    }

    /// <summary>
    /// Réinitialise l'identifiant device local.
    /// </summary>
    public static void ResetDeviceId()
    {
        PlayerPrefs.DeleteKey(DEVICE_KEY);
        PlayerPrefs.Save();
    }
}