using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    //Listeners
    private void OnEnable()
    {
        EventBus.Subscribe<ShowPopupEvent>(OnShowPopup);
        EventBus.Subscribe<HidePopupEvent>(OnHidePopup);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe<ShowPopupEvent>(OnShowPopup);
        EventBus.Unsubscribe<HidePopupEvent>(OnHidePopup);
        
    }
    private void OnShowPopup(ShowPopupEvent e)
    {
        e.popup.transform.SetAsLastSibling(); // passe au-dessus
        e.popup.Show();
    }
    private void OnHidePopup(HidePopupEvent e)
    {
        e.hidePopup.Hide();
    }
}
