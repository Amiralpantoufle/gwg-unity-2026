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

        GridManager.OnSwitchToWorld += OpenWorldScreen;

        await Display_BaseInfos();

        //Load Building List
        BaseBuildings_Model model = await Load_BuildingList(baseId);
        buildings = model.buildings;
        popupMaster.GetComponent<BaseView_Popup>()._SelectionPannel.Load_AvailableBuildings(buildings);
    }
    public override void Hide()
    {
        base.Hide();
        gameView.SetActive(false);
    }

    private async Task Display_BaseInfos()
    {
        baseId = GameDataStorage.Instance.CurrentBase.base_id;
        if (baseId == 0) Debug.LogError("NO BASE ID LOADED");

        //Load Info
        baseData = await Load_BaseInfos(baseId);

        int carbon = (int)baseData.ressources[0].nombre_oer;
        int hydro = (int)baseData.ressources[1].nombre_oer;
        int stone = (int)baseData.ressources[2].nombre_oer;

        carbon_Quantity.text = carbon.ToString();
        hydrogen_Quantity.text = hydro.ToString();
        energyStone_Quantity.text = stone.ToString();

        stockCurrent.text = (carbon + hydro + stone).ToString();
    }
    private async Task<BaseInfo_Model> Load_BaseInfos(int id)
    {
        string endpoint = $"/base/show/{baseId}";
        var response = await API_Client.Instance.LoadApiResponse<BaseInfo_Model>(endpoint);

        return response.output;
    }
    private async Task<BaseBuildings_Model> Load_BuildingList(int id)
    {
        string endpoint = $"/base/buildings/{baseId}";
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
        await GridManager.Instance.Load(GridLevel.Planet, GameDataStorage.Instance.CurrentBase.position.entity_id);
    }
}
