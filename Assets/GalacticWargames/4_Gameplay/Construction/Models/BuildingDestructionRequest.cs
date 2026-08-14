using UnityEngine;

[System.Serializable]
public class BuildingDestructionRequest : MonoBehaviour
{
    public int id_oes;
    public int id_batiment;
}

[System.Serializable]
public class BuildingDestructionResponse : MonoBehaviour
{
    public bool deleted;
    public int id_oes;
    public int id_batiment;
}