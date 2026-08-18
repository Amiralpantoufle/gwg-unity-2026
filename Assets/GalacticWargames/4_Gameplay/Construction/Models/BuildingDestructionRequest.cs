using UnityEngine;

[System.Serializable]
public class BuildingDestructionRequest
{
    public int id_oes;
    public int id_batiment;
}

[System.Serializable]
public class BuildingDestructionResponse
{
    public bool deleted;
    public int id_oes;
    public int id_batiment;
}