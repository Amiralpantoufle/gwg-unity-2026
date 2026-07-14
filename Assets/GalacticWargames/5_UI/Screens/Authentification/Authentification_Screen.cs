using System.Collections;
using TMPro;
using UnityEngine;

public class Authentification_Screen : UIScreen
{
    [SerializeField] private TMP_InputField userEmail;
    [SerializeField] private TMP_InputField userPassword;
    private const string ACCESS_KEY = "EMAIL";

    public override void Show()
    {
        base.Show();
        userEmail.text = PlayerPrefs.GetString(ACCESS_KEY, "");
    }
    public override void Hide()
    {
        base.Hide();
    }
    public void Try_Loggin()
    {
        string email = userEmail.text.ToString();
        string password = userPassword.text.ToString();

        if (password.Length <= 0 || email.Length <= 0)
        {
            ToastManager.Instance.GenerateToast("Add a functional email adress and password", 0, 10f);
            return;
        }

        AuthManager.Instance.Login(email, password);

        //Save Email 
        PlayerPrefs.SetString(ACCESS_KEY, email);
        //Play Audio Feedback
        AudioManager.Instance.PlaySound("ButtonInput_Login");
    }
    public void Try_ForgotPassword()
    {
        string email = userEmail.text.ToString();
        if (email.Length <= 0)
        {
            ToastManager.Instance.GenerateToast("Add a functional email adress", 0, 10f);
            return;
        }

        AuthManager.Instance.ForgotPassword(email);
    }

    //_________Utility
}
