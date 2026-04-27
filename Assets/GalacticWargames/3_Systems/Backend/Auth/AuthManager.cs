using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// gérer login / register / refresh
/// </summary>
public class AuthManager : MonoBehaviour
{
    public static AuthManager Instance;
    [SerializeField] private Auth_ResetPassword_Popup popup_ForgotPassword;

    private const string ACCESS_KEY = "EMAIL";
    private string verificationEmail;

    private void Awake()
    {
        Instance = this;
    }

    //Login
    public void Login(string email, string password)
    {
        var data = new LoginRequest
        {
            email = email,
            password = password
        };

        string json = JsonUtility.ToJson(data);

        StartCoroutine(API_Client.Instance.Post("/auth/login", json, OnLoginResponse));
    }
    private void OnLoginResponse(string json)
    {
        Debug.Log("LOGIN RESPONSE: " + json);

        LoginResponse response = JsonUtility.FromJson<LoginResponse>(json);

        if (!response.error)
        {
            string token = response.output.token;

            API_Client.Instance.SetToken(token);
            UIStateManager.Instance.SetState(UIState.Loggedin);

            GenerateToast("LOGIN SUCCESS", 0, 3f);
            Debug.Log("LOGIN SUCCESS");
        }
        else
        {
            UIStateManager.Instance.SetState(UIState.Loggedout);
            GenerateToast("LOGIN FAILED", 1, 3f);
            Debug.Log("LOGIN FAILED: " + response.error_msg);
        }
    }

    //Register
    public void Register(string name, string email, string password, int idCiv)
    {
        var data = new RegisterRequest
        {
            name = name,
            email = email,
            password = password,
            id_civ = idCiv,
            device_id = SystemInfo.deviceUniqueIdentifier
        };

        string json = JsonUtility.ToJson(data);

        StartCoroutine(API_Client.Instance.Post("/auth/register", json, OnRegisterResponse));
    }
    private void OnRegisterResponse(string json)
    {
        Debug.Log("REGISTER RESPONSE: " + json);

        RegisterResponse response = JsonUtility.FromJson<RegisterResponse>(json);

        if (!response.error)
        {
            API_Client.Instance.SetToken(response.output.token);

            ToastManager.Instance.GenerateToast("REGISTER SUCCESS",1,10f);
        }
        else
        {
            ToastManager.Instance.GenerateToast("REGISTER FAILED", 1,10f);
        }
    }

    //ResetPassword
    public void ForgotPassword(string email)
    {
        var data = new ForgotPasswordRequest
        {
            email = email,
        };
        verificationEmail = email;

        string json = JsonUtility.ToJson(data);
        StartCoroutine(API_Client.Instance.Post("/auth/forgot-password/request", json, OnForgotResponse));
    }
    private void OnForgotResponse(string json)
    {
        Debug.Log("Email Check RESPONSE: " + json);
        LoginResponse response = JsonUtility.FromJson<LoginResponse>(json);

        if (!response.error)
        {
            ToastManager.Instance.GenerateToast("Verification code sent to : "+ verificationEmail, 0, 10f);

            //Open Popup Reset Password A METTRE Après la confirmation
            EventBus.Publish(new ShowPopupEvent
            {
                popup = popup_ForgotPassword
            });
            popup_ForgotPassword.Init_ResetPassword_Process(verificationEmail);
        }
        else
        {
            ToastManager.Instance.GenerateToast(verificationEmail+"Something went wrong", 0, 10f);
        }
    }
    public void ResetPassword(Auth_ResetPasswordData data)
    {
        var resetData = new ResetPasswordRequest
        {
            email = data.email,
            code = data.code,
            newPassword = data.new_password,
            newConfirmPassword = data.new_password_confirm
        };

        string json = JsonUtility.ToJson(data);
        StartCoroutine(API_Client.Instance.Post("/auth/forgot-password/reset", json, OnResetResponse));
    }
    private void OnResetResponse(string json)
    {
        Debug.Log("Reset RESPONSE: " + json);
        LoginResponse response = JsonUtility.FromJson<LoginResponse>(json);

        if (!response.error)
        {
            ToastManager.Instance.GenerateToast("New password defined!", 1, 10f);
            popup_ForgotPassword.ConfirmResetPassword();
        }
        else
        {
            ToastManager.Instance.GenerateToast(verificationEmail + " invalid credentials", 0, 10f);
            verificationEmail = null;

        }
    }

    //Refresh
    public IEnumerator RefreshTokenCoroutine()
    {
        string refreshToken = TokenStorage.GetRefresh();

        var data = new RefreshRequest
        {
            refresh_token = refreshToken
        };

        string json = JsonUtility.ToJson(data);

        bool done = false;

        yield return API_Client.Instance.Post("/auth/refresh", json, (responseJson) =>
        {
            RefreshResponse response = JsonUtility.FromJson<RefreshResponse>(responseJson);

            if (!response.error)
            {
                TokenStorage.Save(response.output.accessToken, response.output.refreshToken);
                API_Client.Instance.SetToken(response.output.accessToken);

                Debug.Log("TOKEN REFRESHED");
            }
            else
            {
                Debug.LogError("REFRESH FAILED → logout");
                TokenStorage.Clear();
            }

            done = true;
        });

        while (!done) yield return null;
    }

    //_________Utility
    private void GenerateToast(string response, int type, float time)
    {
        EventBus.Publish(new ShowToastEvent
        {
            toastType = type,
            message = response,
            duration = time
        });
    }
    public void Force_PopupResetPass()
    {
        //Open Popup Reset Password A METTRE Après la confirmation
        EventBus.Publish(new ShowPopupEvent
        {
            popup = popup_ForgotPassword
        });
        popup_ForgotPassword.Init_ResetPassword_Process("visodoigt@gmail.com");
    }
}