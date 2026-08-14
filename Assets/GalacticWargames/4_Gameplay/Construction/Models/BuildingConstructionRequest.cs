[System.Serializable]
public class BuildingConstructionRequest
{
    public int id_oes;
    public int id_bat;
    public int x;
    public int y;

    public string operation_key;

}

[System.Serializable]
public class SpaceShipConstructionRequest
{
    public int id_oes;
    public int id_vas;
    public int nombre;

    public string operation_key;
}