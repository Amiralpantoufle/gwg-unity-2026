using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GridLevel currentLevel;
    public static GridManager Instance;
    private MapNavigationController nav;
    private GridRenderer gridRenderer;

    private int currentGalaxy;
    private int currentSystem;
    private int currentPlanet;

    public static event Action OnSwitchToBase;
    public static event Action OnSwitchToWorld;

    private void Awake()
    {
        Instance = this;

        nav = GetComponent<MapNavigationController>();
        if (nav == null) Debug.LogError("Couldn't get MapNavigationController. Script is missing");

        gridRenderer = GetComponent<GridRenderer>();

    }
    public async Task LaunchProcess()
    {
        //Load Planete depuis id de la base
        if (GameDataStorage.Instance.CurrentBase == null)
        {
            Debug.LogError("GameDataStorage hasn't stored any base");
        }
        int planetId = GameDataStorage.Instance.CurrentBase.position.planet_id;
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

            //await GetComponent<GridRenderer>().RenderPlanet(planet);
            await gridRenderer.GenerateMap(planet);
        }
        else if (level == GridLevel.SolarSystem)
        {
            GridSystemModel system = await LoadSystemFromData(id);
            if (system == null) Debug.LogError("Failed to load system");

            gridRenderer.RenderSystem(system);
        }
        else if (level == GridLevel.Galaxy)
        {
            GridGalaxyModel galaxy = await LoadGalaxyFromData(id);
            if (galaxy == null) Debug.LogError("Failed to load system");

            gridRenderer.RenderGalaxy(galaxy);
        }

        currentLevel = level;
        OnSwitchToWorld?.Invoke();
    }
    public async void LoadBase(int baseID)
    {
        GridBaseModel baseModel = await LoadBaseFromData(baseID);
        if (baseModel == null) Debug.LogError("Failed to load planet");

        gridRenderer.RenderBase(baseModel);

        OnSwitchToBase?.Invoke();
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

    private async Task<GridBaseModel> LoadBaseFromData(int id)
    {
        var response = await LoadApiResponse<GridBaseModel>($"/base/isometric/{id}");
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

                //tView = gridRenderer.GetTile(GameDataStorage.Instance.CurrentBase.position.x, GameDataStorage.Instance.CurrentBase.position.y);
                break;

            case GridLevel.SolarSystem:
                await SwitchToGalaxy();
                break;

            case GridLevel.Galaxy:
                await SwitchToPlanet();
                break;
        }

        //Recenter on current
        if (tView == null)
            Debug.LogError("No tile referene to center map on");
        else
            nav.CenterOnTile(tView.transform);
    }
    private async Task SwitchToSystem()
    {
        int systemId = GameDataStorage.Instance.CurrentBase.position.system_id;

        /*EntityModelOuput entity = await GetEntity(planetId);
        if (entity == null) return;*/

        currentLevel = GridLevel.SolarSystem;
        await Load(currentLevel, systemId);
    }
    private async Task SwitchToGalaxy()
    {
        int galaxyId = GameDataStorage.Instance.CurrentBase.position.galaxy_id;

        /*EntityModelOuput entity = await GetEntity(systemId);
        if (entity == null) return;*/

        currentLevel = GridLevel.Galaxy;
        await Load(currentLevel, galaxyId);
    }
    private async Task SwitchToPlanet()
    {
        int planetId = GameDataStorage.Instance.CurrentBase.position.planet_id;

        currentLevel = GridLevel.Planet;
        await Load(currentLevel, planetId);
    }

    //Utility
    private void CenterOnBase()
    {
        //Center on Base
        BaseOutput currentBase = GameDataStorage.Instance.CurrentBase;
        if (currentBase == null) return;

        TileView tView = gridRenderer.GetTile(currentBase.position.x, currentBase.position.y);
        Debug.Log(currentBase.position.x +""+ currentBase.position.y);

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

}