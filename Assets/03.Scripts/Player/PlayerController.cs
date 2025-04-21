using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("공통")]
    [SerializeField] private LayerMask groundLayer;
    public LayerMask GroundLayer {  get { return groundLayer; }}
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

    private Player player;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private UIJoystick joyStick;

    // 플레이어 이동 방향
    private Vector2 moveDir = Vector2.zero;
    public Vector2 MoveDir { get { return moveDir; } }
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
    // 일반 점프에서 점프키가 눌렸는지를 판단
    private bool jumpKeyPressed = false;
    public bool JumpKeyPressed 
    { 
        get { return jumpKeyPressed; }
        set { jumpKeyPressed = value; }
    }
    // 고양이 벽점프를 위해 키가 눌렸는지를 판단
    private bool wallJumpKeyDown = false;
    public bool WallJumpKeyDown
    {
        get { return wallJumpKeyDown; }
        set { wallJumpKeyDown = value; }
    }

    [Header("Push")]
    [SerializeField, Tooltip("플레이어 앞 박스를 감지할 거리")]
    private float pushDetectDistance = 0.05f;
    [SerializeField, Tooltip("밀 수 있는 박스 레이어")]
    private LayerMask pushableLayer;

    private IWeightable objWeight = null; 
    // Ray로 감지한 물체의 무게
    private Rigidbody2D objRigid = null;
    // Ray로 감지한 물체의 rb

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        UIManager uiManager = Managers.Instance?.UIManager;
        if (uiManager == null) return;

        joyStick = uiManager.Get<UIJoystick>();
    }

    public void Init(Player player)
    {
        this.player = player;
    }

    private void Update()
    {
        GroundCheck();
        Vector2 joystickInput = joyStick != null ? joyStick.Direction : Vector2.zero;

        if(joystickInput != Vector2.zero)
        {
            moveDir = joystickInput;
        }
    }

    void GroundCheck()
    {
        Vector2 boxSize = new Vector2(boxCollider.bounds.size.x * 0.7f, boxCollider.bounds.size.y);
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
        if(context.phase == InputActionPhase.Started)
        {
            Jump();
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            SetWallJumpKeyDown();
        }
    }

    public void OnFormChange(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        switch(context.control.name)
        {
            case "1":
                player.FormControl.FormChange("Squirrel");
                break;
            case "2":
                player.FormControl.FormChange("Dog");
                break;
            case "3":
                player.FormControl.FormChange("Cat");
                break;
            case "4":
                player.FormControl.FormChange("Hide");
                break;
        }
    }

    public void Move()
    {
        if (moveDir != Vector2.up && moveDir != Vector2.down)
        {
            if (TryDetectBox(moveDir))
            {
                IPusher pusher = player.FormControl;
                float pushPower = pusher.GetPushPower();
                float objWeight = this.objWeight.GetWeight();
                float pushSpeed = (pushPower / objWeight) * moveSpeed;
                //미는 속도 = 미는 힘 / 무게 * 이동속도
                pushSpeed = Mathf.Min(pushSpeed, moveSpeed);
                // 미는 속도의 최대 이동속도 이상을 초과할 수 없도록

                Vector2 velocity = new Vector2(moveDir.x * pushSpeed, rigid.velocity.y);
                rigid.velocity = velocity;

                this.objRigid.velocity = velocity;
            }
            else
            {
                Vector2 playervelocity = new Vector2(moveDir.x * moveSpeed, rigid.velocity.y);
                rigid.velocity = playervelocity;
            }

            player.FormControl.FlipControl(moveDir);
        }
    }

    // 점프 버튼에 이 함수도 추가해주세요
    public void SetWallJumpKeyDown()
    {
        if(!jumpKeyPressed && !wallJumpKeyDown
            && player.FormControl.CurFormData.FormName == "Cat" && !isGround)
        {
            wallJumpKeyDown = true;
            return;
        }
    }

    void JumpKeyPressedOff()
    {
        jumpKeyPressed = false;
    }

    public void Jump()
    {
        if (!isControllable) return;

        if (isGround && !jumpKeyPressed)
        {
            jumpKeyPressed = true;
            rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            Invoke("JumpKeyPressedOff", 0.1f);
        }
    }

    public bool TryDetectBox(Vector2 dir)
    {
        float xOffset = boxCollider.bounds.extents.x + 0.01f; 
        Vector2 origin = (Vector2)transform.position + new Vector2(Mathf.Sign(dir.x) * xOffset, 0);
        Vector2 direction = Vector2.right * Mathf.Sign(dir.x);

        // Debug.DrawRay(origin, direction * pushDetectDistance, Color.red, 1f);

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, pushDetectDistance, pushableLayer);
        if(hit.collider != null && hit.collider.TryGetComponent<IWeightable>(out var weight))
        // IWeightable이 붙은 컴포넌트인지 확인하고, 맞으면 True반환과 무게를 반환        
        {
            objWeight = weight;
            objRigid = hit.collider.attachedRigidbody;
            // Collider가 붙어있는 Rigidbody2D를 가져오고
            return true;
        }
        objWeight = null;
        objRigid = null;
        return false;
    }

    //sprite 이미지에 맞춰서 콜라이더 생성
    public void SetCollider()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        Bounds bounds = spriteRenderer.bounds;

        // bounds는 world size이므로, 로컬 좌표로 환산 필요
        Vector2 size = transform.InverseTransformVector(bounds.size);
        Vector2 offset = transform.InverseTransformPoint(bounds.center);

        boxCollider.size = size;
        boxCollider.offset = offset;
    }
}
