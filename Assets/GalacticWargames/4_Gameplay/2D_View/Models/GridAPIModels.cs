using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Modèle de l'API avec struct et liste des tuiles
/// </summary>
[System.Serializable]
public class GridAPIModels
{
    public int id;

    public string name;

    public string server_cursor;

    public List<GridTileApiDto> tiles;
}

/// <summary>
///  Modèle de l'API avec Construct Tuile
/// </summary>
[System.Serializable]
public class GridTileApiDto
{
    public int x;
    public int y;

    public string type;

    public int entity_id;

    public bool is_portal;

    public int image_id;

    public string image_name;

    public string asset_path;

    public List<EntityDto> entities;

    //public VisibilityDto visibility;
    //public FogOverlayDto fog_overlay;
}





[System.Serializable]
public class GridMapResponse
{
    public string level; // "galaxy", "solar_system", "planet"

    public List<GridTileDto> tiles;
}

[System.Serializable]
public class GridTileDto
{
    public int x;
    public int y;

    public int image_id;
    public List<EntityDto> entities;
}

/*[System.Serializable]
public class EntityDto
{
    public int id;
    public string type;
    public string image_id;
}*/