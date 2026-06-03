using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(transform);
        UIStateManager.Instance.SetState(UIState.Loading);

        //STARTUP PROCESS
        Invoke(nameof(OnLaunchApp), 1f);
    }

    private async void OnLaunchApp()
    {
        if (LoadingScreen.Instance != null)
        {
            LoadingScreen.Instance.gameObject.SetActive(true);
            await LoadingScreen.Instance.BootProcess();

            Debug.Log("Boot Process Completed");
        }
    }
}
