using TMPro;
using UnityEngine;

public class Auth_RegisterData
{
    public string username;
    public string email;
    public string password;
    public int civ=0;
}
public class Auth_Register_Popup : UIScreen
{
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_InputField passwordInput, confirmPasswordInput;

    [SerializeField] private GameObject civChoicePopup;
    Auth_RegisterData authData;

    private void OnEnable()
    {
        civChoicePopup.SetActive(false);
    }
    /// <summary>
    /// Récupère les données inputs field et traite les erreurs
    /// </summary>
    public void OnTryRegister()
    {
        string regUserName = usernameInput.text.ToString();
        string regEmail = emailInput.text.ToString();
        string regPassword = passwordInput.text.ToString();
        string regConfirmPassword = confirmPasswordInput.text.ToString();

        //Vérifier si les MDP correspondent
        if(regPassword != regConfirmPassword)
        {
            TreatRegisterErrors(0);
            return;
        }
        //Vérifier si adresse mail disponible

        //Vérifier niveau sécurité du MDP


        //Serialize les données du nouveau compte
        authData = new Auth_RegisterData();
        authData.username = regUserName; authData.email = regEmail; authData.password = regPassword;

        //Ajouter le choix de la civilisation
        //
        civChoicePopup.SetActive(true);

        //Play Audio Feedback
        AudioManager.Instance.PlaySound("ButtonInput_Confirm");
    }
    public void RegisterCivilization(int civ)
    {
        if (authData != null)
        {
            authData.civ = civ;
        }
        SendRegisterData(authData);

        //Close Register Popup
        civChoicePopup.SetActive(false);
        //Play Audio Feedback
        AudioManager.Instance.PlaySound("ButtonInput_Confirm");
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
    private void SendRegisterData(Auth_RegisterData data)
    {
        AuthManager.Instance.Register(data.username, data.email,data.password,data.civ);
    }
    public void OnClose()
    {
        EventBus.Publish(new HidePopupEvent
        {
            hidePopup = this
        });
    }
}
