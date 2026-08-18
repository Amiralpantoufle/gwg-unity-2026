using TMPro;
using UnityEngine;
using System;
using UnityEngine.UI;

public class RessourceModule : MonoBehaviour
{
    [SerializeField] private int availableCarbon;
    public int _AvailableCarbon { get { return availableCarbon; } }

    [SerializeField] private int availableHydrogen;
    public int _AvailableHydrogen { get { return availableHydrogen; } }

    [SerializeField] private int availableStone;
    public int _AvailableStone { get { return availableStone; } }

    private int available_Storage;
    public int _Available_Storage { get { return available_Storage; } }
    private int current_Storage;

    public bool hasGauges;

    //GUI Elements
    [SerializeField] private TextMeshProUGUI energyStone_Quantity;
    [SerializeField] private TextMeshProUGUI carbon_Quantity;
    [SerializeField] private TextMeshProUGUI hydrogen_Quantity;
    [SerializeField] private TextMeshProUGUI stockCapacity, stockCurrent;

    [SerializeField] private RectTransform carbonFill;
    [SerializeField] private RectTransform hydrogenFill;
    [SerializeField] private RectTransform stoneFill;

    public void RefreshRessources(int[] r)
    {
        if (r == null || r.Length > 3)
        {
            Debug.LogError("Invalid resource package.");
            return;
        }

        if (r.Length > 0)
            availableCarbon = r[0];

        if (r.Length > 1)
            availableHydrogen = r[1];

        if (r.Length > 2)
            availableStone = r[2];

        Display_Amounts();

        if (hasGauges)
            RefreshStorageGauge();
    }
    public void Refresh_StorageCapacity(int capacity)
    {
        //Additionne toutes les ressources
        available_Storage = capacity;
        current_Storage = (availableCarbon + availableHydrogen + availableStone);

        stockCurrent.text = available_Storage.ToString();
        stockCapacity.text = "/" + capacity.ToString();

        RefreshStorageGauge();
    }
    private void RefreshStorageGauge()
    {
        if (available_Storage <= 0)
            return;

        current_Storage = availableCarbon + availableHydrogen + availableStone;
        stockCurrent.text = current_Storage.ToString();

        float carbon = (float)availableCarbon / available_Storage;
        float hydrogen = (float)availableHydrogen / available_Storage;
        float stone = (float)availableStone / available_Storage;

        float start = 0f;

        SetFill(carbonFill, start, start + carbon);
        start += carbon;

        SetFill(hydrogenFill, start, start + hydrogen);
        start += hydrogen;

        SetFill(stoneFill, start, start + stone);

        carbonFill.gameObject.SetActive(availableCarbon > 0);
        hydrogenFill.gameObject.SetActive(availableHydrogen > 0);
        stoneFill.gameObject.SetActive(availableStone > 0);
    }
    private void SetFill(RectTransform fill, float start, float end)
    {
        fill.anchorMin = new Vector2(start, 0);
        fill.anchorMax = new Vector2(end, 1);

        fill.offsetMin = Vector2.zero;
        fill.offsetMax = Vector2.zero;
    }

    private void Display_Amounts()
    {
        carbon_Quantity.text = availableCarbon.ToString();
        hydrogen_Quantity.text = availableHydrogen.ToString();
        energyStone_Quantity.text = availableStone.ToString();
    }

    public bool CanBuild(int[] costs)
    {
        if (costs[0] > availableCarbon)
            return false;
        else if (costs[1] > availableHydrogen)
            return false;
        else if (costs[2] > availableStone)
            return false;
        else
            return true;
    }
}

[System.Serializable]
public class construct_Cost
{
    public int idmtx_bre;
    public int nombre_bre;
}