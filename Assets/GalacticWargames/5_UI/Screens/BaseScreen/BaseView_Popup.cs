using Newtonsoft.Json;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseView_Popup : UIScreen
{
    [SerializeField] private TileSelection_Pannel selectionPannel;
    public TileSelection_Pannel _SelectionPannel { get { return selectionPannel; }}

    [SerializeField] private GameObject upgradePannel;

    [SerializeField] private GameObject[] buildingsPannels;
    private buildingList selectedBuilding;
     
    [SerializeField] private GameObject infoPannel;

    private BaseView_Screen _baseScreen;
    public BaseView_Screen BaseScreen { set { _baseScreen = value; } }


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
    EmptySlot,
    ChantierSpatial,
    EspaceStockage,
    MineCarbon,
    MineHydrogen,
    MinePierre
}
