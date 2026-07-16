using TMPro;
using UnityEngine;

public class BaseView_Screen : UIScreen
{
    [SerializeField] private GameObject gameView;

    //Ressources
    [SerializeField] private TextMeshProUGUI energyStone_Quantity;
    [SerializeField] private TextMeshProUGUI carbon_Quantity;
    [SerializeField] private TextMeshProUGUI hydrogen_Quantity;
    [SerializeField] private TextMeshProUGUI stockCapacity, stockCurrent;


    public async override void Show()
    {
        base.Show();
        gameView.SetActive(true);

        GridManager.OnSwitchToWorld += OpenWorldScreen;
    }
    public override void Hide()
    {
        base.Hide();
        gameView.SetActive(false);
    }

    private void OpenWorldScreen()
    {
        EventBus.Publish(new ReplaceScreenEvent
        {
            screenID = ScreenID.Main
        });
    }
    public async void LeaveBaseView()
    {
        GridLevel level = GridLevel.Planet;

        await GridManager.Instance.Load(level, GameDataStorage.Instance.CurrentBase.position.entity_id);
    }
}
