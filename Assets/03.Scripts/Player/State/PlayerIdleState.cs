using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(PlayerContextData data, PlayerStateFactory factory) : base(data, factory) { }

    public override void OnEnter()
    {
        base.OnEnter();
        context.Controller.Anim.SetBool(PlayerAnimHash.AnimMove, false);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if(context.Controller.MoveDir != Vector2.zero && context.Controller.IsGround)
        {
            context.StateMachine.ChangeState(factory.GetPlayerState(PlayerStateType.Move));
        }

        if(!context.Controller.IsGround)
        {
            context.Controller.Move();
        }
    }
}