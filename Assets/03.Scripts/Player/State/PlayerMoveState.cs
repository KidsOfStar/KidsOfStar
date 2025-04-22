using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundState
{
    public PlayerMoveState(PlayerContextData data, PlayerStateFactory factory) : base(data, factory)
    {
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        
        if(context.Controller.IsGround && context.Controller.MoveDir == Vector2.zero)
        {
            context.StateMachine.ChangeState(factory.GetPlayerState(PlayerStateType.Idle));
        }

        context.Controller.Move();
    }
}
