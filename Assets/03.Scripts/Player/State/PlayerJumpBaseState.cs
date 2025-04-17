using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpBaseState : PlayerStateBase
{
    public PlayerJumpBaseState(PlayerContextData data, PlayerStateFactory factory) : base(data, factory)
    {
    }

    public override void OnUpdate()
    {
        context.Controller.Move();

        if (context.Controller.IsGround && !context.Controller.JumpKeyPressed)
        {
            if (context.Controller.MoveDir == Vector2.zero)
            {
                context.StateMachine.ChangeState(factory.GetPlayerState(PlayerStateType.Idle));
            }
            else
            {
                context.StateMachine.ChangeState(factory.GetPlayerState(PlayerStateType.Move));
            }
        }
    }

    public override void OnExit()
    {
        context.CanCling = true;
    }

    protected void WallTouchCheck()
    {
        Vector2 dir = new Vector2(Mathf.Sign(context.Controller.MoveDir.x), 0);
        Vector2 boxSize = context.BoxCollider.bounds.size;
        boxSize.y *= 0.5f;
        RaycastHit2D hit = Physics2D.BoxCast(context.BoxCollider.bounds.center, boxSize,
            0f, dir, 0.1f, context.Controller.GroundLayer);

        RaycastHit2D checkHit = Physics2D.Raycast(context.Controller.transform.position, dir,
            context.BoxCollider.bounds.size.x + 0.1f, context.Controller.GroundLayer);

        if (hit.collider != null && checkHit.collider != null)
        {
            context.StateMachine.ChangeState(factory.GetPlayerState(PlayerStateType.WallCling));
        }
    }
}
