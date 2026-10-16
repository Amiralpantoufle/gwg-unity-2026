using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MainView_RessourceInfo_Popup : UIScreen
{
    [SerializeField] private GameObject baseProdAsset;
    [SerializeField] private Transform assetsRoot;

    public override void Show()
    {
        base.Show();

        FindAnyObjectByType<MapNavigationController>().freezed = true;
     
        Clear_Bases();
        TEST_GenerateRandomBase();
    }
    public override void Hide()
    {
        base.Hide();
        FindAnyObjectByType<MapNavigationController>().freezed = false;
    }

    private void Add_Base(BaseProd_AssetData data)
    {
        GameObject instance = Instantiate(baseProdAsset, assetsRoot);
        BaseProd_Asset asset = instance.GetComponent<BaseProd_Asset>();

        asset.Load(data);
    }
    private void Clear_Bases()
    {
        for (int i = assetsRoot.childCount - 1; i >= 0; i--)
            Destroy(transform.GetChild(i).gameObject);
    }

    private void TEST_GenerateRandomBase()
    {
        int random = Random.Range(1, 5);

        for (int i = 0; i < random; i++)
        {
            BaseProd_AssetData data = TEST_CreateRandomBaseData();
            Add_Base(data);
        }
    }
    private BaseProd_AssetData TEST_CreateRandomBaseData()
    {
        BaseProd_AssetData data = new BaseProd_AssetData();

        VisualDefinition p = GridVisualService.Instance.GetVisual(143);
        data.planetIcon = p.imageSprite;

        VisualDefinition b = GridVisualService.Instance.GetVisual(91);
        data.baseIcon = b.imageSprite;


        data.planetName = "Planet " + Random.Range(1, 100);
        data.baseName = "Base " + Random.Range(1, 100);

        data.planetPosition = new Vector2(
            Random.Range(0, 100),
            Random.Range(0, 100)
        );

        data.globalCapacity = Random.Range(500, 2000);
        data.globalStored = Random.Range(0, data.globalCapacity);

        data.globalSafeCapacity =
            Mathf.RoundToInt(data.globalCapacity * 0.7f);

        data.resourceStored = new Vector3(
            Random.Range(0, 500),
            Random.Range(0, 500),
            Random.Range(0, 500)
        );

        data.resourceProduction = new Vector3(
            Random.Range(0, 50),
            Random.Range(0, 50),
            Random.Range(0, 50)
        );

        return data;
    }
}

public class BaseProd_AssetData
{
    public string planetName;
    public string baseName;
    public Vector2 planetPosition;

    public Sprite planetIcon;
    public Sprite baseIcon;

    public int globalStored;
    public int globalCapacity;
    public int globalSafeCapacity;

    public Vector3 resourceStored;
    public Vector3 resourceProduction;
}

