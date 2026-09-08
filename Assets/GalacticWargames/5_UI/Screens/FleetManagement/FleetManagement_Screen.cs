using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FleetManagement_Screen : UIScreen 
{
    public static FleetManagement_Screen Instance;
    [SerializeField] private GameObject gameView;
    [SerializeField] private FleetSelection_Pannel fleetPannel;
    [SerializeField] private FleetNavigationController navigation;

    spaceShip_Construct[] availableShips;
    private Fleet activeFleet;

    //GUI
    [SerializeField] private TMP_InputField fleet_txtName;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public override void Show()
    {
        base.Show();

        //Display GridView
        gameView.SetActive(true);
        int tileSprite = 160;
        GetComponent<Fleet_GridManager>().Generate_EmptyGrid(tileSprite);

        Init_FleetScreen();
    }
    public override void Hide()
    {
        base.Hide();
        gameView.SetActive(false);

        activeFleet = null;
        navigation.enabled = false;
    }

    private async void Init_FleetScreen()
    {
        //Init Navigation
        navigation.enabled = true;
        navigation.GetComponent<BaseNavigationController>().enabled = false;
        navigation.GetComponent<MapNavigationController>().enabled = false;

        //Init available spaceships
        SpaceShips_Model model = await Load_SpaceShipList(GameDataStorage.Instance._CurrentBase.base_id);
        availableShips = model.ships;
        fleetPannel.Load_AvailableShips(availableShips);
    }
    public void Display_ShipsPannel(bool enable)
    {
        if (enable)
        {
            fleetPannel.Show();
            navigation.freezed = true;
        }
        else
        {
            fleetPannel.Hide();
            navigation.freezed = false;

        }
    }

    //Save Load
    public void Load_Fleet(Fleet selectedFleet)
    {
        activeFleet = selectedFleet;
        fleet_txtName.text = activeFleet.name;
        Debug.Log("Ready to edit active fleet");

        //Affichage des vaisseaux sur la grille
        GetComponent<Fleet_GridManager>().Load_FleetShips_OnGrid(selectedFleet.composition);
    }
    public void Try_SaveFleet()
    {
        //SAUVEGARDER UNE FLOTTE DEJA ENREGISTREE
        if (activeFleet != null)
        {
            Save_Fleet();
        }
        //SAUVEGARDER UNE NOUVELEL FLOTTE
        else
        {
            Save_NewFleet();
        }
    }
    private void Save_Fleet()
    {
        Quit_FleetScreen();
    }
    private void Save_NewFleet()
    {
        Quit_FleetScreen();
    }

    private void Quit_FleetScreen()
    {
        Display_ShipsPannel(false);
        EventBus.Publish(new OpenScreenByIDEvent
        {
            screenID = ScreenID.Main,
        });
    }

    //API
    private async Task<SpaceShips_Model> Load_SpaceShipList(int id)
    {
        string endpoint = $"/base/ships/{id}";
        var response = await API_Client.Instance.LoadApiResponse<SpaceShips_Model>(endpoint);

        return response.output;
    }
}
