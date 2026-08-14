using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;


public class Base_BuildingLevelUp : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txt_Header;
    [SerializeField] private Base_BuildingLevelAsset[] levelAssets;

    private int building_ID;
    private Vector2Int newTilePos;
    public void Load_UpgradePannel(BaseBuildings_Model availableBuildings, buildingList building, int id, Vector2Int tilePos)
    {
        txt_Header.text = building.ToString();

        List<building_Construct> bList = GetBuildingsData(availableBuildings, building);
        if(bList != null && bList.Count > 0)
        {
            for(int i =0; i<bList.Count; i++)
                levelAssets[i].Load_LevelAsset(bList[i], newTilePos, building_ID);
        }

        gameObject.SetActive(true);
    }

    private List<building_Construct> GetBuildingsData(BaseBuildings_Model availableBuildings, buildingList building)
    {
        List<building_Construct> bList = new List<building_Construct>();
        int targetID_FromList=0;

        switch (building)
        {
            //Range 1-3
            case buildingList.ChantierSpatial:
                targetID_FromList = 1;
                break;

            //Range 4-6
            case buildingList.EspaceStockage:
                targetID_FromList = 4;
                break;

            //Range 25-27
            case buildingList.MineCarbon:
                targetID_FromList = 25;
                break;

            //Range 28-30
            case buildingList.MineHydrogen:
                targetID_FromList = 28;
                break;

            //Range 31-33
            case buildingList.MinePierre:
                targetID_FromList = 31;
                break;
        }

        if (targetID_FromList == 0)
        {
            Debug.LogWarning("No Id Attached to building screen");
            return null;
        }

        for (int i = targetID_FromList; i < targetID_FromList + 2; i++)
            if (targetID_FromList == availableBuildings.buildings[i].id_bat)
                bList.Add(availableBuildings.buildings[i]);
            else
                Debug.Log("Couldn't find tier 3 building");

        return bList;
    }
}
