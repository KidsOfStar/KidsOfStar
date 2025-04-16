using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatWallJumpState : PlayerJumpBaseState
{
    private float wallJumpForce = 7f;    // 벽 점프력

    public CatWallJumpState(PlayerContextData data, PlayerStateFactory factory) : base(data, factory)
    {
    }

    public override void OnEnter()
    {
        context.Rigid.AddForce(Vector2.up * wallJumpForce, ForceMode2D.Impulse);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if(context.Rigid.velocity.y <= 0f)
        {
            context.StateMachine.ChangeState(factory.GetPlayerState(PlayerStateType.Jump));
        }
    }
}
