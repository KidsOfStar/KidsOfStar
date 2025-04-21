using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatWallJumpState : PlayerJumpBaseState
{
    private float wallJumpForce = 5f;    // 벽 점프력
    private float timer = 0;

    public CatWallJumpState(PlayerContextData data, PlayerStateFactory factory) : base(data, factory)
    {
    }

    public override void OnEnter()
    {
        context.Rigid.AddForce(Vector2.up * wallJumpForce, ForceMode2D.Impulse);
        timer = 0;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        timer += Time.deltaTime;

        if(timer >= 0.5f)
        {
            context.StateMachine.ChangeState(factory.GetPlayerState(PlayerStateType.Jump));
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        context.CanCling = true;
        context.Controller.WallJumpKeyDown = false;
    }
}
