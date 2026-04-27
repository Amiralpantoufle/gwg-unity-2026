using UnityEngine;

public class ClosePopupButton : MonoBehaviour
{
    public UIScreen popup;

    public void OnClick()
    {
        EventBus.Publish(new HidePopupEvent
        {
            hidePopup = popup
        });
    }
}
