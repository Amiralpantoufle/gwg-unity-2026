using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Baseview_StorageScreen : BaseView_BuildingScreen
{
    [SerializeField] private RectTransform resourceStored;
    [SerializeField] private RectTransform protected_resourceStored;

    //GUI
    [SerializeField] private TextMeshProUGUI txt_SafeResource;
    [SerializeField] private TextMeshProUGUI[] txt_Resources;

    private void OnEnable()
    {
        BaseInfo_Model baseR = BaseView_Screen.Instance._BaseData;

        int[] r = new int[3];
        for(int i=0; i<3;i++)
        {
            r[i] = baseR.ressources[i].nombre_oer;
        }

        //Ressources edit
        int secured = baseR.local_economy.safe.protected_volume;
        int capacity = baseR.local_economy.storage.max;

        RefreshStorageGauge(r, secured, capacity);
    }
    public void RefreshStorageGauge(int[] resourcesStored, int securedStored, int availableStorage)
    {
        if (availableStorage <= 0)
            return;

        float start = 0f;


        float securedRatio = (float)securedStored / availableStorage;
        SetFill(protected_resourceStored, start, start + securedRatio);

        start += securedRatio;

        int stored = 0;
        for (int i = 0; i < resourcesStored.Length; i++)
        {
            stored += resourcesStored[i];
            txt_Resources[i].text = resourcesStored[i].ToString();
        }

        float storedRatio = ((float)stored-securedStored) / availableStorage;
        SetFill(resourceStored, start, start + storedRatio);
        resourceStored.gameObject.SetActive(stored > 0);

        txt_SafeResource.text = securedStored.ToString();
    }
    private void SetFill(RectTransform fill, float start, float end)
    {
        start = Mathf.Clamp01(start);
        end = Mathf.Clamp01(end);

        fill.anchorMin = new Vector2(start, 0);
        fill.anchorMax = new Vector2(end, 1);

        fill.offsetMin = Vector2.zero;
        fill.offsetMax = Vector2.zero;
    }
}
