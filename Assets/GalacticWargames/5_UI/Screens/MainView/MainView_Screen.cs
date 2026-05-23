using System;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class MainView_Screen : UIScreen
{
    //System
    [SerializeField] private GameObject gameView;
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

    public static event Action OnMainViewLoaded;
    public override void Show()
    {
        base.Show();

        gameView.SetActive(true);
        OnMainViewLoaded?.Invoke();
    }
    public override void Hide()
    {
        gameView.SetActive(false);
    }
}
