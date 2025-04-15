using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerStateBase
{
    private float wallJumpForce = 10f;    // 벽 점프력
    private float stopInputTime = 0.1f;    // 벽 점프 후 플레이어 입력이 제한되는 시간
    // 무한 벽 점프 방지용
    private bool isWallClinging = false;
    // 벽 점프 각도 변경 유지 시간인지 체크
    private bool keepRotation = false;

    public PlayerJumpState(PlayerContextData data, PlayerStateFactory factory) : base(data, factory)
    {
    }

    public override void OnEnter()
    {
        //context.Controller.Jump();
        EditorLog.Log("Jump");
    }

    public override void OnUpdate()
    {
        context.Controller.Move();

        if(context.Controller.IsGround)
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
            if(context.FormController.CurFormData.FormName == "Cat")
            {
                Vector2 dir = new Vector2(context.Controller.MoveDir.normalized.x, 0);
                RaycastHit2D hit = Physics2D.BoxCast(context.BoxCollider.bounds.center, context.BoxCollider.bounds.size,
                    0f, dir, 0.1f, context.Controller.GroundLayer);

                if (hit.collider != null && context.Controller.MoveDir.x != 0 && !isWallClinging)
                {
                    context.Rigid.velocity = Vector2.zero;
                    context.Rigid.gravityScale = 0f;
                    context.StateMachine.InvokeAfter(1f, SetGravity);
                    isWallClinging = true;
                    //context.Rigid.velocity = Vector2.zero;
                    //context.Rigid.AddForce(Vector2.up * wallJumpForce, ForceMode2D.Impulse);
                }
            }
        }
    }

    void SetGravity()
    {
        context.Rigid.gravityScale = 1f;
    }

    public override void OnExit()
    {
        isWallClinging = false;
    }
}
