using UnityEngine;

public class FleetSelection_Pannel : UIScreen
{
    [SerializeField] private GameObject fleetAsset_Prefab;
    [SerializeField] private Transform fleetAssets_Root;

    public void Load_AvailableFleet(spaceShip_Construct[] shipList)
    {
        foreach(var ship in shipList)
        {
            GameObject asset = Instantiate(fleetAsset_Prefab, fleetAssets_Root);
        }
    }
}
