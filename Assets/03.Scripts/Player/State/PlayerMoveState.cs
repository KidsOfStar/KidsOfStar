using UnityEngine;

public class PlayerMoveState : PlayerGroundState
{
    public PlayerMoveState(PlayerContextData data, PlayerStateFactory factory) : base(data, factory)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        // 애니메이터의 Move 파라미터를 true로
        context.Controller.Anim.SetBool(PlayerAnimHash.AnimMove, true);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        
        // 땅에 닿은 상태 && 이동 키 입력 없음
        if(context.Controller.IsGround && context.Controller.MoveDir == Vector2.zero)
        {
            // 대기 상태로 전환
            context.StateMachine.ChangeState(factory.GetPlayerState(PlayerStateType.Idle));
        }

        // 이동 동작
        context.Controller.Move();
    }
}
