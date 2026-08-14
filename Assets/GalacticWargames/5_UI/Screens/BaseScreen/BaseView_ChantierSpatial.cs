using System.Buffers.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseView_ChantierSpatial : BaseView_BuildingScreen
{
    [SerializeField] private Transform spaceShipAsset_Root;
    [SerializeField] private GameObject spaceShipAsset_Prefab;

    private void OnEnable()
    {
        Load_SpaceShipsData();
    }

    private async void Load_SpaceShipsData()
    {
        Clear_AvailableBuildings();

        //Load Spaceship List
        SpaceShips_Model spaceShipModel = await Load_SpaceShipList(GameDataStorage.Instance._CurrentBase.base_id);

        if (spaceShipModel.ships.Length <= 0) Debug.Log("NO SPACESHIPS AVAILABLE TO BUILD");

        foreach (spaceShip_Construct s in spaceShipModel.ships)
        {
            Base_SpaceShipAsset asset = Instantiate(spaceShipAsset_Prefab, spaceShipAsset_Root).GetComponent<Builder_QueueSelector>()._Asset;

            //Init Values
            asset.Load_spaceShipInfo(s);
        }

        //Construct queue
        ConstructQueue_Model globalQueue = await Load_ConstructQueue(GameDataStorage.Instance._CurrentBase.base_id);
        ConstructQueue_Building[] ship_Queue = globalQueue.ships;


    }
    private async Task<SpaceShips_Model> Load_SpaceShipList(int id)
    {
        string endpoint = $"/base/ships/{id}";
        var response = await API_Client.Instance.LoadApiResponse<SpaceShips_Model>(endpoint);

        return response.output;
    }
    private async Task<ConstructQueue_Model> Load_ConstructQueue(int id)
    {
        string endpoint = $"/construction/queue/{id}";
        var response = await API_Client.Instance.LoadApiResponse<ConstructQueue_Model>(endpoint);

        return response.output;
    }

    private void Clear_AvailableBuildings()
    {
        if(spaceShipAsset_Root.childCount >0)
        {

            foreach (GameObject child in spaceShipAsset_Root)
                Destroy(child);
        }
    }
}
