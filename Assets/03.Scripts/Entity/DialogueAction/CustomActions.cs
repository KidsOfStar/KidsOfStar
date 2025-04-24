using System;
using System.Collections.Generic;
using UnityEngine;

public static class CustomActions
{
    private static Dictionary<DialogActionType, Action<string>> actionDict = new();

    public static void Init()
    {
        actionDict[DialogActionType.GoToEnding] = PlayEnding;
    }
    
    public static void PlayEnding(string param)
    {
        if (!Enum.TryParse<EndingType>(param, out var endingType))
        {
            EditorLog.LogError($"GoToEndingAction : Invalid load scene type: {param}");
            return;
        }
        
        Managers.Instance.GameManager.TriggerEnding(endingType);
    }
}
