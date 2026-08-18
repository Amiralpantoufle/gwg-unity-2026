using System;
using System.Diagnostics.Contracts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Base_BuildingAsset : MonoBehaviour
{
    //Main Info
    [SerializeField] private TextMeshProUGUI assetName;
    [SerializeField] private TextMeshProUGUI buildTime;
    [SerializeField] private Image frameSelector;

    [SerializeField] private Image[] ressourceIcons;
    [SerializeField] private Image buildingIcon;

    //Context Info
    private building_Construct loadedConstruct;
    private int[] cost = new int[3];
    Vector2Int tilePos;

    //Extra components
    [SerializeField] private Image lockedIcon;
    [SerializeField] private CanvasGroup[] greyedOut;

    private Base_BuildingAssetInfo contextPannel;

    public void Load_buildingInfo(building_Construct construct, Base_BuildingAssetInfo context)
    {
        //Load Info
        assetName.text = construct.nom_bat.Substring(0, construct.nom_bat.Length - 2);

        //Define Day Build Time
        buildTime.text = GetBuildTime(construct.vitesse_construction_bat);

        //Swap Icon
        VisualDefinition v = GridVisualService.Instance.GetVisual(construct.idiet_bat);
        buildingIcon.sprite = v.imageSprite;

        //Display Ressources
        for (int i = 0; i < construct.couts.Length; i++)
        {
            cost[i] = construct.couts[i].nombre_bre;
            NeedsRessources(construct.couts[i].nombre_bre, i);
        }

        //Callback
        loadedConstruct = construct;
        contextPannel = context;
        GetComponent<Button>().onClick.AddListener(Display_ContextPannel);
    }

    private bool HasRessources()
    {
        BaseView_Screen baseScreen = FindAnyObjectByType<BaseView_Screen>();

        if (baseScreen._AvailableRessources.CanBuild(cost))
            return true;
        else
            return false;
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
        TimeSpan time = TimeSpan.FromSeconds(seconds);

        // Plus d'un jour
        if (time.TotalDays >= 1)
        {
            return $"{time.Days}j{time.Hours:00}h";
        }

        // Plus d'une heure
        if (time.TotalHours >= 1)
        {
            return $"{time.Hours}h{time.Minutes:00}";
        }

        // Moins d'une heure
        return $"{time.Minutes:00}m";
    }

    //Display
    public void Display_ContextPannel()
    {
        TileSelection_Pannel pannel = FindAnyObjectByType<TileSelection_Pannel>();
        pannel._ActiveSelector = frameSelector;
        frameSelector.enabled = true;

        GridBaseTile tile = pannel._LoadedTile._Tile;

        if (tile != null)
            tilePos = new Vector2Int(tile.x, tile.y);
        else
            Debug.LogError("No tile Position referenced");

        contextPannel.Load_Info(loadedConstruct, tilePos);


        //Check if can build
        contextPannel.AvailableConstruct(HasRessources());
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
