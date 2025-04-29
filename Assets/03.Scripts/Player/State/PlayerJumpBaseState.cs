using UnityEngine;

public class PlayerJumpBaseState : PlayerStateBase
{
    private float groundIgnoreTimer = 0.2f;
    private float elapsedTime = 0f;

    public PlayerJumpBaseState(PlayerContextData data, PlayerStateFactory factory) : base(data, factory)
    {
    }

    public override void OnExit()
    {
        elapsedTime = 0f;
    }

    public override void OnUpdate()
    {
        context.Controller.Move();
        base.OnUpdate();

        elapsedTime += Time.deltaTime;

        if (context.Controller.IsGround && elapsedTime >= groundIgnoreTimer)
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
}
