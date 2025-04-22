using System.Collections.Generic;
using UnityEngine;

public enum PlayerStateType
{
    Idle,
    Move,
    Jump,
    WallCling,
    WallJump
}

public class PlayerStateFactory
{
    private Dictionary<PlayerStateType, IPlayerState> stateDictionary;
    
    public PlayerStateFactory(PlayerContextData context)
    {
        stateDictionary = new Dictionary<PlayerStateType, IPlayerState>()
        {
            { PlayerStateType.Idle, new PlayerIdleState(context, this) },
            { PlayerStateType.Move, new PlayerMoveState(context, this) },
            { PlayerStateType.Jump, new PlayerJumpState(context, this) },
            { PlayerStateType.WallCling, new CatWallClingState(context, this) },
            { PlayerStateType.WallJump, new CatWallJumpState(context, this) },
        };
    }

    public IPlayerState GetPlayerState(PlayerStateType type)
    {
        return stateDictionary[type];
    }
}