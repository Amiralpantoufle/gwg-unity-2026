using System.Collections.Generic;
using UnityEngine;

//Construct Maps
[System.Serializable]
public abstract class GridApiModel
{
    public int id; // Id de la map
    public string name; // Nom si dispo

    public int width;
    public int height;

    public List<GridTile> tiles;
}

[System.Serializable] 
public class GridPlanetModel : GridApiModel
{
    public string server_cursor;
}

[System.Serializable]
public class GridSystemModel : GridApiModel
{
}

[System.Serializable]
public class GridGalaxyModel : GridApiModel
{
}


//Construct Tiles
[System.Serializable]
public class GridTile
{
    public int x;
    public int y;
    public string type;
    public int v;
    public int entity_id;

    public List<EntityDto> entities;

    //public string name;
    //public VisibilityDto visibility;
    //public FogOverlayDto fog_overlay;
}

[System.Serializable]
public class PlanetGridTile : GridTile
{
}

[System.Serializable]
public class SystemGridTile : GridTile
{
}

[System.Serializable]
public class GalaxyGridTile : GridTile
{
    //public List<EntityDto> systems;
    public int planet_count;
}
//Entities
[System.Serializable]
public class EntityDto
{
    public int entity_id;
    public string type;
    public string name;
    public int v;
}