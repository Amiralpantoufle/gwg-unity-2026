using UnityEngine;

public class FleetSelection_Pannel : UIScreen
{
    [SerializeField] private GameObject fleetAsset_Prefab;
    [SerializeField] private Transform fleetAssets_Root;

    public override void Show()
    {
        base.Show();
        Debug.Log("try display pannel");
    }
    public override void Hide()
    {
        base.Hide();
    }
    public void Load_AvailableShips(spaceShip_Construct[] shipList)
    {
        foreach(var ship in shipList)
        {
            GameObject asset = Instantiate(fleetAsset_Prefab, fleetAssets_Root);

            Fleet_SpaceshipVignette vignette = asset.GetComponent<Fleet_SpaceshipVignette>();
            vignette.Load_ShipVignette(ship);
        }
    }
}
