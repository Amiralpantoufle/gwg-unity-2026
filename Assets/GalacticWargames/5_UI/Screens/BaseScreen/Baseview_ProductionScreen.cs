using TMPro;
using UnityEngine;

public class Baseview_ProductionScreen : BaseView_BuildingScreen
{
    [SerializeField] private int prodBuildingIndex;

    //Main Content
    public TextMeshProUGUI[] txt_baseProductionQty;
    public TextMeshProUGUI[] txt_bonusProductionQty;
    public TextMeshProUGUI[] txt_totalProductionQty;

    //Bonus Content
    public TextMeshProUGUI[] txt_bonusQty;
    public TextMeshProUGUI txt_bonusResult;

    private void OnEnable()
    {
        Load_Production();
    }
    private void Load_Production()
    {
        //set main values
        int prodM = 0;

        Production_Model prod = BaseView_Screen.Instance._BaseData.local_economy.production_projection;
        if (prodBuildingIndex == 0)
            prodM = prod.by_resource.carbon;
        else if (prodBuildingIndex == 1)
            prodM = prod.by_resource.hydrogene;
        else
            prodM = prod.by_resource.energie;

        int prodH = prodM * 60;
        int prodD = prodH * 24;

        int bonusM = 0;
        int bonusH = prodM * 60;
        int bonusD = prodH * 24;

        int totalM = prodM + bonusM;
        int totalH = prodH + bonusH;
        int totalD = prodD + bonusD;

        //Get Total Production
        for (int i = 0; i < txt_totalProductionQty.Length; i++)
            txt_totalProductionQty[i].text = "0";

        //Display GUI
        txt_baseProductionQty[0].text = prodM.ToString();
        txt_baseProductionQty[1].text = prodH.ToString();
        txt_baseProductionQty[2].text = prodD.ToString();

        txt_bonusProductionQty[0].text = bonusM.ToString();
        txt_bonusProductionQty[1].text = bonusH.ToString();
        txt_bonusProductionQty[2].text = bonusD.ToString();

        txt_totalProductionQty[0].text = totalM.ToString();
        txt_totalProductionQty[1].text = totalH.ToString();
        txt_totalProductionQty[2].text = totalD.ToString();

        //Load Bonus production
        for (int i = 0; i < txt_bonusQty.Length; i++)
            txt_bonusQty[i].text = "0";
        txt_bonusResult.text = (0 + 0 + 0).ToString();
    }
}
