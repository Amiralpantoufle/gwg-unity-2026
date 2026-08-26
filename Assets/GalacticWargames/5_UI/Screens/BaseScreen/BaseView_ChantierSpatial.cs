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

    private ConstructQueue_SpaceShip[] spaceShip_BuildQueue;

    private void OnEnable()
    {
        Load_SpaceShipsData();
    }

    private async void Load_SpaceShipsData()
    {
        Clear_AvailableBuildings();

        //Load les vaisseaux dispos à la construction
        SpaceShips_Model spaceShipModel = await Load_SpaceShipList(GameDataStorage.Instance._CurrentBase.base_id);
        if (spaceShipModel.ships.Length <= 0) Debug.Log("NO SPACESHIPS AVAILABLE TO BUILD");

        //Load la liste des vaisseaux en construction
        ConstructQueue_Model globalQueue = await Load_ConstructQueue(GameDataStorage.Instance._CurrentBase.base_id);
        spaceShip_BuildQueue = globalQueue.ships;

        Dictionary<int, ConstructQueue_SpaceShip> shipsInConstruction = new Dictionary<int, ConstructQueue_SpaceShip>();
        foreach (ConstructQueue_SpaceShip ship in spaceShip_BuildQueue)
        {
            shipsInConstruction[ship.ship_id] = ship;
        }

        //Display Screen UI DATA
        foreach (spaceShip_Construct s in spaceShipModel.ships)
        {
            Builder_QueueSelector queuer = Instantiate(spaceShipAsset_Prefab, spaceShipAsset_Root).GetComponent<Builder_QueueSelector>();
            queuer._Asset.Load_spaceShipInfo(s, 0);

            if (shipsInConstruction.TryGetValue(s.id_vas, out ConstructQueue_SpaceShip queue))
                queuer._Queue.Reload_Queue(queue.remaining, s.vitesse_construction_vas, queue.remaining_batch_seconds);
        }
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
        if (spaceShipAsset_Root.childCount > 0)
        {

            foreach (Transform child in spaceShipAsset_Root)
                Destroy(child.gameObject);
        }
    }
    private int GetShipsInConstruction(Dictionary<int, int> shipsInConstruction,int shipId)
    {
        if (shipsInConstruction.TryGetValue(shipId, out int quantity))
            return quantity;

        return 0;
    }
}
