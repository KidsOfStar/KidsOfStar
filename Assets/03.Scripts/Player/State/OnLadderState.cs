using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnLadderState : PlayerStateBase
{
    // 플레이어의 본래 중력 값
    private float gravityScale = 0;
    // 프레임 간에 위치값 변경 추적
    private Vector2 previousPosition = Vector2.zero;

    public OnLadderState(PlayerContextData data, PlayerStateFactory factory) : base(data, factory)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        // 원래의 중력 값을 변수에 캐싱
        gravityScale = context.Rigid.gravityScale;
        // 중력 값을 0으로 전환
        context.Rigid.gravityScale = 0f;
        // 본래 있었을지도 모를 가속도 제거
        context.Rigid.velocity = Vector2.zero;
        previousPosition = context.Controller.transform.position;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        // 상하 이동키 입력이 있음
        if(Mathf.Abs(context.Controller.MoveDir.y) > 0.1f)
        {
            if(context.Controller.MoveDir.y > 0)
            { 
                // 플레이어가 땅에 닿은 상태 && 사다리 접촉 X
                if(!context.Controller.IsGround && !context.Controller.TouchLadder)
                {
                    context.StateMachine.ChangeState(factory.GetPlayerState(PlayerStateType.Idle));
                }
            }
            else
            {
                // 아래 방향 키를 입력중이라면
                // 이전 위치 값 유무 체크 && 땅에 닿은 상태
                if(previousPosition != Vector2.zero && context.Controller.IsGround)
                {
                    // 입력은 되고 있지만 위치 값이 변하지 않을 경우
                    if(Mathf.Abs(context.Controller.transform.position.y - previousPosition.y) < 0.02f)
                    {
                        context.StateMachine.ChangeState(factory.GetPlayerState(PlayerStateType.Idle));
                    }
                }

                previousPosition = context.Controller.transform.position;

                // isGround 체크 기준과 같은 기준으로 박스 캐스트
                //RaycastHit2D hit = Physics2D.BoxCast(context.BoxCollider.bounds.center, context.BoxCollider.bounds.size, 0f,
                //Vector2.down, 0.02f, context.Controller.GroundLayer);

                //// 감지된 오브젝트가 있음 && 감지된 땅 오브젝트에 PlatformEffector2D 있음
                //if(hit.collider != null && !hit.collider.TryGetComponent<PlatformEffector2D>(out _))
                //{
                //    // 대기 상태로 전환
                //    context.StateMachine.ChangeState(factory.GetPlayerState(PlayerStateType.Idle));
                //}
            }
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        // 사다리 이동
        context.Controller.ClimbLadder();
    }

    public override void OnExit()
    {
        base.OnExit();
        // 위로 승천(?)하는 혹시 모를 버그 방지용
        context.Rigid.velocity = Vector2.zero;
        // 본래의 중력 값 적용
        context.Rigid.gravityScale = gravityScale;

        // 하강하면서 충돌 처리를 무시하게 된 플랫폼이 있다면
        if(context.IgnoredPlatform != null)
        {
            // 충돌 처리가 되도록 되돌리기
            Physics2D.IgnoreCollision(context.BoxCollider, context.IgnoredPlatform, false);
            // 체크를 위해 변수 비워두기
            context.IgnoredPlatform = null;
        }
    }
}