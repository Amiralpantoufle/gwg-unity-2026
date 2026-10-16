using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class BaseProd_Asset : MonoBehaviour
{
    [Header("Base infos")]
    [SerializeField] TextMeshProUGUI planetName;
    [SerializeField] TextMeshProUGUI planetPos;
    [SerializeField] TextMeshProUGUI baseName;
    [SerializeField] private Image baseIcon;
    [SerializeField] private Image planetIcon;

    [Header("ressources")]
    [SerializeField] TextMeshProUGUI[] ressourceStored;
    [SerializeField] TextMeshProUGUI[] ressourceProd;
    [SerializeField] TextMeshProUGUI globalStored;
    [SerializeField] TextMeshProUGUI globalCapacity;
    [SerializeField] TextMeshProUGUI globalSafe;

    //Gauge Settings
    [SerializeField] private Image gaugeSafe;
    [SerializeField] private Image gaugefull;

    public void Load(BaseProd_AssetData data)
    {
        Load_BaseInfo(data.planetName, data.baseName, data.planetPosition, data.planetIcon, data.baseIcon);
        Load_BaseStorage(data.globalStored, data.globalCapacity, data.globalSafeCapacity);
        Load_BaseProd(data.resourceProduction, data.resourceStored);
    }
    public void Load_BaseInfo(string pName, string bName, Vector2 pPos, Sprite pIcon, Sprite bIcon)
    {
        planetName.text = pName;
        baseName.text = bName;

        planetPos.text = pPos.x.ToString() + "/" + pPos.y.ToString();

        baseIcon.sprite = bIcon;
        planetIcon.sprite = pIcon;
    }

    public void Load_BaseStorage(int gStored, int gCapacity,  int safeCapacity)
    {
        globalStored.text = gStored.ToString();
        globalCapacity.text = "/" + gCapacity.ToString();

        globalSafe.text = safeCapacity.ToString();
    }
    public void Load_BaseProd(Vector3 rProd, Vector3 rStored)
    {
        for (int i = 0; i < ressourceStored.Length; i++)
            ressourceStored[i].text = rStored[i].ToString();

        for (int i = 0; i < ressourceProd.Length; i++)
            ressourceProd[i].text = rProd[i].ToString();

        UpdateGauge();
    }
    private void UpdateGauge()
    {

    }
}
