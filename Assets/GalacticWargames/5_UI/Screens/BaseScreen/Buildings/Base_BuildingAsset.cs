using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Base_BuildingAsset : MonoBehaviour
{
    //Main Info
    [SerializeField] private TextMeshProUGUI assetName;
    [SerializeField] private TextMeshProUGUI buildTime;

    [SerializeField] private Image[] ressourceIcons;
    [SerializeField] private Image buildingIcon;

    //Context Info
    private string desc;
    private int[] cost = new int[3];

    //Extra components
    [SerializeField] private Image lockedIcon;
    [SerializeField] private CanvasGroup[] greyedOut;

    [SerializeField] private Image selectionBorder;
    public Image _SelectionBorder { get { return selectionBorder; } }

    private Base_BuildingAssetInfo contextPannel;

    public void Load_buildingInfo(building_Construct construct, Base_BuildingAssetInfo context)
    {
        //Load Info
        assetName.text = construct.nom_bat.Substring(0, construct.nom_bat.Length - 2);
        desc = construct.desc_bat;

        //Define Day Build Time
        buildTime.text = GetBuildTime(construct.vitesse_construction_bat);

        //Swap Icon
        VisualDefinition v = GridVisualService.Instance.GetVisual(construct.idiet_bat);
        buildingIcon.sprite = v.imageSprite;

        //Display Ressources
        for(int i = 0; i< construct.couts.Length; i++)
        {
            cost[i] = construct.couts[i].nombre_bre;
            NeedsRessources(construct.couts[i].nombre_bre, i);
        }

        //Callback
        contextPannel = context;
        GetComponent<Button>().onClick.AddListener(Display_ContextPannel);
    }
    public void Display_ContextPannel()
    {
        contextPannel.Load_Info(assetName.text, desc, cost);
        contextPannel.gameObject.SetActive(true);
    }

    private void NeedsRessources(int quantity, int ressourceIndex)
    {
        if (quantity > 0)
            ressourceIcons[ressourceIndex].color = Color.white;
        else
            ressourceIcons[ressourceIndex].color = new Color(1, 1, 1, 0.25f);
    }
    private string GetBuildTime(int seconds)
    {
        string bTime = "none";

        int dTime = (int)(seconds / 86400f);

        if(dTime > 1)
        {
            bTime = dTime + " D";
        }
        else
        {
            int hTime = (int)(seconds / 1440);

            bTime = hTime + " H";
        }

        return bTime;
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
