using UnityEngine;

[System.Serializable]
public class BaseBuildings_Model 
{
    public int base_id;
    public building_Construct[] buildings;
}

[System.Serializable]
public class building_Construct
{
    public int id_bat;
    public string nom_bat;
    public string desc_bat;
    public int idiet_bat;//Visual id

    public int vitesse_construction_bat;
    public int stockage_bat;

    public int hp_bat;
    public string production_type_bat;
    public int production_rate_bat;

    public int level_required_bat;

    public construct_Cost[] couts;
}
