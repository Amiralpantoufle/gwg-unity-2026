using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TileSelection_Pannel : MonoBehaviour
{ 
    [SerializeField] private TextMeshProUGUI gui_slotName;

    [SerializeField] private Transform buildingAsset_Root;
    [SerializeField] private GameObject buildingAsset_Prefab;

    [SerializeField] private Base_BuildingAssetInfo contextPannel;

    private Base_TileView loadedTile;
    public Base_TileView _LoadedTile { get { return loadedTile; } }


    //Tile Selection
    public void Load_TileData(Base_TileView tile)
    {
        loadedTile = tile;
        gameObject.SetActive(true);

        string name = "Empty Slot";
        gui_slotName.text = name;
    }
    private void Close_SelecPannel()
    {
        gameObject.SetActive(false);
        loadedTile = null;

        contextPannel.gameObject.SetActive(false);
    }

    //Utility
    public void Load_AvailableBuildings(building_Construct[] list)
    {
        Clear_AvailableBuildings();

        if (list.Length <= 0) Debug.Log("NO BUILDING AVAILABLE TO BUILD");

        foreach(building_Construct building in list)
        {
            Base_BuildingAsset asset = Instantiate(buildingAsset_Prefab, buildingAsset_Root).GetComponent<Base_BuildingAsset>();

            //Init Values
            asset.Load_buildingInfo(building, contextPannel);
        }
    }
    private void Clear_AvailableBuildings()
    {
        foreach (Transform child in buildingAsset_Root)
            Destroy(child.gameObject);
    }
}
