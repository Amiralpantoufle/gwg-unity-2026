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


        //Connexion success
        if (!response.error)
        {
            string token = response.output.token;
            string refreshToken = response.output.refresh_token;
            API_Client.Instance.SetTokens(token, refreshToken);

            UIStateManager.Instance.SetState(UIState.Loggedin);
            ToastManager.Instance.GenerateToast("Logged in", 0, 10f);
        }
        else
        {
            UIStateManager.Instance.SetState(UIState.Loggedout);
            ToastManager.Instance.GenerateToast("Failed to log in : " + response.error_msg, 1, 10f);
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

        //Enregistrement success 
        if (!response.error)
        {
            //Défini que le token / ! \ pas le refreshToken
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
            ToastManager.Instance.GenerateToast("New password defined!", 0, 10f);
            popup_ForgotPassword.ConfirmResetPassword();
        }
        else
        {
            ToastManager.Instance.GenerateToast(verificationEmail + " invalid credentials", 1, 10f);
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
                //API_Client.Instance.SetToken(response.output.accessToken);
                API_Client.Instance.SetTokens(response.output.accessToken, response.output.refreshToken);

                Debug.Log("TOKEN REFRESHED");
            }
            else
            {
                Debug.LogWarning("REFRESH FAILED → logout");
                TokenStorage.Clear();
            }

            done = true;
        });

        while (!done) yield return null;
    }

    //_________Utility
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