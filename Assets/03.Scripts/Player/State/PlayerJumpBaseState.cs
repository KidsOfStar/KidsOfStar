using UnityEngine;

public class PlayerJumpBaseState : PlayerStateBase
{
    // GroundCheck를 무시하는 잠깐의 시간
    private float groundIgnoreTimer = 0.2f;
    // 점프 이후 경과 시간
    private float elapsedTime = 0f;

    public PlayerJumpBaseState(PlayerContextData data, PlayerStateFactory factory) : base(data, factory)
    {
    }

    public override void OnExit()
    {
        // 초기화
        elapsedTime = 0f;
    }

    public override void OnUpdate()
    {
        // 이동 동작 실행
        context.Controller.Move();
        base.OnUpdate();

        // 점프 이후 경과 시간
        elapsedTime += Time.deltaTime;

        // 땅에 닿은 상태 && 점프 상태 진입 후 정해둔 만큼의 시간이 지남
        if (context.Controller.IsGround && elapsedTime >= groundIgnoreTimer)
        {
            // 이동 키 입력이 없음
            if (context.Controller.MoveDir == Vector2.zero)
            {
                // 대기 상태로 전환
                context.StateMachine.ChangeState(factory.GetPlayerState(PlayerStateType.Idle));
            }
            else
            {
                //이동 키 입력 중이라면
                // 이동 상태로 전환
                context.StateMachine.ChangeState(factory.GetPlayerState(PlayerStateType.Move));
            }
        }
    }
}
