using Newtonsoft.Json;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

/// <summary>
/// Composant héritage pour les écrans de base avec les fonctions génériques
/// </summary>
public class BaseView_BuildingScreen : MonoBehaviour 
{
    [SerializeField] private TextMeshProUGUI gui_slotName;
    [SerializeField] private TextMeshProUGUI gui_level;
    [SerializeField] private buildingList buildingType;
    [SerializeField] private int entity_id;
    public int _Entity_id { get {  return (entity_id); } set { entity_id = value; }  }
    [SerializeField] private int building_id;
    public int _Building_id { get {  return (building_id); } set { building_id = value; }  }
    [SerializeField] private int instance_id;
    public int _Instance_id{ get {  return (instance_id); } set { instance_id = value; }  }

    public void Load_BaseInfos(string name, int level)
    {
        gui_slotName.text = name;
        gui_level.text = level.ToString() + "/3";
    }
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
        Debug.Log("Destroy Building !" + instance_id);

        BuildingDestructionRequest request = new BuildingDestructionRequest
        {
            id_oes = GameDataStorage.Instance.GetLastBaseId(),
            id_batiment = instance_id,
            operation_key = OperationKeyGenerator.Generate("build")
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

                //Destroy Entity Visual
                GridManager.Instance.Reload_CurrentMap();

                BaseView_Popup popup= BaseView_Screen.Instance.popupMaster.GetComponent<BaseView_Popup>();
                if(popup != null)
                    popup.Close_AllPannels();
            }
            else
                ToastManager.Instance.GenerateToast("Couldn't Destroy Building", 0, 2f);
        }
        else
            ToastManager.Instance.GenerateToast("Invalid building data", 0, 2f);
    }
}
