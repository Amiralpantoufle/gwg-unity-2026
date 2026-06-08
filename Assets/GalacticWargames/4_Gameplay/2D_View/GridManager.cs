using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using static UnityEngine.EventSystems.EventTrigger;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    private MapNavigationController nav;

    private GridLevel currentLevel;
    private int currentLevelID;

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
    //Map Loading Process
    private async void LaunchProcess()
    {
        //Load Planete depuis id de la base
        if (GameDataStorage.Instance.CurrentBase == null)
        {
            Debug.LogError("GameDataStorage hasn't stored any base");
            //return;
        }
        int planetId = GameDataStorage.Instance.CurrentBase.PlanetId;
        await Load(GridLevel.Planet, planetId);

        //Centrer sur la base
        CenterOnBase();
    }
    
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
        var response = await LoadApiResponse<GridGalaxyModel>($"/map/system/{id}");
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

    //Inputs
    private void OnEnable()
    {
        MainView_Screen.OnMainViewLoaded += HandleMainViewLoaded;
    }
    private void OnDisable()
    {
        MainView_Screen.OnMainViewLoaded -= HandleMainViewLoaded;
    }
    private void HandleMainViewLoaded()
    {
        //Get Player and Global Data
        //BootStrap_Loader.Instance.Init_BootStrap();
    }
    public async void SwitchLevel()
    {
        GridLevel level=GridLevel.Planet;
        int id = 0;
        string json = null;

        //analyse entity actuelle pour remonter au parent lié
        if (currentLevel == GridLevel.Planet)//Switch To system
        {
            id = GameDataStorage.Instance.CurrentBase.PlanetId;

            json = await API_Client.Instance.GetAsync($"/entity/show/{id}");
            level = GridLevel.SolarSystem;
        }
        else if (currentLevel == GridLevel.Galaxy)//Switch To Galaxy
        {
           // json = await API_Client.Instance.GetAsync($"/map/system/{id}");

        }
        else if (currentLevel == GridLevel.Galaxy)//Switch To Planet
        {
            //json = await API_Client.Instance.GetAsync($"/map/galaxy/{id}");

        }
        else //Base Level
        {

        }

        //Response build
        if (string.IsNullOrEmpty(json)) return;

        Debug.Log(json);

        ApiResponse<EntityModelOuput> response = JsonConvert.DeserializeObject<ApiResponse<EntityModelOuput>>(json);
        if (response == null)
        {
            Debug.LogError("Impossible de parser EntityModelOuput");
            return;
        }
        if (response.error)
        {
            Debug.LogError($"EntityModelOuput API ERROR : {response.error} - {response.error}");
            return;
        }
        if (response.output == null)
        {
            Debug.LogError("EntityModelOuput output null");
            return;
        }

        await Load(level, response.output.id_parent_esp);
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
            Debug.Log("Center on player selected base at :" + currentBase.TileId + " Transform =" + tView.transform.gameObject);

            //Center on tile
            nav.CenterOnTile(tView.transform);
        }
    }
    //Utility
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