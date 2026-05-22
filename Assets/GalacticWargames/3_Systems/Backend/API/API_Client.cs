using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
/// <summary>
/// envoyer les requêtes au serveur
/// </summary>
public class API_Client : MonoBehaviour
{
    public static API_Client Instance;
    public string deviceID;

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
    public void SetTokens(string token, string refresh)
    {
        accessToken = token;
        TokenStorage.Save(token, refresh);
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
        UnityWebRequest req = CreateRequest(method, endpoint, json);

        yield return req.SendWebRequest();

        if (req.responseCode == 401)
        {
            string body = req.downloadHandler.text;

            Debug.Log("401 BODY: " + body);

            if (body.Contains("token_expired"))
            {
                Debug.Log("TOKEN EXPIRED → refresh");

                yield return AuthManager.Instance.RefreshTokenCoroutine();

                req.Dispose();

                req = CreateRequest(method, endpoint, json);
                yield return req.SendWebRequest();
            }
            else if (body.Contains("DEVICE_MISMATCH"))
            {
                Debug.LogError("Device mismatch detected");
            }
        }

        callback?.Invoke(req.downloadHandler.text);

        req.Dispose();

    }


    //REQUEST MANAGEMENT
    private UnityWebRequest CreateRequest(string method, string endpoint, string json)
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

        return req;
    }
    private void AddHeaders(UnityWebRequest req)
    {
        req.SetRequestHeader("Content-Type", "application/json");

        req.SetRequestHeader("X-Device-Id", deviceID);

        if (!string.IsNullOrEmpty(accessToken))
            req.SetRequestHeader("Authorization", "Bearer " + accessToken);
    }

}
