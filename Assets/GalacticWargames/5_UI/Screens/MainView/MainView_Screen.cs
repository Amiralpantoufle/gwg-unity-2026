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

    public async override void Show()
    {
        base.Show();

        await LoadUserInfo();
        
        gameView.SetActive(true);

        GridManager.OnSwitchToBase += OpenBaseScreen;

        //Load Ressources module
        GameDataStorage.Instance._Current_RessourceModule = GetComponent<RessourceModule>();
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
        if(storage._CurrentBase == null)
        {
            await BootStrap_Loader.Instance.Init_BootStrap();
            if (storage._CurrentBase == null)
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
        if (userData.oes_ressources_oer != null && userData.oes_ressources_oer.Count > 0)
        {
            int[] r = new int[3];
            for(int i=0; i< userData.oes_ressources_oer.Count; i++)
            {
                r[i] = userData.oes_ressources_oer[i].nombre_oer;
            }

            RessourceModule rModule = GetComponent<RessourceModule>();
            if(rModule!=null)
            {
                rModule.RefreshRessources(r);
            }
        }
        else
            Debug.LogWarning("No ressources detected from Get/Api/user/getUserData");
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
