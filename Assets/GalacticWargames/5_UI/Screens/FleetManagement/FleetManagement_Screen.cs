using System.Threading.Tasks;
using UnityEngine;

public class FleetManagement_Screen : UIScreen
{
    public static FleetManagement_Screen Instance;
    [SerializeField] private GameObject gameView;
    private FleetSelection_Pannel fleetPannel;

    spaceShip_Construct[] availableShips;

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
        gameView.SetActive(true);

        fleetPannel = popupMaster.GetComponent<FleetSelection_Pannel>();
        Init_FleetScreen();
    }
    public override void Hide()
    {
        base.Hide();
        gameView.SetActive(false);
    }

    private async void Init_FleetScreen()
    {
        SpaceShips_Model model = await Load_SpaceShipList(GameDataStorage.Instance._CurrentBase.base_id);
        availableShips = model.ships;

        fleetPannel.Load_AvailableFleet(availableShips);
    }

    private async Task<SpaceShips_Model> Load_SpaceShipList(int id)
    {
        string endpoint = $"/base/ships/{id}";
        var response = await API_Client.Instance.LoadApiResponse<SpaceShips_Model>(endpoint);

        return response.output;
    }
}
