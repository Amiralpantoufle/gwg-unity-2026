using System;
using System.Threading.Tasks;
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
    public async override void Show()
    {
        base.Show();

        await LoadUserInfo();

        gameView.SetActive(true);
        OnMainViewLoaded?.Invoke();
    }
    public override void Hide()
    {
        base.Hide();
        gameView.SetActive(false);
    }

    private async Task LoadUserInfo()
    {
        GameDataStorage storage = GameDataStorage.Instance;

        if (storage != null)
        {
            username.text = storage._Username;
            level.text = storage._Level;
            Debug.Log("Info Loaded");
        }

        if(storage.CurrentBase == null)
        {
            await BootStrap_Loader.Instance.Init_BootStrap();

            if (storage.CurrentBase == null) Debug.LogError("Couldn't load player base");
        }
    }

    public void SwitchLevel()
    {
        GridManager.Instance.SwitchLevel();
    }
}
