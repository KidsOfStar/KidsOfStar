using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerStateBase
{
    public PlayerGroundState(PlayerContextData data, PlayerStateFactory factory) : base(data, factory)
    {
    }

    public override void OnExit()
    {
        context.CanCling = true;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if(context.Controller.JumpKeyPressed)
        {
            // 점프 상태로 전환
            context.StateMachine.ChangeState(factory.GetPlayerState(PlayerStateType.Jump));
        }
    }
}
