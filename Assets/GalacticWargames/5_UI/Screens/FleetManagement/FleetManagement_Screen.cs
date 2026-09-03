using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FleetManagement_Screen : UIScreen
{
    public static FleetManagement_Screen Instance;
    [SerializeField] private GameObject gameView;
    [SerializeField] private FleetSelection_Pannel fleetPannel;
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
    }

    private async void Init_FleetScreen()
    {
        SpaceShips_Model model = await Load_SpaceShipList(GameDataStorage.Instance._CurrentBase.base_id);
        availableShips = model.ships;

        fleetPannel.Load_AvailableFleet(availableShips);
    }
    public void Load_Fleet(Fleet selectedFleet)
    {
        activeFleet = selectedFleet;
        fleet_txtName.text = activeFleet.name;
        Debug.Log("Ready to edit active fleet");

        //Affichage de la grille
    }

    //API
    private async Task<SpaceShips_Model> Load_SpaceShipList(int id)
    {
        string endpoint = $"/base/ships/{id}";
        var response = await API_Client.Instance.LoadApiResponse<SpaceShips_Model>(endpoint);

        return response.output;
    }
}
