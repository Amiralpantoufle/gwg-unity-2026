using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Base_BuildingLevelAsset : MonoBehaviour
{
    [SerializeField] private Image levelBuilding_Icon;
    [SerializeField] private TextMeshProUGUI[] costs;
    [SerializeField] private TextMeshProUGUI value;

    private Vector2Int selecPos;
    private int selecID;

    public void Load_LevelAsset(building_Construct construct, Vector2Int pos, int idBat)
    {
        selecPos = pos;
        selecID = idBat;

        for (int i=0; i< costs.Length;i++)
        {
            costs[i].text = construct.couts[i].nombre_bre.ToString();
        }

        value.text = construct.production_rate_bat.ToString();

        levelBuilding_Icon.sprite = GridVisualService.Instance.GetVisual(construct.id_bat).imageSprite;
    }

    public void ClickOnBuild()
    {
        Debug.Log("Building asset !");

        BuildingConstructionRequest request = new BuildingConstructionRequest
        {
            id_oes = GameDataStorage.Instance.GetLastBaseId(),
            id_bat = selecID,
            x = selecPos.x,
            y = selecPos.y,
        };

        string json = JsonUtility.ToJson(request);

        StartCoroutine(API_Client.Instance.Post("/construction/building", json, OnConstructionQueued));
    }
    private void OnConstructionQueued(string response)
    {
        //Treat errors
        ApiResponse<string> result = JsonUtility.FromJson<ApiResponse<string>>(response);

        if (result == null)
        {
            Debug.LogError("Réponse de construction invalide.");
            return;
        }

        if (result.error)
        {
            Debug.LogWarning($"Construction impossible : {result.error_code} - {result.error_msg}");
            ToastManager.Instance.GenerateToast("Construction Impossible", 0, 2f);

            return;
        }

        //Success construction
        BuilderQueue_Displayer.Instance.SpawnVignette(0, selecPos);
        gameObject.SetActive(false);
    }
}
