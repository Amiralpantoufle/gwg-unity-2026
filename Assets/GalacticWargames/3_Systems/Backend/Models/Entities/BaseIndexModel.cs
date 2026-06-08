using System;
using System.Collections.Generic;
using UnityEngine;

/*[Serializable]
public class BaseIndexOuput
{
    public List<BaseIndexData> baseIndexDatas;
}*/

[Serializable]
public class BaseIndexOutput
{
    public int id_oes;
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
}
