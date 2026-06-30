using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;
using static UnityEngine.Analytics.IAnalytic;
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
            Debug.Log("Bootstrap Process Ended correctly");
        }
        else
        {
            isLoaded = false;
            Debug.Log("Couldn't Init BootStrap Process");
        }
    }
    private async Task LoadBaseIndex()
    {
        string json = await API_Client.Instance.GetAsync("/base/index");
        if (string.IsNullOrEmpty(json))
            return;

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
            Debug.Log("BaseIndex output null");
            return;
        }

        GameDataStorage.Instance.LoadCurrentBaseData(FindBaseToDisplay(response.output));
    }
    private BaseIndexOutput FindBaseToDisplay(List<BaseIndexOutput> dataList)
    {
        //Protection
        if (dataList.Count <= 0 || dataList[0] == null)
        {
            Debug.LogError("No data found in list");
            return null;
        }

        //Selection d'une base
        BaseIndexOutput firstData;

        if (dataList.Count > 1)
        {
            firstData = dataList.LastOrDefault();
        }
        else
        {
            firstData = dataList[0];
        }

        return firstData;
    }

}