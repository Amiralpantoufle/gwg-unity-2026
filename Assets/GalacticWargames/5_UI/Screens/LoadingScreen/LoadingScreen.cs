using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.CullingGroup;

public class LoadingScreen : MonoBehaviour
{  
    public static LoadingScreen Instance;
    public LoadingService loadingService;
    public float Progress { get; private set; }
    public string Message { get; private set; }

    //Components
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private TextMeshProUGUI loadingMessage;

    private void Awake()
    {
        Instance = this;
        loadingService = new LoadingService();
        loadingService.OnProgressChanged += UpdateUI;

    }

    private void UpdateUI(float progress, string message)
    {
        loadingSlider.value = progress;
        loadingMessage.text = message;  

    }

    public async Task BootProcess()
    {
        await BootStrap_Loader.Instance.Init_BootStrap();

        //Données chargées donc Token Valide -> Redirection MainScreen
        if(BootStrap_Loader.Instance.isLoaded)
        {
            loadingService.SetProgress(0.4f, "Boot Process"); 

            UIStateManager.Instance.SetState(UIState.Loggedin);
            ToastManager.Instance.GenerateToast("Logged in", 0, 10f);
        }

        //Données non chargées -> Force logout -> Redirection AuthScreen
        else
        {
            loadingService.SetProgress(1f, "Failed to connect"); 

            UIStateManager.Instance.SetState(UIState.Loggedout);
            ToastManager.Instance.GenerateToast("Logged out", 1, 10f);
        }

        //CloseScreen();
    }

    public void CloseScreen()
    {
        gameObject.SetActive(false);
    }
}

public class LoadingService
{
    public event Action<float, string> OnProgressChanged;
    public void SetProgress(float progress, string message)
    {
        OnProgressChanged?.Invoke(progress, message);
    }
}
