using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Fleet_Instance : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txt_fleetName;
    [SerializeField] private Image icon_fleetStatus;
    [SerializeField] private Sprite[] icons_Status;

    private Fleet loadedFleet;
    private bool fleet_AttackMode;

    public void Load_FleetInstance(Fleet fleet)
    {
        if(fleet != null)
            loadedFleet = fleet;

        txt_fleetName.text = loadedFleet.name;
        Set_AttackMode = loadedFleet.active;

    }

    public void Try_OpenFleetScreen()
    {
        if(loadedFleet != null)
        {
            EventBus.Publish(new OpenScreenByIDEvent
            {
                screenID = ScreenID.fleetScreen,
            });
            FleetManagement_Screen.Instance.Load_Fleet(loadedFleet);
        }
    }

    private bool Set_AttackMode
    {
        get { return fleet_AttackMode; }
        set
        {
            if (fleet_AttackMode)
                icon_fleetStatus.sprite = icons_Status[0];
            else
                icon_fleetStatus.sprite = icons_Status[1];

            fleet_AttackMode = value;
        }
    }
}
