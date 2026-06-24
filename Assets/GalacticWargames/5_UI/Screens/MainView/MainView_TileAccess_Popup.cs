using TMPro;
using UnityEngine;

public class MainView_TileAccess_Popup : UIScreen
{
    [SerializeField] private TextMeshProUGUI tileName;

    public void LoadTileViewData(TileView tile)
    {
        tileName.text = tile.gameObject.name;
    }
    public void OnClose()
    {
        EventBus.Publish(new HidePopupEvent
        {
            hidePopup = this
        });
    }
}
