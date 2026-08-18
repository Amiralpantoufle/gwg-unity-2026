using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Fleet_Instance : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txt_fleetName;
    public string _FleetName { get { return txt_fleetName.text; } set { txt_fleetName.text = value; } }
    [SerializeField] private Image icon_fleetStatus;
    [SerializeField] private Sprite[] icons_Status;

    public bool _Fleet_AttackMode
    {
        get { return _Fleet_AttackMode; }
        set
        {
            _Fleet_AttackMode = value;
            if (_Fleet_AttackMode)
                icon_fleetStatus.sprite = icons_Status[0];
            else
                icon_fleetStatus.sprite = icons_Status[1];

        }
    }


}
