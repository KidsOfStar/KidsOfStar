using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateBase : IPlayerState
{
    protected PlayerContextData context;
    protected PlayerStateFactory factory;

    public PlayerStateBase(PlayerContextData data, PlayerStateFactory factory)
    {
        this.context = data;
        this.factory = factory;
    }

    public virtual void OnEnter()
    {
        
    }

    public virtual void OnExit()
    {
        
    }

    public virtual void OnUpdate()
    {
        if (context.Controller.IsGround)
        {
            if (!context.Controller.JumpKeyPressed)
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
        else
        {
            if (context.FormController.CurFormData.FormName == "Cat" && context.Controller.MoveDir.x != 0
                && context.CanCling)
            {
                WallTouchCheck();
            }
        }
    }

    protected void WallTouchCheck()
    {
        Vector2 dir = new Vector2(Mathf.Sign(context.Controller.MoveDir.x), 0);

        RaycastHit2D checkHit = Physics2D.Raycast(context.Controller.transform.position, dir,
            context.BoxCollider.bounds.size.x * 0.7f, context.Controller.GroundLayer);

        if (checkHit.collider != null)
        {
            context.StateMachine.ChangeState(factory.GetPlayerState(PlayerStateType.WallCling));
        }
    }
}
