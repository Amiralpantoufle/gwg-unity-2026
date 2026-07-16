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

    [SerializeField] private TextMeshProUGUI username;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] public Slider xpGauge;
    //Ressources
    [SerializeField] private TextMeshProUGUI energyStone_Quantity;
    [SerializeField] private TextMeshProUGUI carbon_Quantity;
    [SerializeField] private TextMeshProUGUI hydrogen_Quantity;
    [SerializeField] private TextMeshProUGUI stockCapacity,stockCurrent;


    public async override void Show()
    {
        base.Show();

        await LoadUserInfo();
        
        gameView.SetActive(true);

        GridManager.OnSwitchToBase += OpenBaseScreen;
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
            if (storage.CurrentBase == null)
                Debug.LogWarning("Couldn't load player base");
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
        storage.SetUserStartData(userData);

        username.text = userData.infos_user.name;

       //Asign Experience
        level.text = userData.infos_user.level_progress.current_level.ToString();
        xpGauge.maxValue = userData.infos_user.level_progress.xp_for_next_level;
        xpGauge.value = userData.infos_user.level_progress.xp_in_level;

        //Asign Ressources
        string current = "0";
        if (userData.oes_ressources_oer != null && userData.oes_ressources_oer.Count > 0)
        {
            energyStone_Quantity.text = userData.oes_ressources_oer[0].nombre_oer.ToString();
            carbon_Quantity.text = userData.oes_ressources_oer[1].nombre_oer.ToString();
            hydrogen_Quantity.text = userData.oes_ressources_oer[2].nombre_oer.ToString();

            //Additionne toutes les ressources
            current = (userData.oes_ressources_oer[0].nombre_oer + userData.oes_ressources_oer[1].nombre_oer + userData.oes_ressources_oer[2].nombre_oer).ToString();
        }
        else
        {
            Debug.LogWarning("No ressources detected from Get/Api/user/getUserData");
        }

        stockCurrent.text = current;
        stockCapacity.text = "/" + userData.infos_user.BASE_STOCKAGE_DEFAULT.ToString();

        Debug.Log("Infos user loaded");
    }

    private void OpenBaseScreen()
    {
        EventBus.Publish(new ReplaceScreenEvent
        {
            screenID = ScreenID.Base
        });
    }

    //UI Inputs
    /// <summary>
    /// Button UI to zoom out Level map
    /// </summary>
    public void SwitchLevel()
    {
        GridManager.Instance.SwitchLevel();
    }
}
