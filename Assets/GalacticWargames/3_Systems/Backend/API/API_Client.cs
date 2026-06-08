using System.Collections;
using System.Threading.Tasks;
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

    //Token Management
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

    //Legacy API
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

    //Async API
    public async Task<string> GetAsync(string endpoint)
    {
        return await SendRequestAsync("GET", endpoint, null);
    }
    public async Task<string> PostAsync(string endpoint, string json = "")
    {
        return await SendRequestAsync("POST", endpoint, json);
    }
    private async Task<string> SendRequestAsync(string method,string endpoint,string json)
    {
        UnityWebRequest req = CreateRequest(method, endpoint, json);

        await SendUnityWebRequest(req);

        //================================================
        // 401 HANDLING
        //================================================

        if (req.responseCode == 401)
        {
            string body = req.downloadHandler.text;

            Debug.Log("401 BODY: " + body);

            //--------------------------------------------
            // TOKEN EXPIRED
            //--------------------------------------------

            if (body.Contains("token_expired"))
            {
                Debug.Log("TOKEN EXPIRED → refresh");

                // Refresh token
                await CoroutineToTask(
                    AuthManager.Instance.RefreshTokenCoroutine()
                );

                // Retry request
                req.Dispose();

                req = CreateRequest(method, endpoint, json);

                await SendUnityWebRequest(req);
            }

            //--------------------------------------------
            // DEVICE MISMATCH
            //--------------------------------------------

            else if (body.Contains("DEVICE_MISMATCH"))
            {
                Debug.LogError("Device mismatch detected");
            }
        }

        //------------------------------------------------
        // FINAL ERROR CHECK
        //------------------------------------------------

        if (req.result != UnityWebRequest.Result.Success)
        {
            string errorBody = req.downloadHandler.text;

            Debug.LogError(errorBody);

            req.Dispose();

            return null;
        }

        string result = req.downloadHandler.text;

        req.Dispose();

        return result;
    }
    private async Task SendUnityWebRequest(UnityWebRequest request)
    {
        var operation = request.SendWebRequest();

        while (!operation.isDone)
        {
            await Task.Yield();
        }
    }
    private Task CoroutineToTask(IEnumerator coroutine)
    {
        var tcs = new TaskCompletionSource<bool>();

        StartCoroutine(WrapCoroutine(coroutine, tcs));

        return tcs.Task;
    }
    private IEnumerator WrapCoroutine(IEnumerator coroutine,TaskCompletionSource<bool> tcs)
    {
        yield return coroutine;

        tcs.SetResult(true);
    }

    //REQUEST MANAGEMENT
    private UnityWebRequest CreateRequest(string method, string endpoint, string json="")
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
