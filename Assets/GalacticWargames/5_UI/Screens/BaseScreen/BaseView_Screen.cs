using System.Buffers.Text;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using static UnityEngine.Audio.ProcessorInstance;

public class BaseView_Screen : UIScreen
{
    public static BaseView_Screen Instance; 

    [SerializeField] private GameObject gameView;
    [SerializeField] private EntityPool entities_Pool;
    public EntityPool _Entities_Pool {  get { return entities_Pool; } }
    private int baseId;

    //Buildings
    private BaseBuildings_Model availableBuildings;
    public BaseBuildings_Model _AvailableBuildings { get { return availableBuildings; } }

    //SpaceShips
    ConstructQueue_SpaceShip[] ship_Queue;

    //Ressources
    private RessourceModule availableRessources;
    public RessourceModule _AvailableRessources { get { return availableRessources; } }

    private BaseInfo_Model baseData;
    public BaseInfo_Model _BaseData { get {return baseData; } }

    private BaseResource resourceview;
    public BaseResource _ResourceView { get { return resourceview;} }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public override void Show()
    {
        base.Show();
        gameView.SetActive(true);

        Reload_BaseView();
        GridManager.OnSwitchToWorld += OpenWorldScreen;
    }
    public override void Hide()
    {
        base.Hide();
        gameView.SetActive(false);
        GridManager.OnSwitchToWorld -= OpenWorldScreen;
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
        availableBuildings = await Load_BuildingList(baseId);
        if(availableBuildings != null)
            popupMaster.GetComponent<BaseView_Popup>()._SelectionPannel.Load_AvailableBuildings(availableBuildings.buildings);

        //Load Construction Queue
        ConstructQueue_Model queue =  await Load_ConstructQueue(baseId);
        Display_Vignettes(queue.buildings);
        ship_Queue = queue.ships;
    }

    private async Task Display_BaseInfos()
    {
        //Load Info
        baseData = await Load_BaseInfos(baseId);
        resourceview = await Load_RessourceInfos();

        int[] r = new int[3];

        r[0] = (int)baseData.ressources[0].nombre_oer;
        r[1] = (int)baseData.ressources[1].nombre_oer;
        r[2] = (int)baseData.ressources[2].nombre_oer;

        availableRessources.RefreshRessources(r);
        //Attention ! Renvoi pour l'instant que la première base de la liste (a traiter pour récupérer la base actuelle chargée)
        availableRessources.Refresh_StorageCapacity(resourceview.storage_total);
    }
    private void Display_Vignettes(ConstructQueue_Building[] buildings)
    {
        foreach(ConstructQueue_Building b in buildings)
        {
            Transform pos = FindAnyObjectByType<GridRenderer>().GetBaseTile(b.x, b.y).transform;

            BuilderQueue_Displayer.Instance.SpawnVignette(0,pos.position);
        }
    }

    private async Task<BaseResource> Load_RessourceInfos()
    {
        string endpoint = $"/resources/overview";
        var response = await API_Client.Instance.LoadApiResponse<ResourceOverview>(endpoint);

        //Choisir la bonne base dans la liste !!
        if (response.output == null) Debug.LogWarning("No Base Data Loaded");

        BaseResource r = response.output.base_storage[0];

        return r;
    }

    private async Task<BaseInfo_Model> Load_BaseInfos(int id)
    {
        string endpoint = $"/base/show/{id}";
        var response = await API_Client.Instance.LoadApiResponse<BaseInfo_Model>(endpoint);

        if (response.output==null) Debug.LogWarning("No Base Data Loaded");

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
