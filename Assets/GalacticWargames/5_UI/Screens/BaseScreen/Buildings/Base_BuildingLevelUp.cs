using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;


public class Base_BuildingLevelUp : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txt_Header;
    [SerializeField] private Image[] levelBuilding_Icons;

    public void Load_UpgradePannel(buildingList building)
    {
        gameObject.SetActive(true);

        txt_Header.text = building.ToString();
    }
}
