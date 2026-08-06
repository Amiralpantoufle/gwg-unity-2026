using System.Buffers.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseView_ChantierSpatial : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gui_slotName;

    [SerializeField] private Transform spaceShipAsset_Root;
    [SerializeField] private GameObject spaceShipAsset_Prefab;

    private void OnEnable()
    {
        Load_AvailableSpaceShips();
    }
    private async void Load_AvailableSpaceShips()
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
    }
    public void Upgrade_Building()
    {
        BaseView_Popup baseView = FindAnyObjectByType<BaseView_Popup>();

        if (baseView != null)
        {
            baseView.UpgradeBuilding(buildingList.ChantierSpatial);
        }
    }

    //Utility
    private async Task<SpaceShips_Model> Load_SpaceShipList(int id)
    {
        string endpoint = $"/base/ships/{id}";
        var response = await API_Client.Instance.LoadApiResponse<SpaceShips_Model>(endpoint);

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
