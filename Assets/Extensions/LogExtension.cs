using UnityEngine;

public static class LogExtension
{
    public static void LogMissingVariable(string objectName, string variable)
    {
        Debug.LogException(new System.Exception($"{objectName} is missing the \"{variable}\" variable, Please set it in the inspector."));
    }

    public static void LogMissingComponent(string objectName, string componentName)
    {
        Debug.LogException(new System.Exception($"{objectName} is missing the \"{componentName}\" component, Please set it in the inspector."));
    }
}
