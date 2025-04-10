using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Player playerSc;
    public Player PlayerSc
    {
        set { playerSc = value; }
    }
    private Rigidbody2D rigid;
    private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask groundLayer;

    // 플레이어 이동 방향
    private Vector2 moveDir = Vector2.zero;
    // 플레이어 캐릭터 이동 속도
    [SerializeField] private float moveSpeed;
    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }
    // 플레이어 캐릭터 점프력
    [SerializeField] private float jumpForce;
    public float JumpForce
    {
        get { return jumpForce; }
        set { jumpForce = value; }
    }
    // 벽 점프력
    [SerializeField] private float wallJumpForce;
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
    // 벽 점프 중이라면 true
    private bool wallJumping = false;

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
            playerSc.FormControl.FlipControl(moveDir);
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
                StartCoroutine(WallJump());
            }
        }
    }

    IEnumerator WallJump()
    {
        yield return new WaitForSeconds(0.1f);

        wallJumping = false;
    }
}
