using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class MainView_Screen : UIScreen
{
    //Profile
    [SerializeField] private TextMeshProUGUI username;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private Slider xpGauge;
    //Ressources
    [SerializeField] private TextMeshProUGUI energyStone_Quantity;
    [SerializeField] private TextMeshProUGUI carbon_Quantity;
    [SerializeField] private TextMeshProUGUI hydrogen_Quantity;
    [SerializeField] private TextMeshProUGUI stockCapacity,stockCurrent;
}
