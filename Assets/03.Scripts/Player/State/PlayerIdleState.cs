using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(PlayerContextData data, PlayerStateFactory factory) : base(data, factory) { }

    public override void OnEnter()
    {
        context.Rigid.velocity = Vector2.zero;
    }

    public override void OnExit()
    {
        
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if(context.Controller.MoveDir != Vector2.zero)
        {
            context.StateMachine.ChangeState(factory.GetPlayerState(PlayerStateType.Move));
        }
    }
}