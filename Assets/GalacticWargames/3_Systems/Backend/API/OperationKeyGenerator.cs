using System;
using UnityEngine;

public static class OperationKeyGenerator
{
    public static string Generate(string operation)
    {
        return $"{operation}-{Guid.NewGuid():N}";
    }
}
