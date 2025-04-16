using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerStateBase
{
    private float wallJumpForce = 5f;    // 벽 점프력
    private float stopInputTime = 0.1f;    // 벽 점프 후 플레이어 입력이 제한되는 시간
    // 고양이 상태에서 붙은 벽의 방향
    private int wallDirection = 0;
    // 무한 벽 점프 방지용
    private bool isWallClinging = false;

    public PlayerJumpState(PlayerContextData data, PlayerStateFactory factory) : base(data, factory) { }

    public override void OnUpdate()
    {
        context.Controller.Move();

        if(isWallClinging)
        {
            if ((int)Mathf.Sign(context.Controller.MoveDir.x) != wallDirection
            || context.Controller.MoveDir.x == 0)
            {
                SetGravity();
            }
            else if(context.Controller.JumpKeyPressed)
            {
                SetGravity();
                context.Rigid.AddForce(Vector2.up * context.Controller.JumpForce, ForceMode2D.Impulse);
            }
        }

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
                CatWallCling();
            }
        }
    }

    void CatWallCling()
    {
        Vector2 dir = new Vector2(Mathf.Sign(context.Controller.MoveDir.x), 0);
        Vector2 boxSize = context.BoxCollider.bounds.size;
        boxSize.y *= 0.5f;
        RaycastHit2D hit = Physics2D.BoxCast(context.BoxCollider.bounds.center, boxSize,
            0f, dir, 0.1f, context.Controller.GroundLayer);

        RaycastHit2D checkHit = Physics2D.Raycast(context.Controller.transform.position, dir,
            context.BoxCollider.bounds.size.x + 0.1f, context.Controller.GroundLayer);
        //Debug.DrawRay(context.Controller.transform.position, dir, Color.red);

        if (hit.collider != null && context.Controller.MoveDir.x != 0 && !isWallClinging && checkHit.collider != null)
        {
            context.Rigid.velocity = Vector2.zero;
            context.Rigid.gravityScale = 0f;
            WallRatationSet(dir);
            wallDirection = (int)Mathf.Sign(dir.x);
            context.StateMachine.ReplaceCoroutine(1f, SetGravity);
            isWallClinging = true;
            //context.Rigid.velocity = Vector2.zero;
            //context.Rigid.AddForce(Vector2.up * wallJumpForce, ForceMode2D.Impulse);
        }
    }

    void SetGravity()
    {
        context.Rigid.gravityScale = 1f;
        wallDirection = 0;
        context.Controller.transform.rotation = Quaternion.Euler(Vector3.zero);
        context.StateMachine.ReplaceCoroutine(0.3f, ResetIsWallCling);
    }

    void ResetIsWallCling()
    {
        isWallClinging = false;
    }

    private void WallRatationSet(Vector2 dir)
    {
        Vector3 rot = Vector3.zero;

        if (dir.x > 0)
        {
            rot = new Vector3(0, 0, 90);
        }
        else if (dir.x < 0)
        {
            rot = new Vector3(0, 0, -90);
        }

        context.Controller.transform.rotation = Quaternion.Euler(rot);
    }

    public override void OnExit()
    {
        isWallClinging = false;
    }
}
