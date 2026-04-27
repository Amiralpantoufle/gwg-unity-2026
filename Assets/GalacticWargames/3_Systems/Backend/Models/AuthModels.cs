//Login
[System.Serializable]
public class LoginRequest
{
    public string email;
    public string password;
}

[System.Serializable]
public class LoginOutput
{
    public string token;
}

[System.Serializable]
public class LoginResponse
{
    public bool error;
    public string error_msg;
    public LoginOutput output;
}
//Reset Password
[System.Serializable]
public class ForgotPasswordRequest
{
    public string email;
}

[System.Serializable]
public class ForgotPasswordResponse
{
    public bool error;
    public string error_msg;
}
[System.Serializable]
public class ResetPasswordRequest
{
    public string email;
    public string code;
    public string newPassword;
    public string newConfirmPassword;
}

//Refresh
[System.Serializable]
public class RefreshRequest
{
    public string refresh_token;
}

[System.Serializable]
public class RefreshOutput
{
    public string accessToken;
    public string refreshToken;
}

[System.Serializable]
public class RefreshResponse
{
    public bool error;
    public RefreshOutput output;
}


//Register
[System.Serializable]
public class RegisterRequest
{
    public string name;
    public string email;
    public string password;
    public int id_civ;
    public string device_id;
}

[System.Serializable]
public class User
{
    public int id;
    public string name;
    public string email;
    public int idciv_usr;
    public int experience;
    public int level;
}

[System.Serializable]
public class RegisterOutput
{
    public string token;
    public User user;
}

[System.Serializable]
public class RegisterResponse
{
    public bool error;
    public RegisterOutput output;
}