using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatWallClingState : PlayerStateBase
{
    float clingTimer = 0;
    float limit = 1f;

    public CatWallClingState(PlayerContextData data, PlayerStateFactory factory) : base(data, factory)
    {
    }

    public override void OnEnter()
    {
        context.Rigid.velocity = Vector2.zero;
        WallRatationSet();
        context.CanCling = false;
        clingTimer = 0;
    }

    private void WallRatationSet()
    {
        float dirx = Mathf.Sign(context.Controller.MoveDir.x);
        Vector3 rot = Vector3.zero;

        if (dirx > 0)
        {
            rot = new Vector3(0, 0, 90);
        }
        else if (dirx < 0)
        {
            rot = new Vector3(0, 0, -90);
        }

        context.Controller.transform.GetChild(0).rotation = Quaternion.Euler(rot);
        context.Controller.SetCollider();
    }

    public override void OnUpdate()
    {

        context.Controller.Move();

        if(context.Controller.WallJumpKeyDown)
        {
            context.StateMachine.ChangeState(factory.GetPlayerState(PlayerStateType.WallJump));
        }

        if(clingTimer >= limit)
        {
            context.StateMachine.ChangeState(factory.GetPlayerState(PlayerStateType.Jump));
            return;
        }
        else
        {
            clingTimer += Time.deltaTime;
        }

        if(context.Controller.IsGround)
        {
            if (context.Controller.MoveDir == Vector2.zero)
            {
                context.StateMachine.ChangeState(factory.GetPlayerState(PlayerStateType.Idle));
            }

            return;
        }

        if(context.Controller.MoveDir.x == 0 || !WallTouchCheck())
        {
            context.StateMachine.ChangeState(factory.GetPlayerState(PlayerStateType.Jump));
            return;
        }
    }

    bool WallTouchCheck()
    {
        Vector2 dir = new Vector2(Mathf.Sign(context.Controller.MoveDir.x), 0);
        RaycastHit2D hit = Physics2D.BoxCast(context.BoxCollider.bounds.center, context.BoxCollider.bounds.size,
            0f, dir, context.BoxCollider.bounds.size.x, context.Controller.GroundLayer);

        if (hit.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void OnExit()
    {
        context.Controller.transform.GetChild(0).rotation = Quaternion.Euler(Vector3.zero);
        context.Controller.SetCollider();
    }
}