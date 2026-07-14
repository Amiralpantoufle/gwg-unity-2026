using System;
using System.Collections.Generic;
using UnityEngine;

/*[Serializable]
public class BaseIndexOutput
{
    //public int id_oes;
    public int idesp_oes;
    public int id_parent_esp;
    public int planet_id;
    public int niveau_esp;

    public string nom_oes;
    public string name_esp;

    public int idcat_esp;

    public int x_p_esp;
    public int y_p_esp;

    public int x_s_esp;
    public int y_s_esp;

    public int idglx_esp;
}

[Serializable]
public class PlayerBaseData
{
    public int BaseId;
    public int TileId;
    public int LocationEntityId;
    public int PlanetId;
    public int SystemId;

    public int PlanetX;
    public int PlanetY;

    public int SystemX;
    public int SystemY;
}*/

[Serializable]
public class BaseIndexOutput
{
    public List<BaseOutput> bases;
}

[Serializable]
public class BaseOutput
{
    //public int tile_id;
    public int base_id;
    public string name;
    public PositionOutput position;
}

[Serializable]
public class PositionOutput
{
    public int entity_id;
    public string entity_name;
    public string category;
    public int planet_id;
    public int system_id;
    public int galaxy_id;

    public int x;
    public int y;
}