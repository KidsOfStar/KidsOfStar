using UnityEngine;

public interface IFormState
{
    void OnEnter();
    void OnExit();
    void OnMove(Vector2 moveDir, float speed);
    void OnJump();

    FormData FormData { get; }
}

public class BaseFormState : IFormState
{
    protected PlayerStateContext context;
    protected FormData formData;
    public FormData FormData { get { return formData; } }

    public BaseFormState(PlayerStateContext _context, FormData data)
    {
        context = _context;
        formData = data;
    }

    public void OnEnter()
    {
        context.Controller.CurState?.OnExit();
        context.Renderer.sprite = formData.FormImage;
        context.BoxCollider.size = new Vector2(formData.SizeX, formData.SizeY);
        context.BoxCollider.offset = new Vector2(formData.OffsetX, formData.OffsetY);
        context.Controller.CurState = this;
    }

    public void OnExit()
    {
        
    }

    public virtual void OnJump()
    {
        if (!context.Controller.IsControllable) return;

        if(context.Controller.IsGround)
        {
            context.Rigid.AddForce(Vector2.up * formData.JumpForce, ForceMode2D.Impulse);
        }
    }

    public virtual void OnMove(Vector2 moveDir, float speed)
    {
        if (moveDir != Vector2.up && moveDir != Vector2.down)
        {
            if (context.Controller.TryDetectBox(moveDir))
            {
                IPusher pusher = context.PlayerSc.FormControl;
                float pushPower = pusher.GetPushPower();
                float objWeight = context.Controller.ObjWeight.GetWeight();
                float pushSpeed = (pushPower / objWeight) * speed;
                //미는 속도 = 미는 힘 / 무게 * 이동속도
                pushSpeed = Mathf.Min(pushSpeed, speed);
                // 미는 속도의 최대 이동속도 이상을 초과할 수 없도록

                Vector2 velocity = new Vector2(moveDir.x * pushSpeed, context.Rigid.velocity.y);
                context.Rigid.velocity = velocity;

                context.Controller.ObjRigid.velocity = velocity;
            }
            else
            {
                Vector2 playervelocity = new Vector2(moveDir.x * speed, context.Rigid.velocity.y);
                context.Rigid.velocity = playervelocity;
            }

            FlipControl(moveDir);
        }
    }

    // 스프라이트 렌더러 플립
    public void FlipControl(Vector2 dir)
    {
        if (dir != Vector2.zero)
        {
            if (formData.Direction == DefaultDirection.Right)
            {
                context.Renderer.flipX = dir.x < 0;
            }
            else
            {
                context.Renderer.flipX = dir.x > 0;
            }
        }
    }
}