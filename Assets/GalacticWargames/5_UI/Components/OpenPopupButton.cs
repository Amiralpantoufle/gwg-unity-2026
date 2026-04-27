
using UnityEngine;

public class OpenPopupButton : MonoBehaviour
{
    public UIScreen popup;

    public void OnClick()
    {
        EventBus.Publish(new ShowPopupEvent
        {
            popup = popup
        });
    }
}
