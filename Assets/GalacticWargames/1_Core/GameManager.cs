using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SyncManager syncManager;
    [SerializeField] private LoadingScreen loading;

    public bool DEVOP_ForceConnexion;

    private void Start()
    {
        EventBus.Subscribe<UIStateChangedEvent>(onStateChanged);
        DontDestroyOnLoad(transform);

        if(DEVOP_ForceConnexion)
        {
            UIStateManager.Instance.SetState(UIState.Loggedin);
            return;
        }


        UIStateManager.Instance.SetState(UIState.Loading);

/*        //STARTUP PROCESS
        Invoke(nameof(Boot), 1f);*/
    }

    private async void Boot()
    {
        if (LoadingScreen.Instance != null)
        {
            LoadingScreen.Instance.gameObject.SetActive(true);

            var bootProcess = new Progress<float>(p => { loading.loadingService.SetProgress(Mathf.Lerp(0f, 0.40f, p), "Boot Process"); });
            await LoadingScreen.Instance.BootProcess();

            //Token Error
            if (LoadingScreen.Instance.Progress < 1)
            {
                var mapProgress = new Progress<float>(p => { loading.loadingService.SetProgress(Mathf.Lerp(0.40f, 0.95f, p), "Génération de la carte"); });
                await GridManager.Instance.LaunchProcess();

                UIStateManager.Instance.SetState(UIState.Loggedin);
                Debug.Log("Boot Process Completed");
            }
            else
                UIStateManager.Instance.SetState(UIState.Loggedout);

            loading.CloseScreen();
        }

    }
    public async void Logout()
    {
        var response = await LogoutResponse<object>($"/auth/logout");
        if(response!=null)
        {
            UIStateManager.Instance.SetState(UIState.Loggedout);
        }
    }
    private async Task<ApiResponse<T>> LogoutResponse<T>(string endpoint)
    {
        string json = await API_Client.Instance.PostAsync(endpoint);

        if (string.IsNullOrEmpty(json))
            return null;

        var response = JsonConvert.DeserializeObject<ApiResponse<T>>(json);
        if (response == null || response.error)
        {
            Debug.LogError($"Null Logout Response or error detected.");
            return null;
        }

        return response;
    }

    void onStateChanged(UIStateChangedEvent e)
    {
        switch (e.newState)
        {
            case UIState.Loggedout:

                EventBus.Publish(new ReplaceScreenEvent
                {
                    screenID = ScreenID.Auth
                });

                Debug.LogWarning("Switched to loggedout state");
                ToastManager.Instance.GenerateToast("Logged out", 1, 10f);
                break;

            case UIState.Loggedin:

                //BootStrap_Loader.Instance.Init_BootStrap();

                EventBus.Publish(new ReplaceScreenEvent
                {
                    screenID = ScreenID.Main
                });

                //Start Live Refresh Tick
                syncManager.StartLiveRefresh();

                Debug.LogWarning("Switched to loggedin state");
                ToastManager.Instance.GenerateToast("Logged in", 0, 10f);
                break;

            case UIState.Loading:
                Invoke(nameof(Boot),1f);
                Debug.LogWarning("Switched to Loading state");
                break;
        }
    }

    private void OnApplicationQuit()
    {
        syncManager.StopRefresh();
    }
}
