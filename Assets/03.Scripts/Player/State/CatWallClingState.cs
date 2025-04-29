using UnityEngine;

public class CatWallClingState : PlayerStateBase
{
    float clingTimer = 0;
    float limit = 1f;

    public CatWallClingState(PlayerContextData data, PlayerStateFactory factory) : base(data, factory)
    {
    }

    public override void OnEnter()
    {
        context.Rigid.velocity = Vector2.zero;
        WallRatationSet();
        context.CanCling = false;
        clingTimer = 0;
    }

    private void WallRatationSet()
    {
        float dirx = Mathf.Sign(context.Controller.MoveDir.x);
        Vector3 rot = Vector3.zero;

        if (dirx > 0)
        {
            rot = new Vector3(0, 0, 90);
        }
        else if (dirx < 0)
        {
            rot = new Vector3(0, 0, -90);
        }

        context.Controller.transform.GetChild(0).rotation = Quaternion.Euler(rot);
        context.Controller.SetCollider();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        context.Controller.Move();
        context.Rigid.velocity = new Vector2(context.Rigid.velocity.x, 0);
        clingTimer += Time.deltaTime;

        if(clingTimer >= limit)
        {
            context.StateMachine.ChangeState(factory.GetPlayerState(PlayerStateType.Idle));
            return;
        }

        if(context.Controller.IsGround)
        {
            context.StateMachine.ChangeState(factory.GetPlayerState(PlayerStateType.Idle));
            return;
        }

        if(context.Controller.MoveDir.x == 0 || !IsWallTouchCheck())
        {
            context.StateMachine.ChangeState(factory.GetPlayerState(PlayerStateType.Idle));
            return;
        }
    }

    bool IsWallTouchCheck()
    {
        Vector2 dir = new Vector2(Mathf.Sign(context.Controller.MoveDir.x), 0);
        RaycastHit2D hit = Physics2D.Raycast(context.BoxCollider.bounds.center, dir,
            context.BoxCollider.bounds.size.x * 1.5f, context.Controller.GroundLayer);
        Debug.DrawRay(context.BoxCollider.bounds.center, dir * context.BoxCollider.bounds.size.x * 1.5f,
            Color.green, 1f);

        if (hit.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void OnExit()
    {
        context.Controller.transform.GetChild(0).rotation = Quaternion.Euler(Vector3.zero);
        context.Controller.SetCollider();
    }
}