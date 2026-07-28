using TMPro;
using UnityEngine;

public class Base_BuildingAssetInfo : MonoBehaviour
{
    //Components
    [SerializeField] private TextMeshProUGUI GUI_Name;
    [SerializeField] private TextMeshProUGUI GUI_description;
    [SerializeField] private TextMeshProUGUI[] costs;

    //Selection
    //Selected BuildingAsset_Model

    public void Load_Info(string bName, string desc, int[] cost)
    {
        GUI_Name.text = bName;
        GUI_description.text = desc;

        for (int i = 0; i < costs.Length; i++)
        {
            costs[i].text = cost[i].ToString();
        }
    }
}
