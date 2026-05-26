using System;
using UnityEngine;


[Serializable]
public class PlanetMapResponse
{
    public bool error;
    public PlanetMapOutput output;
}

[Serializable]
public class PlanetMapOutput
{
    public int id;
    public string name;
    public string server_cursor;

    public TileData[] tiles;
}

[Serializable]
public class TileData
{
    public int x;
    public int y;

    public string type;

    public int entity_id;

    public bool is_portal;

    public int image_id;
    public string image_name;
    public string asset_path;

    public EntityData[] entities;

/*    public VisibilityData visibility;
    public FogOverlayData fog_overlay;
    public ResourceData[] resources;
    public PveEncounterData[] pve_encounters;*/
}

[Serializable]
public class EntityData
{
    public string type;
    public string name;

    public int image_id;
    public string image_name;
    public string asset_path;
}