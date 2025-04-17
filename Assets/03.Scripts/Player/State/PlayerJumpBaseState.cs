using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpBaseState : PlayerStateBase
{
    public PlayerJumpBaseState(PlayerContextData data, PlayerStateFactory factory) : base(data, factory)
    {
    }

    public override void OnUpdate()
    {
        context.Controller.Move();
        base.OnUpdate();
    }

    public override void OnExit()
    {
        context.CanCling = true;
    }
}
