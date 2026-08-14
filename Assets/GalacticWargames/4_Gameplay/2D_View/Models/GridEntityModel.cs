using UnityEngine;

[System.Serializable]
public class GridEntityModel 
{
    public string type;
    public string name;
    public int v;

    public int id; // ???
    //public int entity_id; // ???
}

[System.Serializable]
public class MapEntity : GridEntityModel
{
}

[System.Serializable]
public class BaseEntity : GridEntityModel
{
    public int building_id;
    public int level;

    public string building_type;
    public string status;

    public int hp;
    public int hp_max;

}
