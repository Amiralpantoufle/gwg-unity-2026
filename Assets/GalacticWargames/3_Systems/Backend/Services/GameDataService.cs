using UnityEngine;

/// <summary>
/// récupérer les données du jeu (civilisations etc)
/// </summary>
public class GameDataService : MonoBehaviour
{
    public static GameDataService Instance;

    public GlobalDataOutput CachedData;

    private void Awake()
    {
        Instance = this;
    }

    public void LoadGlobalData()
    {
        StartCoroutine(API_Client.Instance.Get("/data/getGlobalData", OnDataReceived));
    }

    private void OnDataReceived(string json)
    {
        Debug.Log("GLOBAL DATA: " + json);

        GlobalDataResponse response = JsonUtility.FromJson<GlobalDataResponse>(json);

        if (!response.error)
        {
            CachedData = response.output;

            Debug.Log("DATA LOADED");

            // utilisation directe des données

        }
        else
        {
            Debug.LogError("ERROR DATA");
        }
    }
}
