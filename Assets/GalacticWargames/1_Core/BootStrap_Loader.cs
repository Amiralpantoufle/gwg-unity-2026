using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.Audio.ProcessorInstance;

public class BootStrap_Loader : MonoBehaviour
{
    public static BootStrap_Loader Instance;

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Initialise le chargement auto a l'affichage de l'écran MainView
    /// </summary>
    public async void Init_BootStrap()
    {
        await LoadBaseIndex();

        if (GameDataStorage.Instance.CurrentBase != null)
        {
            //await LoadPlanet();

            int planetId = GameDataStorage.Instance.CurrentBase.PlanetId;
            await GridManager.Instance.Load(GridLevel.Planet, planetId);

            Debug.Log("Planet Loaded");
        }
        else
        {
            Debug.LogError("No current base found");
        }
    }

    //================================================
    // BASE INDEX
    //================================================
    private async Task LoadBaseIndex()
    {
        string json = await API_Client.Instance.GetAsync("/base/index");

        if (string.IsNullOrEmpty(json))
        {
            Debug.LogError("BaseIndex json is null");
            return;
        }

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
        Debug.Log("Base Index Stored");
    }

    //================================================
    // PLANET
    //================================================
    private async Task LoadPlanet()
    {
        int planetId = GameDataStorage.Instance.CurrentBase.PlanetId;

        string json = await API_Client.Instance.GetAsync($"/map/planet/{planetId}");

        if (string.IsNullOrEmpty(json))
        {
            Debug.LogError("Planet json is null");
            return;
        }

        PlanetMapResponse response = JsonConvert.DeserializeObject<PlanetMapResponse>(json);

        if (response == null)
        {
            Debug.LogError("Impossible de parser PlanetMap");
            return;
        }

        if (response.error)
        {
            Debug.LogError($"Planet API ERROR : {response.error} - {response.error}");
            return;
        }

        if (response.output == null)
        {
            Debug.LogError("Planet output null");
            return;
        }

        // Render map
        //await GridManager.Instance.Load(GridLevel.Planet, planetId);

        Debug.Log("Planet Loaded");
    }
}