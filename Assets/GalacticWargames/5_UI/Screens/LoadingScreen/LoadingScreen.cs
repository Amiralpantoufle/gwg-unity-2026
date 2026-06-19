using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.CullingGroup;

public class LoadingScreen : MonoBehaviour
{  
    public static LoadingScreen Instance;
    [SerializeField] private Slider loadingSlider;

    private void Awake()
    {
        Instance = this;
    } 

    public async Task BootProcess()
    {
        await BootStrap_Loader.Instance.Init_BootStrap();

        //Données chargées donc Token Valide -> Redirection MainScreen
        if(BootStrap_Loader.Instance.isLoaded)
        {
            loadingSlider.value = 1f;

            UIStateManager.Instance.SetState(UIState.Loggedin);
            ToastManager.Instance.GenerateToast("Logged in", 0, 10f);
        }

        //Données non chargées -> Force logout -> Redirection AuthScreen
        else
        {
            loadingSlider.value = 0f;
            UIStateManager.Instance.SetState(UIState.Loggedout);
            ToastManager.Instance.GenerateToast("Logged out", 1, 10f);
        }

        CloseScreen();
    }

    private void CloseScreen()
    {
        gameObject.SetActive(false);
    }
}
