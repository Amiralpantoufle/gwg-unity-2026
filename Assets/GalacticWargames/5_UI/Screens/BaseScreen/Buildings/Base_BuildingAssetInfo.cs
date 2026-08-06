using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class Base_BuildingAssetInfo : MonoBehaviour
{
    //Components
    [SerializeField] private TextMeshProUGUI GUI_Name;
    [SerializeField] private TextMeshProUGUI GUI_description;
    [SerializeField] private TextMeshProUGUI[] costs;

    [SerializeField] private Button buttonConstruct;

    private building_Construct loadedConstruct;
    private Vector2Int tilePos;

    public void Load_Info(building_Construct construct, Vector2Int newTilePos)
    {
        loadedConstruct = construct;
        tilePos = newTilePos;

        GUI_Name.text = construct.nom_bat;
        GUI_description.text = construct.desc_bat;

        for (int i = 0; i < costs.Length; i++)
        {
            costs[i].text = construct.couts[i].nombre_bre.ToString();
        }

        gameObject.SetActive(true);
    }

    public void AvailableConstruct(bool isAvailable)
    {
        //Set Visibility
        float alpha;

        //Add Callbacks
        buttonConstruct.enabled = isAvailable;

        if(isAvailable)
        {
            alpha = 1f;
            buttonConstruct.onClick.AddListener(ClickOnBuild);

            Debug.Log("Build Available");
        }
        else
        {
            alpha = 0.15f;

            buttonConstruct.onClick.RemoveAllListeners();
            Debug.Log("Build Not Available");
        }

        //Set Button Visibility
        buttonConstruct.GetComponent<CanvasGroup>().alpha = alpha;
    }

    public void ClickOnBuild()
    {
        Debug.Log("Building asset !");

        BuildingConstructionRequest request = new BuildingConstructionRequest
        {
            id_oes = GameDataStorage.Instance.GetLastBaseId(),
            id_bat = loadedConstruct.id_bat,
            x = tilePos.x,
            y = tilePos.y,
        };

        string json = JsonUtility.ToJson(request);

        StartCoroutine(API_Client.Instance.Post("/construction/building",json,OnConstructionQueued));
    }

    private void OnConstructionQueued(string response)
    {
        Debug.Log(response);
        gameObject.SetActive(false);
    }
}
