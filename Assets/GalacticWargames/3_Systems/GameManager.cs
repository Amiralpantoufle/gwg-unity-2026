using UnityEngine;

public class GameManager : MonoBehaviour
{
    //[SerializeField] private LocalizationManager localization;
    private void Start()
    {
        UIStateManager.Instance.SetState(UIState.Loading);

        //Loading UI
        Invoke(nameof(FinishInit), 1f);

        DontDestroyOnLoad(transform);
    }

    private void FinishInit()
    {
        //Set UI state
        UIStateManager.Instance.SetState(UIState.Loggedout);
        EventBus.Publish(new OpenScreenByIDEvent
        {
            screenID = ScreenID.Auth
        });

    }
}
