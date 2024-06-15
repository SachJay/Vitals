using UnityEngine;

public static class LogExtension
{
    public static void LogMissingVariable(string objectName, string variable)
    {
        Debug.LogException(new System.Exception($"{objectName} is missing the \"{variable}\" variable, Please set it in the inspector."));
    }
}
