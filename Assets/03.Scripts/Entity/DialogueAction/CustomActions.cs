using System;
using System.Collections.Generic;
using UnityEngine;

public static class CustomActions
{
    private static Dictionary<CustomActionType, Action<string>> actionDict = new();

    public static void Init()
    {
        actionDict[CustomActionType.GoToEnding] = PlayEnding;
        actionDict[CustomActionType.MoveTo] = PlayerMoveTo;
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

    public static void PlayerMoveTo(string param)
    {
        var split = param.Split(',');
        if (split.Length != 2)
        {
            EditorLog.LogError($"PlayerMoveToAction : Invalid param: {param}");
            return;
        }
        
        if (!float.TryParse(split[0], out var x) || !float.TryParse(split[1], out var y))
        {
            EditorLog.LogError($"PlayerMoveToAction : Invalid param: {param}");
            return;
        }
        
        var player = Managers.Instance.GameManager.Player;
        player.transform.position = new Vector3(x, y, player.transform.position.z);
    }
}
