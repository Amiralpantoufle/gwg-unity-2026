using Newtonsoft.Json;
using TMPro;
using UnityEngine;

/// <summary>
/// Composant héritage pour les écrans de base avec les fonctions génériques
/// </summary>
public class BaseView_BuildingScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gui_slotName;
    [SerializeField] private buildingList buildingType;
    [SerializeField] private int building_id;
    public int _Building_id { get {  return (building_id); } set { building_id = value; Debug.Log("Set value to : " + value); }  }
    

    public void Upgrade_Building()
    {
        BaseView_Popup baseView = FindAnyObjectByType<BaseView_Popup>();

        if (baseView != null && building_id != 0)
        {
            baseView.UpgradeBuilding(buildingType, building_id);
        }
    }
    public void Destroy_Building()
    {
        Debug.Log("Destroy Building !" + building_id);

        BuildingDestructionRequest request = new BuildingDestructionRequest
        {
            id_oes = GameDataStorage.Instance.GetLastBaseId(),
            id_batiment = building_id
        };

        string json = JsonUtility.ToJson(request);

        StartCoroutine(API_Client.Instance.Post("/construction/delete-building", json, OnDestructionQueued<BuildingDestructionResponse>));
    }
    private void OnDestructionQueued<T>(string response)
    {
        ApiResponse<T> request = JsonConvert.DeserializeObject<ApiResponse<T>>(response);
        
        if (request != null)
        {
            Debug.Log(request);

            if (!request.error)
            {
                ToastManager.Instance.GenerateToast( "Building Destroyed",1,2f);
                FindAnyObjectByType<BaseView_Popup>().Close_AllPannels();
            }
            else
                ToastManager.Instance.GenerateToast("Couldn't Destroy Building", 0, 2f);
        }
        else
            ToastManager.Instance.GenerateToast("Invalid building data", 0, 2f);

    }
}
