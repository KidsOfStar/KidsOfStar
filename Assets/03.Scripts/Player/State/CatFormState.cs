using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatFormState : BaseFormState
{
    private float wallJumpForce;    // 벽 점프력
    private float stopInputTime = 0.1f;    // 벽 점프 후 플레이어 입력이 제한되는 시간
    [SerializeField, Tooltip("벽 점프 후 캐릭터의 각도 변경이 유지되는 시간")] private float keepRotationTime = 0.5f;
    // 벽 점프 중에 일시적으로 플레이어 조작 잠금
    private bool wallJumping = false;
    // 벽 점프 각도 변경 유지 시간인지 체크
    private bool keepRotation = false;

    public CatFormState(PlayerStateContext _context, FormData data) : base(_context, data) { }

    public override void OnMove(Vector2 moveDir, float speed)
    {
        if (!wallJumping)
        {
            base.OnMove(moveDir, speed);
        }
    }

    public override void OnJump()
    {
        base.OnJump();

        if (!context.Controller.IsGround && !wallJumping)
        {
            Vector2 dir = new Vector2(context.Rigid.velocity.normalized.x, 0);
            RaycastHit2D hit = Physics2D.BoxCast(context.BoxCollider.bounds.center, context.BoxCollider.bounds.size, 
                0f, dir, 0.02f, context.Controller.GroundLayer);

            if (hit.collider != null)
            {
                context.Rigid.velocity = Vector2.zero;
                context.Rigid.AddForce(Vector2.up * wallJumpForce, ForceMode2D.Impulse);
                wallJumping = true;
                keepRotation = true;
                //WallRatationSet(dir);
                //playerSc.FormControl.FlipControl(dir);
                //StartCoroutine(WallJump());
            }
        }
    }

    IEnumerator WallJump()
    {
        yield return new WaitForSeconds(stopInputTime);

        wallJumping = false;
        //StartCoroutine(ReturnRotation());
    }

    IEnumerator ReturnRotation()
    {
        yield return new WaitForSeconds(keepRotationTime);

        keepRotation = false;
        //transform.rotation = Quaternion.Euler(Vector3.zero);
    }
}
