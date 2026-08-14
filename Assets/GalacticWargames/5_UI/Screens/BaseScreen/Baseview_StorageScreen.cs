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
        int[] r = new int[3];
        r[0] = 20;
        r[2] = 20;
        RefreshStorageGauge(r,10,200);
    }
    public void RefreshStorageGauge(int[] resourcesStored, int securedStored, int availableStorage)
    {
        if (availableStorage <= 0)
            return;

        float start = 0f;

        float securedRatio = (float)securedStored / availableStorage;
        SetFill(protected_resourceStored, start, start + securedRatio);
        protected_resourceStored.gameObject.SetActive(securedStored > 0);

        start += securedRatio;

        int stored = 0;
        for (int i = 0; i < resourcesStored.Length; i++)
        {
            stored += resourcesStored[i];
            txt_Resources[i].text = resourcesStored[i].ToString();
        }

        float storedRatio = (float)stored / availableStorage;
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
