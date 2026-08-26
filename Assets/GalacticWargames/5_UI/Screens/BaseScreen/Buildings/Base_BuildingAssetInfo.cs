using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class Base_BuildingAssetInfo : MonoBehaviour  
{
    //Components
    [SerializeField] private TextMeshProUGUI GUI_Name;
    [SerializeField] private TextMeshProUGUI GUI_description;
    [SerializeField] private TextMeshProUGUI[] costs;

    [SerializeField] private Button buttonConstruct;

    private building_Construct loadedConstruct;
    private Vector2Int tilePos;

    public void Load_Info(building_Construct construct, Vector2Int newTilePos) 
    {
        loadedConstruct = construct;
        tilePos = newTilePos;

        GUI_Name.text = construct.nom_bat;
        GUI_description.text = construct.desc_bat;

        for (int i = 0; i < costs.Length; i++)
        {
            costs[i].text = construct.couts[i].nombre_bre.ToString();
        }

        gameObject.SetActive(true);
    }

    public void AvailableConstruct(bool isAvailable)
    {
        //Set Visibility
        float alpha;

        //Add Callbacks
        buttonConstruct.enabled = isAvailable;

        if (isAvailable)
        {
            alpha = 1f;
            buttonConstruct.onClick.AddListener(ClickOnBuild);

            Debug.Log("Build Available");
        }
        else
        {
            alpha = 0.15f;

            buttonConstruct.onClick.RemoveAllListeners();
            Debug.Log("Build Not Available");
        }

        //Set Button Visibility
        buttonConstruct.GetComponent<CanvasGroup>().alpha = alpha;
    }

    public void ClickOnBuild()
    {
        BuildingConstructionRequest request = new BuildingConstructionRequest
        {
            id_oes = GameDataStorage.Instance.GetLastBaseId(),
            id_bat = loadedConstruct.id_bat,
            x = tilePos.x,
            y = tilePos.y,
            operation_key = OperationKeyGenerator.Generate("build")
        };

        string json = JsonUtility.ToJson(request);
        StartCoroutine(API_Client.Instance.Post("/construction/building", json, OnConstructionQueued));
    }

    private void OnConstructionQueued(string response)
    {
        //Treat errors
        ApiResponse<string> result = JsonUtility.FromJson<ApiResponse<string>>(response);

        if (result == null)
        {
            Debug.LogError("Réponse de construction invalide.");
            return;
        }

        if (result.error)
        {
            Debug.LogWarning($"Construction impossible : {result.error_code} - {result.error_msg}");
            ToastManager.Instance.GenerateToast("Construction Impossible", 0, 2f);

            return;
        }

        //Success construction
        Debug.Log("Building asset !");
        BaseView_Screen.Instance.popupMaster.GetComponent<BaseView_Popup>().Close_AllPannels();
        Display_TempBuilding();
    }

    private void Display_TempBuilding()
    {
        BaseEntity newEntity = new BaseEntity
        {
            building_id = loadedConstruct.id_bat,
            level = 1,
            building_type = "",
            status = "",
            hp=0,
            hp_max=0
        };

        if(newEntity!= null)
            BaseView_Screen.Instance._Entities_Pool.SpawnBaseEntity(newEntity, tilePos);
    }
}
