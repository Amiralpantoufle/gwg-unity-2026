using UnityEngine;

[System.Serializable]
public class ApiResponse<T>
{
    public bool error;
    public string error_code;
    public string error_msg;
    public string error_validator;
    public T output;
}
