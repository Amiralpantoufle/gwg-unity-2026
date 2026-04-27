using UnityEngine;

public class OpenScreenButton : MonoBehaviour
{
    public ScreenID targetScreen;

    /// <summary>
    /// Open Screen by ID NavgiationService
    /// </summary>
    public void OnClick()
    {
        Debug.Log("Button input detected");
        EventBus.Publish(new OpenScreenByIDEvent
        {
            screenID = targetScreen,
        });
    }
}

