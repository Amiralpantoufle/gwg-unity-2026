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

    BaseInfo_Model baseData;

    public override void Show()
    {
        base.Show();
        gameView.SetActive(true);

        Reload_BaseView();
    }
    public override void Hide()
    {
        base.Hide();
        gameView.SetActive(false);
    }

    public async void Reload_BaseView()
    {
        baseId = GameDataStorage.Instance.GetLastBaseId();
        if (baseId == 0) Debug.LogError("NO BASE ID LOADED");


        //Load Ressource Component
        availableRessources = GetComponent<RessourceModule>();
        GameDataStorage.Instance._Current_RessourceModule = availableRessources;
        await Display_BaseInfos();

        //Load Building List
        BaseBuildings_Model buildingModel = await Load_BuildingList(baseId);
        popupMaster.GetComponent<BaseView_Popup>()._SelectionPannel.Load_AvailableBuildings(buildingModel.buildings);

        //Load Construction Queue
        ConstructQueue_Model queue =  await Load_ConstructQueue(baseId);
        Display_Vignettes(queue.buildings, queue.ships);

        GridManager.OnSwitchToWorld += OpenWorldScreen;
    }

    private async Task Display_BaseInfos()
    {
        //Load Info
        baseData = await Load_BaseInfos(baseId);

        int[] r = new int[3];

        r[0] = (int)baseData.ressources[0].nombre_oer;
        r[1] = (int)baseData.ressources[1].nombre_oer;
        r[2] = (int)baseData.ressources[2].nombre_oer;

        availableRessources.RefreshRessources(r);
        //availableRessources.Refresh_StorageCapacity(userData.infos_user.BASE_STOCKAGE_DEFAULT);
    }
    private void Display_Vignettes(ConstructQueue_Building[] buildings, ConstructQueue_Building[] ships)
    {
        foreach(ConstructQueue_Building b in buildings)
        {
            Transform pos = FindAnyObjectByType<GridRenderer>().GetBaseTile(b.x, b.y).transform;

            BuilderQueue_Displayer.Instance.SpawnVignette(0,pos.position);
        }
    }

    private async Task<RessourceOverview> Load_RessourceInfos()
    {
        string endpoint = $"/resources/overview";
        var response = await API_Client.Instance.LoadApiResponse<RessourceOverview>(endpoint);

        return response.output;
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

    private async Task<ConstructQueue_Model> Load_ConstructQueue(int id)
    {
        string endpoint = $"/construction/queue/{id}";
        var response = await API_Client.Instance.LoadApiResponse<ConstructQueue_Model>(endpoint);

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
