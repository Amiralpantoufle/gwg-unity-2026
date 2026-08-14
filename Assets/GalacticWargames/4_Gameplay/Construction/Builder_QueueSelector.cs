using TMPro;
using UnityEngine;

public class Builder_QueueSelector : MonoBehaviour
{
    [SerializeField] private Base_SpaceShipAsset asset;
    public Base_SpaceShipAsset _Asset { get { return asset; } }

    //Selector
    [SerializeField] private Base_QueueDisplayer queue;
    public Base_QueueDisplayer _Queue { get { return queue; } }
    private int selectedAmount;
    private int maxAvailable;

    public int _SelectedAmount { get { return selectedAmount; } }
    //GUI
    [SerializeField] private TextMeshProUGUI txt_Amount;

    public void Update_Amount(bool add)
    {
        maxAvailable = asset.GetMaxAvailable();

        int amount = 1;

        if (add)
            amount += selectedAmount;
        else
            amount = (selectedAmount - amount);

        if(amount <= maxAvailable)
            Display_SelectedAmount(amount);

    }
    public void Update_MaxAmount(bool add)
    {
        maxAvailable = asset.GetMaxAvailable();

        int amount = maxAvailable;

        if (!add)
            amount = 0;

        Display_SelectedAmount(amount);
    }

    private void Display_SelectedAmount(int newAmount)
    {
        if(newAmount >= 0)
        {
            selectedAmount = newAmount;
            txt_Amount.text = newAmount.ToString();
        }
    }

    private void OnEnable()
    {
        Display_SelectedAmount(0);
    }
}
