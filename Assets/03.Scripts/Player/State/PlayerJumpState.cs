using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerJumpBaseState
{
    public PlayerJumpState(PlayerContextData data, PlayerStateFactory factory) : base(data, factory) { }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if(!context.Controller.IsGround)
        {
            if(context.FormController.CurFormData.FormName == "Cat" && context.Controller.MoveDir.x != 0
                && context.CanCling)
            {
                WallTouchCheck();
            }
        }
    }
}