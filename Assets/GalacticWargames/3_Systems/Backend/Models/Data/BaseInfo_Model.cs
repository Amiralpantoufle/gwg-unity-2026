using UnityEngine;

[System.Serializable]
public class BaseInfo_Model
{
    public Buildings_Model[] buildings;
    public Ressources_Model[] ressources;
    public Production_Model[] production;
}

[System.Serializable]
public class Buildings_Model
{

}

[System.Serializable]
public class Ressources_Model
{
    public int? id_oer;
    public int? nombre_oer;
}

[System.Serializable]
public class Production_Model
{
    public int production_pep; 
}
