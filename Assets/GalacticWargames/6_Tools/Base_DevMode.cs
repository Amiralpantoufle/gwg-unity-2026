using UnityEngine;

public class Base_DevMode : MonoBehaviour
{
    public Vector2 tilePos;
    public int buildingID;
    public void ForceBuild()
    {
        Debug.Log("Building asset !");

        BuildingConstructionRequest request = new BuildingConstructionRequest
        {
            id_oes = GameDataStorage.Instance.GetLastBaseId(),
            id_bat = buildingID,
            x = (int)tilePos.x,
            y = (int)tilePos.y,
        };

        string json = JsonUtility.ToJson(request);

        StartCoroutine(API_Client.Instance.Post("/construction/building", json, OnConstructionQueued));
    }

    private void OnConstructionQueued(string response)
    {
        Debug.Log(response);
    }
}
