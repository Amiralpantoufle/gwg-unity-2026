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
    [SerializeField] private Image lockedIcon;
    [SerializeField] private CanvasGroup[] greyedOut;
    public Image _SelectionBorder { get { return selectionBorder; } }

    public void Load_buildingInfo(building_Construct construct)
    {
        assetName.text = construct.nom_bat;
        buildTime.text = construct.vitesse_construction_bat.ToString();
        
    }

    private void HideConstruct()
    {
        foreach (CanvasGroup group in greyedOut)
        {
            group.alpha = 0.33f;
        }
        lockedIcon.enabled = true;
    }
}
