using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    public event Action<GridLevel, GridMapResponse> OnGridLoaded;

    private void Awake()
    {
        Instance = this;
        GetComponent<GridRenderer>().Init_GridRenderer();
    }

    // =========================
    // ENTRY POINT
    // =========================

    public async Task Load(GridLevel level, int id)
    {
        //Charge les visuels si pas en cache
        //await GridVisualService.Instance.LoadVisuals();

        //Charge la map
        GridMapResponse map = await LoadMapFromData(level, id);

        if (map == null)
        {
            Debug.LogError("Failed to load map");
            return;
        }

        //Render map
        OnGridLoaded?.Invoke(level, map);
    }

    private async Task<GridMapResponse> LoadMapFromData(GridLevel level, int id)
    {
        string json = await API_Client.Instance.GetAsync($"/map/planet/{id}");

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

        Debug.Log("Finished Loading Map Data from API :" + response.output);
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
                image_id = tile.entity_id,
                entities = tile.entities
            });
        }

        Debug.Log("Finished converting map Response");
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
        BootStrap_Loader.Instance.Init_BootStrap();
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