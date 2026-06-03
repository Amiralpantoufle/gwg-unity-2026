using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.Audio.ProcessorInstance;

public class BootStrap_Loader : MonoBehaviour
{
    public static BootStrap_Loader Instance;
    public bool isLoaded;

    private void Awake()
    {
        Instance = this;
    }
     
    /// <summary>
    /// Initialise le chargement auto a l'affichage de l'écran MainView
    /// </summary>
    public async Task Init_BootStrap()
    {
        await LoadBaseIndex();

        if (GameDataStorage.Instance.CurrentBase != null)
        {
            isLoaded = true;
        }
        else
        {
            isLoaded = false;
            Debug.LogError("No current base found");

            //DECONEXION ?
        }
    }
    private async Task LoadBaseIndex()
    {
        string json = await API_Client.Instance.GetAsync("/base/index");
        if (string.IsNullOrEmpty(json))
        {
            Debug.LogError("BaseIndex json is null");
            return;
        }

        //Construction d'une liste des bases du joueur
        ApiResponse<List<BaseIndexOutput>> response = JsonConvert.DeserializeObject<ApiResponse<List<BaseIndexOutput>>>(json);
        if (response == null)
        {
            Debug.LogError("Impossible de parser BaseIndex");
            return;
        }
        if (response.error)
        {
            Debug.LogError($"API ERROR : {response.error_code} - {response.error_msg}");
            return;
        }
        if (response.output == null)
        {
            Debug.LogError("BaseIndex output null");
            return;
        }

        GameDataStorage.Instance.SetBaseIndexData(response.output);
        //
    }
}