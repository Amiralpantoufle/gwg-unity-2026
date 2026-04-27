using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
/// <summary>
/// envoyer les requêtes au serveur
/// </summary>
public class API_Client : MonoBehaviour
{
    public static API_Client Instance;

    private string baseUrl = "https://admindev2.galacticwargames.com/api";
    private string accessToken;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        accessToken = TokenStorage.GetAccess();
    }

    public void SetToken(string token)
    {
        accessToken = token;
        TokenStorage.Save(token, TokenStorage.GetRefresh());
    }

    public IEnumerator Get(string endpoint, System.Action<string> callback)
    {
        yield return SendRequest("GET", endpoint, null, callback);
    }

    public IEnumerator Post(string endpoint, string json, System.Action<string> callback)
    {
        yield return SendRequest("POST", endpoint, json, callback);
    }

    private IEnumerator SendRequest(string method, string endpoint, string json, System.Action<string> callback)
    {
        UnityWebRequest req;

        if (method == "POST")
        {
            req = new UnityWebRequest(baseUrl + endpoint, "POST");
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            req.uploadHandler = new UploadHandlerRaw(bodyRaw);
            req.downloadHandler = new DownloadHandlerBuffer();
        }
        else
        {
            req = UnityWebRequest.Get(baseUrl + endpoint);
        }

        AddHeaders(req);

        yield return req.SendWebRequest();

        //Gestion auto expiration token
        if (req.responseCode == 401)
        {
            Debug.Log("TOKEN EXPIRED → refresh");

            yield return AuthManager.Instance.RefreshTokenCoroutine();

            AddHeaders(req);
            yield return req.SendWebRequest();
        }

        callback?.Invoke(req.downloadHandler.text);
    }
    private void AddHeaders(UnityWebRequest req)
    {
        req.SetRequestHeader("Content-Type", "application/json");

        if (!string.IsNullOrEmpty(accessToken))
            req.SetRequestHeader("Authorization", "Bearer " + accessToken);
    }

}
