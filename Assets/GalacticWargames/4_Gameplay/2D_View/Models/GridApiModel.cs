using System.Collections.Generic;
using UnityEngine;

//Construct Maps
[System.Serializable]
public abstract class GridApiModel
{
    public int id;
    public string name;

    public int width;
    public int height;
}

[System.Serializable]
public class GridPlanetModel : GridApiModel
{
    public string server_cursor;
    public List<GridTile> tiles;
}

[System.Serializable]
public class GridSystemModel : GridApiModel
{
    public List<GridTile> tiles;
    //public List<GridSystemTileModel> planets;
}

[System.Serializable]
public class GridGalaxyModel : GridApiModel
{
    public List<GridTile> tiles;

}

//Construct Tiles
[System.Serializable]
public class GridTile
{
    public int x;
    public int y;

    public int image_id;
    public int entity_id;
    public string type;

    //public List<EntityModel> entities;
    public List<EntityDto> entities;


    //public VisibilityDto visibility;
    //public FogOverlayDto fog_overlay;
}
[System.Serializable]
public class PlanetGridTile : GridTile
{
    public bool is_portal;

    //public List<EntityDto> entities;
}

[System.Serializable]
public class SystemGridTile : GridTile
{
    public string name;
}

[System.Serializable]
public class EntityDto
{
    public int id;
    public string type;
    public int image_id;
}