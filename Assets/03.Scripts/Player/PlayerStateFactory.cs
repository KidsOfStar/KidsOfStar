using System.Collections.Generic;
using UnityEngine;

public enum PlayerStateType
{
    Idle,
    Move,
    Jump
}

public class PlayerStateFactory : MonoBehaviour
{
    private Dictionary<PlayerStateType, IPlayerState> stateDictionary;
    
    public PlayerStateFactory(PlayerContextData context)
    {
        stateDictionary = new Dictionary<PlayerStateType, IPlayerState>()
        {
            { PlayerStateType.Idle, new PlayerIdleState(context, this) },
            { PlayerStateType.Move, new PlayerMoveState(context, this) },
            { PlayerStateType.Jump, new PlayerJumpState(context, this) }
        };
    }

    public IPlayerState GetPlayerState(PlayerStateType type)
    {
        return stateDictionary[type];
    }
}