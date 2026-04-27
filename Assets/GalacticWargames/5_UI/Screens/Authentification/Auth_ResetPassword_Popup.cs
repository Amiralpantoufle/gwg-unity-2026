using TMPro;
using UnityEngine;

public class Auth_ResetPasswordData
{
    public string email;
    public string code;
    public string new_password;
    public string new_password_confirm;
}
public class Auth_ResetPassword_Popup : UIScreen
{
    [SerializeField] private GameObject popup_reset, popup_code;

    [SerializeField] private TMP_InputField confirmationCode;
    [SerializeField] private TMP_InputField newPasswordInput, newConfirmPasswordInput;
    
    private string emailRef;
    private string verificationref;

    //[SerializeField] private GameObject civChoicePopup;
    Auth_ResetPasswordData newPasswordData;

    public void Init_ResetPassword_Process(string email)
    {
        popup_code.SetActive(true);
        popup_reset.SetActive(false);

        emailRef = email;
    }
    public void OnTryVerificationCode()
    {
        verificationref = confirmationCode.text.ToString();

        popup_code.SetActive(false);
        popup_reset.SetActive(true);

        //Play Audio Feedback
        AudioManager.Instance.PlaySound("ButtonInput_Confirm");
    }
    /// <summary>
    /// Récupère les données inputs field et traite les erreurs
    /// </summary>
    public void OnTryResetPassword()
    {
        string email = emailRef;
        string code = verificationref;
        string newPassword = newPasswordInput.text.ToString();
        string newConfirmPassword = newConfirmPasswordInput.text.ToString();

        //Vérifier si les MDP correspondent
        if(newPassword != newConfirmPassword)
        {
            TreatRegisterErrors(0);
            return;
        }
        //Vérifier niveau sécurité du MDP


        //Serialize les données du nouveau compte
        newPasswordData = new Auth_ResetPasswordData();
        newPasswordData.email = email; ; newPasswordData.code = code; newPasswordData.new_password = newPassword; newPasswordData.new_password_confirm = newConfirmPassword;

        AuthManager.Instance.ResetPassword(newPasswordData);

        //Play Audio Feedback
        AudioManager.Instance.PlaySound("ButtonInput_Confirm");
    }
    public void ConfirmResetPassword()
    {

    }
    private void TreatRegisterErrors(int error)
    {
       switch(error)
        {
            case 0://Passwords Mismatch
                base.PostToken("Passwords do not match", 1, 10f);
                break;
            case 1://Mail already exists
                base.PostToken("Email adress already exists", 1, 10f);
                break;
            case 2://Mail already exists
                base.PostToken("Password security is too low", 1, 10f);
                break;
        }
    }
    private void SendResetData(Auth_RegisterData data)
    {
        //AuthManager.Instance.Register(data.username, data.email,data.password,data.civ);
    }
    public void OnClose()
    {
        EventBus.Publish(new HidePopupEvent
        {
            hidePopup = this
        });
    }
}
