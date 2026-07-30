using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TileSelection_Pannel : MonoBehaviour
{ 
    [SerializeField] private TextMeshProUGUI gui_slotName;

    private List<Base_BuildingAsset> buildList = new List<Base_BuildingAsset>();
    [SerializeField] private Transform buildingAsset_Root;
    [SerializeField] private GameObject buildingAsset_Prefab;

    [SerializeField] private Base_BuildingAssetInfo contextPannel;

    //Selection
    [SerializeField] private SpriteRenderer selector;
    [SerializeField] private Sprite[] selectorVariants;
    private Base_TileView loadedTile;
    public Base_TileView _LoadedTile { get { return loadedTile; } }

    public event Action OnClosePannel;

    //Tile Selection
    public void Load_TileData(Base_TileView tile)
    {
        gameObject.SetActive(true);

        loadedTile = tile;
        string name = "Empty Slot";
        gui_slotName.text = name;

        GetTileStatus();
    }
    public void Close_SelecPannel()
    {
        gameObject.SetActive(false);
        loadedTile = null;
        selector.enabled = false;

        contextPannel.gameObject.SetActive(false);

        OnClosePannel?.Invoke();
    }
    private void HighlightTile(int typeOf)
    {
        selector.sprite = selectorVariants[typeOf];
        selector.transform.position = loadedTile.transform.position;

        selector.enabled = true;
    }

    //Utility
     private buildingList GetTileStatus()
     {
         buildingList typeOf = buildingList.EmptySlot;


        if (loadedTile._Tile.constructible)
            HighlightTile(0);
        else
            HighlightTile(1);

        return typeOf;
    }

    public void Load_AvailableBuildings(building_Construct[] list)
    {
        buildList.Clear();

        if (list.Length <= 0) Debug.Log("NO BUILDING AVAILABLE TO BUILD");

        foreach(building_Construct building in list)
        {
            Base_BuildingAsset asset = Instantiate(buildingAsset_Prefab, buildingAsset_Root).GetComponent<Base_BuildingAsset>();
            buildList.Add(asset);

            //Init Values
            asset.Load_buildingInfo(building, contextPannel);
        }
    }
}
