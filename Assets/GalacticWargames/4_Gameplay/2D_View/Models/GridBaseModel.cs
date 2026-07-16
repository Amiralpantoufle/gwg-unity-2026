using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridBaseModel
{
    public int id; // Id de la map
    public string name; // Nom si dispo

    public int width;
    public int height;

    public List<GridBaseTile> tiles;
}

//Construct Tiles
[System.Serializable]
public class GridBaseTile
{
    public int x;
    public int y;
    public bool constructible;
    public int v;

    public List<BaseEntity> entities;

    //public string name;
    //public VisibilityDto visibility;
    //public FogOverlayDto fog_overlay;
}

[System.Serializable]
public class BaseEntity
{
    public string type;
    public int id;
    public int building_id;
    public int level;

    public string building_type;
    public string name;
    public string status;

    public int hp;
    public int hp_max;

    public int v;
}