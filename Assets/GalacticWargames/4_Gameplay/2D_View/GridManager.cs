using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using static UnityEditor.Rendering.CameraUI;
using static UnityEngine.EventSystems.EventTrigger;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    private MapNavigationController nav;

    [SerializeField] private GridLevel currentLevel;
    [SerializeField] private MainView_TileAccess_Popup tileAccess_Popup;
    private bool tileSelected;
    public bool _TileSelected { get { return tileSelected; } }

    private void Awake()
    {
        Instance = this;

        nav = GetComponent<MapNavigationController>();
        if (nav == null) Debug.LogError("Couldn't get MapNavigationController. Script is missing");

    }
    private void Start()
    {
        LaunchProcess();
    }
    private async void LaunchProcess()
    {
        //Load Planete depuis id de la base
        if (GameDataStorage.Instance.CurrentBase == null)
        {
            Debug.LogError("GameDataStorage hasn't stored any base");
        }
        int planetId = GameDataStorage.Instance.CurrentBase.PlanetId;
        await Load(GridLevel.Planet, planetId);

        //Centrer sur la base
        CenterOnBase();
    }
    
    //Map Loading Process
    /// <summary>
    /// Load map depuis le niveau selectionné et l'ID de la map
    /// </summary>
    /// <param name="level"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task Load(GridLevel level, int id)
    {

        if (level == GridLevel.Planet)
        {
            GridPlanetModel planet = await LoadPlanetFromData(id);
            if(planet==null) Debug.LogError("Failed to load planet");

            GetComponent<GridRenderer>().RenderPlanet(planet);
        }
        else if (level == GridLevel.SolarSystem)
        {
            GridSystemModel system = await LoadSystemFromData(id);
            if (system == null) Debug.LogError("Failed to load system");

            GetComponent<GridRenderer>().RenderSystem(system);
        }
        else if (level == GridLevel.Galaxy)
        {
            GridGalaxyModel galaxy = await LoadGalaxyFromData(id);
            if (galaxy == null) Debug.LogError("Failed to load system");

            GetComponent<GridRenderer>().RenderGalaxy(galaxy);
        }

        currentLevel = level;
    }
    private async Task<GridPlanetModel> LoadPlanetFromData(int id)
    {
        var response = await LoadApiResponse<GridPlanetModel>($"/map/planet/{id}");
        return response.output;
    }
    private async Task<GridSystemModel> LoadSystemFromData(int id)
    {
        var response = await LoadApiResponse<GridSystemModel>($"/map/system/{id}");
        return response.output;
    }
    private async Task<GridGalaxyModel> LoadGalaxyFromData(int id)
    {
        var response = await LoadApiResponse<GridGalaxyModel>($"/map/galaxy/{id}");
        return response.output;
    }

    private async Task<ApiResponse<T>> LoadApiResponse<T>(string endpoint)
    {
        string json = await API_Client.Instance.GetAsync(endpoint);

        if (string.IsNullOrEmpty(json))
            return null;

        var response = JsonConvert.DeserializeObject<ApiResponse<T>>(json);
        if (response == null)
        {
            Debug.LogError($"Impossible de parser {typeof(T).Name}");
            return null;
        }
        if (response.error)
        {
            Debug.LogError($"API Error : {response.error}");
            return null;
        }
        if (response.output == null)
        {
            Debug.LogError("Output null");
            return null;
        }

        return response;
    }
    private GridMapResponse ConvertModelToMap(GridLevel level, GridApiModel data)
    {
        GridMapResponse map = new GridMapResponse();

        switch (data)
        {
            case GridPlanetModel planet:

                break;

            case GridSystemModel system:

                break;

            case GridGalaxyModel galaxy:

                break;
        }

        return map;
    }

    //Switch
    /// <summary>
    /// Appelé depuis MainScreen-> Button | Change de niveau par incrémentation
    /// </summary>
    public async void SwitchLevel()
    {
        TileView tView = null;

        switch (currentLevel)
        {
            case GridLevel.Planet:
                await SwitchToSystem();

                tView = GetComponent<GridRenderer>().GetTile(GameDataStorage.Instance.CurrentBase.SystemX, GameDataStorage.Instance.CurrentBase.SystemY);
                break;

            case GridLevel.SolarSystem:
                await SwitchToGalaxy();
                break;

            case GridLevel.Galaxy:
                await SwitchToPlanet();

                tView = GetComponent<GridRenderer>().GetTile(GameDataStorage.Instance.CurrentBase.PlanetX, GameDataStorage.Instance.CurrentBase.PlanetY);
                break;
        }

        //Recenter on current
        if (tView == null)
            Debug.LogError("No matching tile");
        else
        {
            //Center on tile
            nav.CenterOnTile(tView.transform);
        }
    }
    private async Task SwitchToSystem()
    {
        int planetId = GameDataStorage.Instance.CurrentBase.PlanetId;

        EntityModelOuput entity = await GetEntity(planetId);
        if (entity == null) return;

        currentLevel = GridLevel.SolarSystem;

        await Load(currentLevel, entity.id_parent_esp);
    }
    private async Task SwitchToGalaxy()
    {
        int systemId = GameDataStorage.Instance.CurrentBase.SystemId;

        EntityModelOuput entity = await GetEntity(systemId);
        if (entity == null) return;

        currentLevel = GridLevel.Galaxy;

        await Load(currentLevel, entity.idglx_esp);
    }
    private async Task SwitchToPlanet()
    {
        currentLevel = GridLevel.Planet;
        int planetId = GameDataStorage.Instance.CurrentBase.PlanetId;
        await Load(currentLevel, planetId);
    }

    //Utility
    public void SelectTile(TileView tile)
    {
        EventBus.Publish(new ShowPopupEvent
        {
            popup = tileAccess_Popup
        });
        tileAccess_Popup.LoadTileViewData(tile);
        tileSelected = true;
    }
    public void CancelSelect()
    {
        EventBus.Publish(new HidePopupEvent
        {
            hidePopup = tileAccess_Popup
        });
        tileAccess_Popup.HideCurrentTile();
        tileSelected = false;
    }
    private void CenterOnBase()
    {
        //Center on Base
        PlayerBaseData currentBase = GameDataStorage.Instance.CurrentBase;
        if (currentBase == null) return;

        TileView tView = GetComponent<GridRenderer>().GetTile(currentBase.PlanetX, currentBase.PlanetY);

        if (tView == null)
            Debug.LogError("No matching tile");
        else
        {
            //Center on tile
            nav.CenterOnTile(tView.transform);
        }
    }
    private async Task<EntityModelOuput> GetEntity(int entityId)
    {
        string json = await API_Client.Instance.GetAsync($"/entity/show/{entityId}");

        if (string.IsNullOrEmpty(json))
            return null;

        Debug.Log(json);

        try
        {

            ApiResponse<EntityModelOuput> response = JsonConvert.DeserializeObject<ApiResponse<EntityModelOuput>>(json);
            if (response == null)
            {
                Debug.LogError("Impossible de parser EntityModelOuput");
                return null;
            }

            if (response.error)
            {
                Debug.LogError($"EntityModelOuput API ERROR : {response.error}");
                return null;
            }

            if (response.output == null)
            {
                Debug.LogError("EntityModelOuput output null");
                return null;
            }
            return response.output;
        }
        catch (Exception e)
        {
            return null;
        }
    }
    private GridMapResponse FakeMap(GridLevel level)
    {
        var map = new GridMapResponse();
        map.level = level.ToString().ToLower();
        map.tiles = new System.Collections.Generic.List<GridTileDto>();

        int size = level switch
        {
            GridLevel.Galaxy => 4,
            GridLevel.SolarSystem => 6,
            GridLevel.Planet => 8,
            _ => 5
        };

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                map.tiles.Add(new GridTileDto
                {
                    x = x,
                    y = y,
                    image_id = x * 100 + y,
                    entities = new System.Collections.Generic.List<EntityDto>()
                });
            }
        }

        return map;
    }
}