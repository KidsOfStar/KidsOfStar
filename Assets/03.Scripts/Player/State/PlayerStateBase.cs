using UnityEngine;

public class PlayerStateBase : IPlayerState
{
    protected PlayerContextData context;
    protected PlayerStateFactory factory;

    public PlayerStateBase(PlayerContextData data, PlayerStateFactory factory)
    {
        this.context = data;
        this.factory = factory;
    }

    public virtual void OnEnter()
    {
        
    }

    public virtual void OnExit()
    {
        
    }

    public virtual void OnUpdate()
    {
        context.CanClingTimer += Time.deltaTime;
        if (!context.Controller.IsControllable) return;

        if(!context.Controller.IsGround)
        {
            if (context.FormController.CurFormData.FormName == "Cat" && context.Controller.MoveDir.x != 0
                && context.CanCling)
            {
                WallTouchCheck();
            }
        }
    }

    protected void WallTouchCheck()
    {
        Vector2 dir = new Vector2(Mathf.Sign(context.Controller.MoveDir.x), 0);

        RaycastHit2D checkHit = Physics2D.Raycast(context.BoxCollider.bounds.center, dir,
            context.BoxCollider.bounds.size.x * 0.75f, context.Controller.GroundLayer);
        Debug.DrawRay(context.BoxCollider.bounds.center, dir * context.BoxCollider.bounds.size.x * 0.75f,
            Color.red, 1f);

        if (checkHit.collider != null)
        {
            if (context.CanClingTimer >= context.Controller.CatClingTimer)
            {
                context.CanClingTimer = 0f;
                context.StateMachine.ChangeState(factory.GetPlayerState(PlayerStateType.WallCling));
            }
        }
    }
}
