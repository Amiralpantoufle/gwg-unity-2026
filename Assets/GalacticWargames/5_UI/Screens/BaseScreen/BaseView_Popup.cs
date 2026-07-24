using UnityEngine;

public class BaseView_Popup : UIScreen
{
    [SerializeField] private GameObject selectionPannel;
    [SerializeField] private GameObject upgradePannel;

    [SerializeField] private GameObject[] buildingsPannels;
    private buildingList selectedBuilding;

    [SerializeField] private GameObject infoPannel;

    //Selection
    Base_TileView loadedTile;

    public void Load_TileData(Base_TileView tile)
    {
        selectionPannel.SetActive(true);
        loadedTile = tile;
    }
    public void Close_SelecPannel()
    {
        selectionPannel.SetActive(false);
        loadedTile = null;
    }
    private void Open_BuildingPannel(buildingList selection)
    {
        foreach (GameObject pannel in buildingsPannels)
            pannel.SetActive(false);

        switch(selection)
        {
            case buildingList.ChantierSpatial:
                buildingsPannels[0].SetActive(true);
                break;
            case buildingList.EspaceStockage:
                buildingsPannels[1].SetActive(true);
                break;
            case buildingList.MineCarbon:
                buildingsPannels[2].SetActive(true);
                break;
            case buildingList.MineHydrogen:
                buildingsPannels[3].SetActive(true);
                break;
            case buildingList.MinePierre:
                buildingsPannels[4].SetActive(true);
                break;
        }
    }

}
/// <summary>
/// 0=chantierSpatial 1=EspaceStockage 2=MineCarbon 3=Hydrogen 4=PierreEnerg 
/// </summary>
public enum buildingList
{
    ChantierSpatial,
    EspaceStockage,
    MineCarbon,
    MineHydrogen,
    MinePierre
}
