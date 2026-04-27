using UnityEngine;

public class Base_Popup : UIScreen
{
    public void OnClose()
    {
        EventBus.Publish(new HidePopupEvent
        {
            hidePopup = this
        });
    }
}
