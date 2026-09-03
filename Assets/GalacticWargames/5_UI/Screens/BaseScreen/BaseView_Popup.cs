using UnityEngine;
using UnityEngine.UI;
using System;


public class BaseView_Popup : UIScreen
{
    //Selection
    [SerializeField] private TileSelection_Pannel selectionPannel;
    public TileSelection_Pannel _SelectionPannel { get { return selectionPannel; } }
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

        if (selection != buildingList.EmptySlot)
        {
            BaseEntity selectedEntity = tile._Tile.entities[0];

            int idToLoad = selectedEntity.id;
            if (idToLoad != 0)
            {
                BaseView_BuildingScreen building = buildingsPannels[(int)selection].GetComponent<BaseView_BuildingScreen>();
                building._Entity_id = idToLoad;
                building._Building_id = selectedEntity.building_id;
                building._Instance_id = selectedEntity.id;

                //Set Name and Levels on template Building
                string result = StringUtils.AddSpaces(selection.ToString());
                building.Load_BaseInfos(result, selectedEntity.level);
            }
            else
                Debug.LogError("no ID set on selected entity");
        }

        if (selection != 0)
            buildingsPannels[(int)selection].SetActive(true);
        else
            selectionPannel.Load_TileData(tile);
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
        {
            if (obj != null)
                obj.SetActive(false);
        }

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
    MinePierre,
    Undefined
}
