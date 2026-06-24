using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainView_Screen : UIScreen
{
    //System
    [SerializeField] private GameObject gameView;
    //[SerializeField] private UIScreen tileAccess_Popup;
    //Profile
    [SerializeField] private TextMeshProUGUI username;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] public Slider xpGauge;
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

        //Vérifie si bootstrap est bien initialisé
        if(storage.CurrentBase == null)
        {
            await BootStrap_Loader.Instance.Init_BootStrap();
            if (storage.CurrentBase == null) Debug.LogError("Couldn't load player base");
        }

        //Récupération API des infos
        string json = await API_Client.Instance.GetAsync("/user/getUserData");
        if (string.IsNullOrEmpty(json)) return ;

        ApiResponse<UserDataOutput> response = JsonConvert.DeserializeObject<ApiResponse<UserDataOutput>>(json);
        if (response == null)
        {
            Debug.LogError("Impossible de parser BaseIndex");
            return;
        }
        if (response.error)
        {
            Debug.LogError($"API ERROR : {response.error_code} - {response.error_msg}");
            return;
        }
        if (response.output == null)
        {
            Debug.Log("BaseIndex output null");
            return;
        }

        ParseUserInfo(response.output, storage);
        Debug.Log("Info Loaded");
    }
    private void ParseUserInfo(UserDataOutput userData, GameDataStorage storage)
    {
        storage.SetUserStartData(userData.infos_user);

        username.text = storage._Username;
        level.text = storage._Level.ToString();
        xpGauge.value = storage._Experience;

        energyStone_Quantity.text = userData.oes_ressources_oer[0].nombre_oer.ToString();
        carbon_Quantity.text = userData.oes_ressources_oer[1].nombre_oer.ToString();
        hydrogen_Quantity.text = userData.oes_ressources_oer[2].nombre_oer.ToString();

        Debug.Log("Infos user loaded");
    }

    public void SwitchLevel()
    {
        Debug.Log("Start Switch Level");
        GridManager.Instance.SwitchLevel();
    }
}
