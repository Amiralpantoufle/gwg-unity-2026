using UnityEngine;

[System.Serializable]
public class ResourceData 
{
    public int idoes_oer;
    public int nombre_oer;
}

[System.Serializable]
public class ResourceOverview
{
    public BaseResource[] base_storage;
}
[System.Serializable]
public class BaseResource
{
    public int base_id;
    public int storage_total;
    public int storage_used;

    public bool production_blocked;
}