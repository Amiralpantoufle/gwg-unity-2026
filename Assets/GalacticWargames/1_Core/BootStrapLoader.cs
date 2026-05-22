using UnityEngine;
using Newtonsoft;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;


public class BootStrapLoader : MonoBehaviour
{
    public static BootStrapLoader Instance;

    private void Awake()
    {
        Instance = this;
    }
    /// <summary>
    /// Récupère les infos jeu/joueur/base 
    /// </summary>
    public void Init_BootStrap()
    {
        StartCoroutine(API_Client.Instance.Get("/base/index", OnBaseIndexResponse));
        //StartCoroutine(API_Client.Instance.Get("/data/getGlobalData", OnGlobalDataResponse));
        //StartCoroutine(API_Client.Instance.Get("/user/getUserData", OnUserDataResponse));
    }
    private void OnBaseIndexResponse(string json)
    {
        Debug.Log("BASE INDEX RESPONSE: " + json); 

        //ApiResponse<BaseIndexOuput[]> response = JsonUtility.FromJson<ApiResponse<BaseIndexOuput[]>>(json);
        ApiResponse<List<BaseIndexOutput>> response = JsonConvert.DeserializeObject<ApiResponse<List<BaseIndexOutput>>>(json);

        //Errors management
        if (response == null)
        {
            Debug.LogError("Impossible de parser la réponse API");
            return;
        }
        if (response.error)
        {
            Debug.LogError($"Erreur API : {response.error_code} - {response.error_msg}");
            return;
        }
        if (response.output == null)
        {
            Debug.LogError("Output vide");
        }

        GameDataStorage.Instance.SetBaseIndexData(response.output);

    }
    private void OnGlobalDataResponse(string json)
    {
        Debug.Log("GLOBAL DATA RESPONSE: " + json);

        ApiResponse<GlobalDataOutput> response = JsonUtility.FromJson<ApiResponse<GlobalDataOutput>>(json);

        if (response == null)
        {
            Debug.LogError("Impossible de parser la réponse API");
            return;
        }

        if (response.error)
        {
            Debug.LogError($"Erreur API : {response.error_code} - {response.error_msg}");
            return;
        }

        if (response != null)
            GameDataStorage.Instance.SetGlobalData(response.output);
    }
    private void OnUserDataResponse(string json)
    {
        Debug.Log("USER DATA RESPONSE: " + json);

        ApiResponse<UserDataOutput> response = JsonUtility.FromJson<ApiResponse<UserDataOutput>>(json);

        if (response == null)
        {
            Debug.LogError("Impossible de parser la réponse API");
            return;
        }

        if (response.error)
        {
            Debug.LogError($"Erreur API : {response.error_code} - {response.error_msg}");
            return;
        }

        if (response != null)
            GameDataStorage.Instance.SetUserData(response.output);

    }
}
