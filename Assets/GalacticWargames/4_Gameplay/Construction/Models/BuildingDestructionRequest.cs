using UnityEngine;

[System.Serializable]
public class BuildingDestructionRequest
{
    public int id_oes;
    public int id_batiment;
    public string operation_key;
}

[System.Serializable]
public class BuildingDestructionResponse
{
    public bool deleted;
    public int id_oes;
    public int id_batiment;
}