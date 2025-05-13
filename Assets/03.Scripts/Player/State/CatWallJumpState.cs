using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatWallJumpState : PlayerJumpBaseState
{
    // 벽 점프 상태를 유지하는 시간
    private float timer = 0;

    public CatWallJumpState(PlayerContextData data, PlayerStateFactory factory) : base(data, factory)
    {
    }

    public override void OnEnter()
    {
        float dirX = Mathf.Sign(context.Controller.MoveDir.x);

        context.Controller.transform.GetChild(0).rotation = Quaternion.Euler(Vector3.zero);
        context.Controller.SetCollider();
        context.Rigid.velocity = Vector2.zero;
        Vector2 pushDir = new Vector2(0f, 
            context.Controller.WallJumpForce);
        //context.Rigid.AddForce(Vector2.right * -dirX * 10f, 0);
        //// 벽 점프 실제 동작 실행
        //context.Rigid.AddForce(pushDir, ForceMode2D.Impulse);
        context.StateMachine.RunCoroutine(DelayWallJump(pushDir));
        EditorLog.Log(context.Rigid.velocity.y);
        EditorLog.Log(context.Controller.transform.position.y);

        // 초기화
        timer = 0;
    }

    private IEnumerator DelayWallJump(Vector2 force)
    {
        yield return new WaitForFixedUpdate();
        context.Rigid.AddForce(force, ForceMode2D.Impulse);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        timer += Time.deltaTime;

        // 벽 점프 후 0.5초가 지났다면
        if (timer >= 0.5f)
        {
            // 대기 상태로 전환
            context.StateMachine.ChangeState(factory.GetPlayerState(PlayerStateType.Idle));
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        EditorLog.Log(context.Rigid.velocity.y);
        EditorLog.Log(context.Controller.transform.position.y);
    }

    public override void OnExit()
    {
        base.OnExit();
        // 벽 타기 잠금 해제
        context.CanCling = true;
    }
}
