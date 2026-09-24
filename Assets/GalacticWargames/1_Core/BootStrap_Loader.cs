using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

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

        if (GameDataStorage.Instance._CurrentBase != null)
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
        var response = await API_Client.Instance.LoadApiResponse<BaseIndexOutput>("/base/index");

        if (response == null || response.error || response.output == null)
            return;

        GameDataStorage.Instance.LoadCurrentBaseData(FindBaseToDisplay(response.output.bases));

    }
    private BaseOutput FindBaseToDisplay(List<BaseOutput> dataList)
    {
        //Protection
        if (dataList.Count <= 0 || dataList[0] == null)
        {
            Debug.LogWarning("No Base data found in list");
            return null;
        }

        //Selection d'une base
        BaseOutput firstData;

        if (dataList.Count > 1)
        {
            Debug.Log("Multiple bases found");
            firstData = dataList.LastOrDefault();
        }
        else
            firstData = dataList[0];

        return firstData;
    }

}