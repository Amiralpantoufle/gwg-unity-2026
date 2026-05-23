using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridAPIModels
{
    public int id;

    public string name;

    public string server_cursor;

    public List<GridTileApiDto> tiles;
}

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
