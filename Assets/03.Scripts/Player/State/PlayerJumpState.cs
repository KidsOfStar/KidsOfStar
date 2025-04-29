using UnityEngine;

public class PlayerJumpState : PlayerJumpBaseState
{
    public PlayerJumpState(PlayerContextData data, PlayerStateFactory factory) : base(data, factory) { }

    public override void OnEnter()
    {
        context.Controller.Anim.SetTrigger(PlayerAnimHash.AnimJump);
        context.Rigid.AddForce(Vector2.up * context.Controller.JumpForce, ForceMode2D.Impulse);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}