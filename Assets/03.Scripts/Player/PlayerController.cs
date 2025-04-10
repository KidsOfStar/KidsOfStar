using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("공통")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField, Tooltip("플레이어 캐릭터 이동 속도")] private float moveSpeed;
    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }
    [SerializeField, Tooltip("플레이어 캐릭터 점프력")] private float jumpForce;
    public float JumpForce
    {
        get { return jumpForce; }
        set { jumpForce = value; }
    }

    [Space, Header("고양이 벽 점프")]
    [SerializeField, Tooltip("벽 점프력")] private float wallJumpForce;
    [SerializeField, Tooltip("벽 점프 후 플레이어 입력이 제한되는 시간")] private float stopInputTime = 0.1f;
    [SerializeField, Tooltip("벽 점프 후 캐릭터의 각도 변경이 유지되는 시간")] private float keepRotationTime = 0.5f;
    // 벽 점프 중에 일시적으로 플레이어 조작 잠금
    private bool wallJumping = false;
    // 벽 점프 각도 변경 유지 시간인지 체크
    private bool keepRotation = false;
    

    private Player playerSc;
    public Player PlayerSc
    {
        set { playerSc = value; }
    }
    private Rigidbody2D rigid;
    private BoxCollider2D boxCollider;

    // 플레이어 이동 방향
    private Vector2 moveDir = Vector2.zero;
    // 플레이어 캐릭터가 사다리에 닿은 상태일 때 true
    private bool touchLadder = false;
    public bool TouchLadder
    {
        get { return touchLadder; }
        set { touchLadder = value; }
    }
    // 플레이어 캐릭터가 땅에 닿으면 true
    private bool isGround;
    public bool IsGround { get  { return isGround; } }
    // 플레이어 캐릭터 조작 가능하면 true
    private bool isControllable = true;
    public bool IsControllable
    {
        get { return isControllable; }
        set { isControllable = value; }
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        GroundCheck();
        Move();
    }

    void Move()
    {
        if (!wallJumping)
        {
            if (moveDir != Vector2.up && moveDir != Vector2.down)
            {
                Vector2 playervelocity = new Vector2(moveDir.x * moveSpeed, rigid.velocity.y);
                rigid.velocity = playervelocity;
                playerSc.FormControl.FlipControl(moveDir);
                
                if (keepRotation)
                {
                    WallRatationSet(rigid.velocity);
                }
            }
        }
    }

    void GroundCheck()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down,
            0.02f, groundLayer);
        isGround = hit.collider != null ? true : false;
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (!isControllable) return;

        if (context.phase == InputActionPhase.Performed)
        {
            moveDir = context.ReadValue<Vector2>();
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            moveDir = Vector2.zero;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Canceled && isGround)
        {
            rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if(!isGround && playerSc.FormControl.CurFormData.FormName == "Cat"
            && !wallJumping)
        {
            Vector2 dir = new Vector2(moveDir.normalized.x, 0);
            RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, dir, 0.02f,
                groundLayer);
            
            if(hit.collider != null)
            {
                dir *= -wallJumpForce;
                dir.y = jumpForce;
                rigid.velocity = Vector2.zero;
                rigid.AddForce(dir, ForceMode2D.Impulse);
                wallJumping = true;
                keepRotation = true;
                WallRatationSet(dir);
                playerSc.FormControl.FlipControl(dir);
                StartCoroutine(WallJump());
            }
        }
    }

    private void WallRatationSet(Vector2 dir)
    {
        Vector3 rot = dir.x > 0 ? new Vector3(0, 0, 90) : new Vector3(0, 0, -90);
        transform.rotation = Quaternion.Euler(rot);

    }

    IEnumerator WallJump()
    {
        yield return new WaitForSeconds(stopInputTime);

        wallJumping = false;
        StartCoroutine(ReturnRotation());
    }

    IEnumerator ReturnRotation()
    {
        yield return new WaitForSeconds(keepRotationTime);

        keepRotation = false;
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }
}
