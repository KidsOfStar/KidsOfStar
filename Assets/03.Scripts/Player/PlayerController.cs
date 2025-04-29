using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour,IWeightable, ILeafJumpable
{
    [Header("공통")]
    [SerializeField] private LayerMask groundLayer;
    public LayerMask GroundLayer { get { return groundLayer; } }
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator anim;
    public Animator Anim { get { return anim; } }
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

    [Space, Header("고양이")]
    [SerializeField, Tooltip("벽 점프력")] private float wallJumpForce = 5f;
    public float WallJumpForce { get { return wallJumpForce; } }
    [SerializeField, Tooltip("벽에 붙었다가 떨어졌을 때 다시 붙을 수 있게 되기까지의 쿨타임")] private float catClingTimer = 1f;
    public float CatClingTimer { get { return catClingTimer; } }

    private Player player;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rigid;

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
    public bool IsGround { get { return isGround; } }
    // 플레이어 캐릭터 조작 가능하면 true
    private bool isControllable = true;
    public bool IsControllable
    {
        get { return isControllable; }
        set { isControllable = value; }
    }
    // 추격 모드면 ture, 평소에는 false
    private bool isChaseMode = false;
    public bool IsChaseMode
    {
        get { return isChaseMode; }
        set { isChaseMode = value; }
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

    [Tooltip("Inspector에서 설정할 나뭇잎 힘")]
    public float leafJumpPower;
    private float basePushPower;
    private bool isLeafJumping = false;
    public void Init(Player player)
    {
        this.player = player;
        rigid = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        EditorLog.Log("매니저 이벤트에 lockPlayer 등록");
        Managers.Instance.CutSceneManager.OnCutSceneStart += LockPlayer;
        Managers.Instance.DialogueManager.OnDialogStart += LockPlayer;
        Managers.Instance.CutSceneManager.OnCutSceneEnd += UnlockPlayer;
        Managers.Instance.DialogueManager.OnDialogEnd += UnlockPlayer;
    }

    private void Update()
    {
        GroundCheck();
    }

    void GroundCheck()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down,
            0.02f, groundLayer);
        
        if(hit.collider != null && hit.normal.y > 0.7f)
        {
            isGround = true;
            isLeafJumping = false;
        }
        else
        {
            isGround = false;
        }

        anim.SetBool(PlayerAnimHash.AnimGround, isGround);
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed || context.phase == InputActionPhase.Started)
        {
            moveDir = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            moveDir = Vector2.zero;
        }
    }
    
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Jump();
        }
    }

    public void OnFormChange(InputAction.CallbackContext context)
    {
        if (!context.performed || !isGround || !isControllable) return;
        var skillBtn = Managers.Instance.UIManager.Get<PlayerBtn>().skillPanel;
        switch (context.control.name)
        {
            case "1":
                //player.FormControl.FormChange("Squirrel");
                skillBtn.OnSquirrel();
                break;
            case "2":
                //player.FormControl.FormChange("Dog");
                skillBtn.OnDog();
                break;
            case "3":
                //player.FormControl.FormChange("Cat");
                skillBtn.OnCat();
                break;
            case "4":
                //player.FormControl.FormChange("Hide");
                skillBtn.OnHide();
                break;
        }
    }

    public void Move()
    {
        if (!isControllable) return;

        if (moveDir != Vector2.up && moveDir != Vector2.down && !isLeafJumping)
        {
            if (TryDetectBox(moveDir))
            {
                IPusher pusher = player.FormControl;
                float pushPower = pusher.GetPushPower();
                float objWeight = this.objWeight.GetWeight();
                basePushPower = (pushPower / objWeight) * moveSpeed;
                //미는 속도 = 미는 힘 / 무게 * 이동속도
                basePushPower = Mathf.Min(basePushPower, moveSpeed);
                // 미는 속도의 최대 이동속도 이상을 초과할 수 없도록

                rigid.velocity = new Vector2(moveDir.x * basePushPower, rigid.velocity.y);
            }
            else
            {
                rigid.velocity = new Vector2(moveDir.x * moveSpeed, rigid.velocity.y);
            }
            player.FormControl.FlipControl(moveDir);
        }
    }

    public void Jump()
    {
        isLeafJumping = true;
        if (!isControllable) return;

        if (isGround)
        {
            player.StateMachine.ChangeState(player.StateMachine.Factory.GetPlayerState(PlayerStateType.Jump));
        }
        else if(!player.StateMachine.ContextData.CanCling)
        {
            player.StateMachine.ChangeState(player.StateMachine.Factory.GetPlayerState(PlayerStateType.WallJump));
        }
    }

    public float GetWeight()
    {
        return isLeafJumping
            ? 0f
            : Managers.Instance.GameManager.Player.FormControl.GetWeight();
    }

    public void StartLeafJump(Vector2 dropPosition,float jumpPower)
    {
        isLeafJumping = true;

        rigid.velocity = Vector2.zero;
        rigid.gravityScale = 1f;

        Vector2 dir = (dropPosition - rigid.position).normalized;
        Vector2 impulse = dir * jumpPower * rigid.mass;
        rigid.AddForce(impulse, ForceMode2D.Impulse);
    }

    public bool TryDetectBox(Vector2 dir)
    {
        float xOffset = boxCollider.bounds.extents.x + 0.01f;
        Vector2 origin = (Vector2)transform.position + new Vector2(Mathf.Sign(dir.x) * xOffset, 0.1f);
        Vector2 direction = Vector2.right * Mathf.Sign(dir.x);

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, pushDetectDistance, pushableLayer);

        if (hit.collider != null)
        {
            if (hit.collider != null && hit.collider.TryGetComponent<IWeightable>(out var weight))
            // IWeightable이 붙은 컴포넌트인지 확인하고, 맞으면 True반환과 무게를 반환        
            {
                objWeight = weight;
                objRigid = hit.collider.attachedRigidbody;
                // Collider가 붙어있는 Rigidbody2D를 가져오고
                return true;
            }
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

    public void LockPlayer()
    {
        isControllable = false;
        rigid.velocity = Vector2.zero;
    }
    
    private void UnlockPlayer()
    {
        isControllable = true;
    }

    private void OnDestroy()
    {
        Managers.Instance.CutSceneManager.OnCutSceneStart -= LockPlayer;
        Managers.Instance.CutSceneManager.OnCutSceneEnd -= UnlockPlayer;
        
        Managers.Instance.DialogueManager.OnDialogStart -= LockPlayer;
        Managers.Instance.DialogueManager.OnDialogEnd -= UnlockPlayer;
    }

}
