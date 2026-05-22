using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ApiResponse<T>
{
    public bool error;
    public string error_code;
    public string error_msg;
    public Dictionary<string, string[]> error_validator;
    public T output;
}
