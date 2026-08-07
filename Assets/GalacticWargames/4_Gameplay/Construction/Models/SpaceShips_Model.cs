using UnityEngine;

[System.Serializable]

public class SpaceShips_Model
{
    public int base_id;
    public spaceShip_Construct[] ships;
}


[System.Serializable]
public class spaceShip_Construct
{
    public int id_vas;
    public string nom_vas;
    public string desc_vas;

    public int nb_cible_vas;
    public int initiative_vas;
    public int attaque_base_vas;
    public int attaque_batiment_vas;
    public int defense_base_vas;
    public int resistance_vas;
    public int vitesse_vas;
    public int nb_up_case_vas;
    public int esquive_vas;
    public int vitesse_construction_vas;
    public int soute_vas;

    public int level_required_vas;
    public int type_vas;

    public ShipConstruct_Cost[] couts; 
}

[System.Serializable]
public class ShipConstruct_Cost
{
    public int idmtx_vre;
    public int nombre_vre;
}
