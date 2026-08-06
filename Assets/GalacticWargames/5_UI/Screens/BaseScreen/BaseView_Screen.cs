using System.Buffers.Text;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class BaseView_Screen : UIScreen
{
    [SerializeField] private GameObject gameView;
    private int baseId;

    //Ressources
    private RessourceModule availableRessources;
    public RessourceModule _AvailableRessources { get { return availableRessources; } }

    [SerializeField] private TextMeshProUGUI carbon_Quantity;
    [SerializeField] private TextMeshProUGUI hydrogen_Quantity;
    [SerializeField] private TextMeshProUGUI energyStone_Quantity;
    [SerializeField] private TextMeshProUGUI stockCapacity, stockCurrent;

    BaseInfo_Model baseData;
    building_Construct[] buildings;

    public async override void Show()
    {
        base.Show();
        gameView.SetActive(true);
        availableRessources = GetComponent<RessourceModule>();

        GridManager.OnSwitchToWorld += OpenWorldScreen;

        await Display_BaseInfos();

        //Load Building List
        BaseBuildings_Model buildingModel = await Load_BuildingList(baseId);
        buildings = buildingModel.buildings;
        popupMaster.GetComponent<BaseView_Popup>()._SelectionPannel.Load_AvailableBuildings(buildings);
    }
    public override void Hide()
    {
        base.Hide();
        gameView.SetActive(false);
    }

    private async Task Display_BaseInfos()
    {
        baseId = GameDataStorage.Instance.GetLastBaseId();
        if (baseId == 0) Debug.LogError("NO BASE ID LOADED");

        //Load Info
        baseData = await Load_BaseInfos(baseId);

        int[] r = new int[3];

        r[0] = (int)baseData.ressources[0].nombre_oer;
        r[1] = (int)baseData.ressources[1].nombre_oer;
        r[2] = (int)baseData.ressources[2].nombre_oer;

        availableRessources.RefreshRessources(r);

        carbon_Quantity.text = r[0].ToString();
        hydrogen_Quantity.text = r[1].ToString();
        energyStone_Quantity.text = r[2].ToString();

        stockCurrent.text = (r[0] + r[1] + r[2]).ToString();
    }
    private async Task<BaseInfo_Model> Load_BaseInfos(int id)
    {
        string endpoint = $"/base/show/{id}";
        var response = await API_Client.Instance.LoadApiResponse<BaseInfo_Model>(endpoint);

        return response.output;
    }
    private async Task<BaseBuildings_Model> Load_BuildingList(int id)
    {
        string endpoint = $"/base/buildings/{id}";
        var response = await API_Client.Instance.LoadApiResponse<BaseBuildings_Model>(endpoint);

        return response.output;
    }


    //Utility
    private void OpenWorldScreen()
    {
        EventBus.Publish(new ReplaceScreenEvent
        {
            screenID = ScreenID.Main
        });
    }
    public async void LeaveBaseView()
    {
        await GridManager.Instance.Load(GridLevel.Planet, GameDataStorage.Instance._CurrentBase.position.entity_id);
    }
}
