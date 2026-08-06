using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Base_SpaceShipAsset : MonoBehaviour
{
    //Main Info
    [SerializeField] private TextMeshProUGUI assetName;
    [SerializeField] private TextMeshProUGUI buildTime;

    [SerializeField] private Image[] ressourceIcons;
    [SerializeField] private TextMeshProUGUI[] ressourceTxt;
    [SerializeField] private Image spaceShipIcon;

    //Context Info
    private spaceShip_Construct loadedConstruct;
    private int[] cost = new int[3];
    Vector2Int tilePos;

    //Extra components
    [SerializeField] private Image lockedIcon;

    public void Load_spaceShipInfo(spaceShip_Construct construct)
    {
        //Load Info
        assetName.text = construct.nom_vas;

        //Define Day Build Time
        buildTime.text = GetBuildTime(construct.vitesse_construction_vas);

        //Swap Icon
/*        VisualDefinition v = GridVisualService.Instance.GetVisual(construct.idiet_bat);
        buildingIcon.sprite = v.imageSprite;*/

        //Display Ressources
        for (int i = 0; i < construct.couts.Length; i++)
        {
            cost[i] = construct.couts[i].nombre_vre;
            ressourceTxt[i].text = construct.couts[i].nombre_vre.ToString();
            NeedsRessources(construct.couts[i].nombre_vre, i);
        }

        //Callback
        loadedConstruct = construct;
        
        //Display Helper Pannel
        //GetComponent<Button>().onClick.AddListener(Display_ContextPannel);
    }
    public void ClickOnBuild()
    {
        Debug.Log("Building asset !");

        int nombre = transform.GetComponentInParent<Builder_QueueSelector>()._SelectedAmount;

        if(nombre > 0)
        {
            SpaceShipConstructionRequest request = new SpaceShipConstructionRequest
            {
                id_oes = GameDataStorage.Instance.GetLastBaseId(),
                id_vas = loadedConstruct.id_vas,
                nombre = nombre
            };

            string json = JsonUtility.ToJson(request);

            StartCoroutine(API_Client.Instance.Post("/construction/ship", json, OnConstructionQueued));
        }
    }
    private void OnConstructionQueued(string response)
    {
        Debug.Log(response);
        gameObject.SetActive(false);
    }

    //Utility
    private void NeedsRessources(int quantity, int ressourceIndex)
    {
        if (quantity > 0)
        {
            ressourceIcons[ressourceIndex].color = Color.white;
            ressourceTxt[ressourceIndex].color = Color.white;
        }
        else
        {
            ressourceIcons[ressourceIndex].color = new Color(1, 1, 1, 0.25f);
            ressourceTxt[ressourceIndex].color = Color.red;
        }
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
    public int GetMaxAvailable()
    {
        BaseView_Screen baseScreen = FindAnyObjectByType<BaseView_Screen>();
        if (baseScreen == null) Debug.LogError("Couldn't find BaseScreen script in scene0");

        if (cost == null || cost.Length != 3)
        {
            Debug.LogError("Le coût doit contenir 3 ressources.");
            return 0;
        }

        // Si une ressource n'est pas nécessaire, elle ne limite pas la production.
        int carbonLimit = cost[0] > 0 ? baseScreen._AvailableRessources._AvailableCarbon / cost[0] : int.MaxValue;
        int hydrogenLimit = cost[1] > 0 ? baseScreen._AvailableRessources._AvailableHydrogen / cost[1] : int.MaxValue;
        int stoneLimit = cost[2] > 0 ? baseScreen._AvailableRessources._AvailableStone / cost[2] : int.MaxValue;

        return Mathf.Min(carbonLimit, hydrogenLimit, stoneLimit);
    }
}
