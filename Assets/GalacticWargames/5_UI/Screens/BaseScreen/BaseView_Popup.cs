using UnityEngine;
using UnityEngine.UI;
using System;


public class BaseView_Popup : UIScreen
{
    //Selection
    [SerializeField] private TileSelection_Pannel selectionPannel;
    public TileSelection_Pannel _SelectionPannel { get { return selectionPannel; }}
    private buildingList lastSelectedBuilding;
    private Base_TileView loadedTile;
    public Base_BuildingLevelUp upgradePannel;
    [SerializeField] private GameObject[] buildingsPannels;
    [SerializeField] private GameObject infoPannel;


    //Actions
    public event Action OnClosePannel;

    public void Open_BuildingPannel(buildingList selection, Base_TileView tile) 
    {
        lastSelectedBuilding = selection;
        loadedTile = tile;

        if(selection!= buildingList.EmptySlot)
        {
            int idToLoad = tile._Tile.entities[0].id;

            if (idToLoad != 0)
            {
                buildingsPannels[(int)selection].GetComponent<BaseView_BuildingScreen>()._Building_id = idToLoad;
            }
            else
                Debug.LogError("no ID set on selected entity");
        }

        switch (selection)
        {
            case buildingList.EmptySlot:
                selectionPannel.Load_TileData(tile);
                break;
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
    public void UpgradeBuilding(buildingList building, int id)
    {
        Debug.Log("Open Upgrade Building");
        Vector2Int pos = new Vector2Int(loadedTile._Tile.x, loadedTile._Tile.y);
        upgradePannel.Load_UpgradePannel(popupMaster.GetComponent<BaseView_Screen>()._AvailableBuildings, building, id, pos);
    }

    //Utility
    public void Close_AllPannels()
    {
        foreach (GameObject obj in buildingsPannels)
            obj.SetActive(false);

        upgradePannel.gameObject.SetActive(false);
        selectionPannel.gameObject.SetActive(false);


        popupMaster.GetComponent<BaseView_Screen>().Reload_BaseView();
        OnClosePannel?.Invoke();
    }
}
/// <summary>
/// 0=chantierSpatial 1=EspaceStockage 2=MineCarbon 3=Hydrogen 4=PierreEnerg 
/// </summary>
public enum buildingList 
{
    EmptySlot,
    ChantierSpatial,
    EspaceStockage,
    MineCarbon,
    MineHydrogen,
    MinePierre
}
