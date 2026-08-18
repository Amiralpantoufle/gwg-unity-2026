using UnityEngine;

[System.Serializable]
public class BaseInfo_Model 
{
    public Buildings_Model[] buildings;
    public Ressources_Model[] ressources;
    public LocalEconomy_Model local_economy;
}


[System.Serializable]
public class Buildings_Model
{

}

[System.Serializable]
public class Ressources_Model
{
    public int id_oer;
    public int nombre_oer;
}

[System.Serializable]
public class LocalEconomy_Model
{
    public Storage_Model storage;
    public Production_Model production_projection;
    public Safe_Model safe;
}
[System.Serializable]
public class Storage_Model
{
    public int used;
    public int max;
    public int available;
}
[System.Serializable]
public class Production_Model
{
    public ResourceProduction_Model by_resource;
}
[System.Serializable]
public class ResourceProduction_Model
{
    public int carbon;
    public int hydrogene;
    public int energie;
}
[System.Serializable]
public class Safe_Model
{
    public int protected_volume;
}

