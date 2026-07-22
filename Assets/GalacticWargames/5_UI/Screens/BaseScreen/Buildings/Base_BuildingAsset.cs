using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Base_BuildingAsset : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI assetName;
    [SerializeField] private TextMeshProUGUI buildTime;

    [SerializeField] private Image[] ressourceIcons;
    [SerializeField] private Image buildingIcon;

    [SerializeField] private Image selectionBorder;
    public Image _SelectionBorder { get { return selectionBorder; } }

    public void Load_buildingInfo()
    {
        //assetName.text = 
        //buildTime.text = 
        
    }
}
