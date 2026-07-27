using UnityEngine;

[System.Serializable]
public class BaseInfo_Model
{
    public Buildings_Model[] buildings;
    public Ressources_Model[] ressources;
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
