using UnityEngine;
using UnityEngine.UI;
using System;


public class BaseView_Popup : UIScreen
{
    [SerializeField] private TileSelection_Pannel selectionPannel;
    public TileSelection_Pannel _SelectionPannel { get { return selectionPannel; }}


    public Base_BuildingLevelUp upgradePannel;
    [SerializeField] private GameObject[] buildingsPannels;
    private buildingList lastSelectedBuilding;
     
    [SerializeField] private GameObject infoPannel;

    //Actions
    public event Action OnClosePannel;

    public void Open_BuildingPannel(buildingList selection, Base_TileView tile)
    {
        lastSelectedBuilding = selection;

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
    public void UpgradeBuilding(buildingList building)
    {
        Debug.Log("Open Upgrade Building");
        upgradePannel.Load_UpgradePannel(building);
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
