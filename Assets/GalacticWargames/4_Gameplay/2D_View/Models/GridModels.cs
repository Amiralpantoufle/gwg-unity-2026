using System.Collections.Generic;


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

    public int entity_id;

    public List<EntityDto> entities;
}

[System.Serializable]
public class EntityDto
{
    public int id;

    public string type;

    public string image_id;
    public string image_name;
    public string asset_path;

    public float render_scale;
}