using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        UIStateManager.Instance.SetState(UIState.Loading);

        //STARTUP PROCESS
        Invoke(nameof(FinishInit), 1f);
        DontDestroyOnLoad(transform);
    }

    private void FinishInit()
    {
        //Set Default State
        UIStateManager.Instance.SetState(UIState.Loggedout);
        EventBus.Publish(new OpenScreenByIDEvent
        {
            screenID = ScreenID.Auth
        });
    }
}
