using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    public event Action<GridLevel, GridMapResponse> OnGridLoaded;
    private MapNavigationController nav;

    private GridLevel currentLevel;
    private int currentLevelID;

    private void Awake()
    {
        Instance = this;
        GetComponent<GridRenderer>().Init_GridRenderer();

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
        int planetId = GameDataStorage.Instance.CurrentBase.PlanetId;
        await Load(GridLevel.Planet, planetId);

        //Centrer sur la base
        CenterOnBase();
    }
    /// <summary>
    /// Load la map a partir du niveau et ID selectionnes
    /// </summary>
    /// <param name="level"></param>
    /// <param name="id"></param>
    public async Task Load(GridLevel level, int id)
    {
        currentLevel = level;
        GridMapResponse map = await LoadmapFromData(level, id);

        if (map == null)
        {
            Debug.LogError("Failed to load map");
            return;
        }

        //Render map
        OnGridLoaded?.Invoke(level, map);
    }
    private async Task<GridMapResponse> LoadmapFromData(GridLevel level, int id)
    {
        string json = null;

        //Charge la map
        switch (level)
        {
            case GridLevel.Planet:
                json = await API_Client.Instance.GetAsync($"/map/planet/{id}");
                break;
            case GridLevel.SolarSystem:
                json = await API_Client.Instance.GetAsync($"/map/system/{id}");
                break;
            case GridLevel.Galaxy:
                json = await API_Client.Instance.GetAsync($"/map/galaxy/{id}");
                break;
        }

        if (string.IsNullOrEmpty(json))
            return null;

        //API call & Security
        ApiResponse<GridAPIModels> response = JsonConvert.DeserializeObject<ApiResponse<GridAPIModels>>(json);
        if (response == null)
        {
            Debug.LogError("Impossible de parser PlanetMap");
            return null;
        }
        if (response.error)
        {
            Debug.LogError($"Planet API ERROR : {response.error} - {response.error}");
            return null;
        }
        if (response.output == null)
        {
            Debug.LogError("Planet output null");
            return null;
        }

        return ConvertToGridMapResponse(level, response.output);
    }
    private GridMapResponse ConvertToGridMapResponse(GridLevel level, GridAPIModels apiData)
    {
        GridMapResponse map = new GridMapResponse();


        map.level = level.ToString().ToLower();

        map.tiles = new List<GridTileDto>();

        foreach (var tile in apiData.tiles)
        {
            map.tiles.Add(new GridTileDto
            {
                x = tile.x,
                y = tile.y,
                image_id = tile.image_id,
                entities = tile.entities,
            });
        }

        return map;
    }

    //UI Inputs
    public async void SwitchLevel()
    {
        GridLevel level;
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

        /*ApiResponse<GridAPIModels> response = JsonConvert.DeserializeObject<ApiResponse<GridAPIModels>>(json);
        if (response == null)
        {
            Debug.LogError("Impossible de parser PlanetMap");
            return;
        }
        if (response.error)
        {
            Debug.LogError($"Planet API ERROR : {response.error} - {response.error}");
            return;
        }
        if (response.output == null)
        {
            Debug.LogError("Planet output null");
            return;
        }

        GetLevelParentInfos(response);

        await Load(level, id);*/
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

    //Utility
    private void GetLevelParentInfos(ApiResponse<GridAPIModels> levelResponse)
    {
        Debug.Log(levelResponse.ToString());
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