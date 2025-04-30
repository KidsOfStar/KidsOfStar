using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour,IWeightable, ILeafJumpable
{
    [Header("공통")]
    [SerializeField, Tooltip("플레이어가 땅으로 인식할(+점프 가능한) 레이어")] private LayerMask groundLayer;
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
    // 현재 입력중인 이동 방향
    private Vector2 moveDir = Vector2.zero;
    public Vector2 MoveDir { get { return moveDir; } }
    // 플레이어 캐릭터가 사다리에 닿았는지 여부
    // 플레이어 캐릭터가 사다리에 닿은 상태일 때 true
    private bool touchLadder = false;
    public bool TouchLadder
    {
        get { return touchLadder; }
        set { touchLadder = value; }
    }
    // 플레이어 캐릭터가 땅에 닿았는지 여부
    // 플레이어 캐릭터가 땅에 닿으면 true
    private bool isGround;
    public bool IsGround { get { return isGround; } }
    // 플레이어 캐릭터 조작 가능 여부
    // 플레이어 캐릭터 조작 가능하면 true
    private bool isControllable = true;
    public bool IsControllable
    {
        get { return isControllable; }
        set { isControllable = value; }
    }
    // 추격전 모드 여부
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

    /// <summary>
    /// 스크립트 초기화 함수
    /// </summary>
    /// <param name="player">Player 스크립트</param>
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

    /// <summary>
    /// 현재 땅(혹은 점프 가능한 오브젝트)에 닿은 상태인지 체크
    /// </summary>
    void GroundCheck()
    {
        // 박스 캐스트로 플레이어의 아래 방향을 체크
        // 레이 캐스트가 아닌 이유는
        // 플레이어가 플랫폼 끝자락에 위치할 때 제대로 체크되지 않는 경우를 방지하기 위함
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down,
            0.02f, groundLayer);
        
        // 점프 가능한 오브젝트에 닿은 동시에
        // 그 위치가 플레이어 기준으로 아래 방향이라면
        if(hit.collider != null && hit.normal.y > 0.7f)
        {
            //  isGround를 true로
            isGround = true;
        }
        else
        {
            // 위의 조건이 충족 상태가 아니라면 false로
            isGround = false;
        }

        // 애니메이터의 Ground 파라미터의 값을 isGround에 따라서 수정
        anim.SetBool(PlayerAnimHash.AnimGround, isGround);
    }

    /// <summary>
    /// 플레이어 이동 조작을 위한 입력 함수
    /// </summary>
    /// <param name="context">입력 값</param>
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        // 입력을 시작했거나 입력 중이라면
        if (context.phase == InputActionPhase.Performed || context.phase == InputActionPhase.Started)
        {
            // 입력된 값을 moveDir에 적용
            moveDir = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            // 입력이 없다면 곧바로 이동을 정지
            moveDir = Vector2.zero;
        }
    }
    
    /// <summary>
    /// 점프 키 입력 함수
    /// 키보드 입력 담당
    /// </summary>
    /// <param name="context">입력 값</param>
    public void OnJump(InputAction.CallbackContext context)
    {
        // 점프 키 입력이 있다면(짧게라도)
        if (context.phase == InputActionPhase.Started)
        {
            // 실제 점프 동작 함수 호출
            Jump();
        }
    }

    /// <summary>
    /// 형태 변화 키 입력 함수
    /// 키보드 입력 담당
    /// </summary>
    /// <param name="context"></param>
    public void OnFormChange(InputAction.CallbackContext context)
    {
        // 충분한 입력이 없었거나(performed를 체크함으로...)
        // 땅에 닿은 상태가 아니거나
        // 조작이 불가능한 상태인 경우에는 return
        if (!context.performed || !isGround || !isControllable) return;
        var skillBtn = Managers.Instance.UIManager.Get<PlayerBtn>().skillPanel;
        switch (context.control.name)
        {
            case "1":
                skillBtn.OnSquirrel();
                break;
            case "2":
                skillBtn.OnDog();
                break;
            case "3":
                skillBtn.OnCat();
                break;
            case "4":
                skillBtn.OnHide();
                break;
        }
    }

    /// <summary>
    /// 실제 플레이어 이동 동작 함수
    /// </summary>
    public void Move()
    {
        // 조작이 불가능한 상태라면 동작 X
        if (!isControllable) return;

        if (moveDir != Vector2.up && moveDir != Vector2.down)
        // 입력된 값이 위나 아래를 향한 것이 아니고
        // 나뭇잎 트램펄린을 이용하는 중이 아니라면
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
                // 상자를 미는 상태가 아니라면 이동속도와 입력 방향에 맞춰 이동
                rigid.velocity = new Vector2(moveDir.x * moveSpeed, rigid.velocity.y);
            }

            // FlipControl 함수에 플레이어 이동 방향을 전달
            player.FormControl.FlipControl(moveDir);
        }
    }

    /// <summary>
    /// 실제 플레이어의 점프 동작 함수
    /// </summary>
    public void Jump()
    {
        // 동작 불가 상태라면 return
        if (!isControllable) return;

        // 점프 가능한 레이어의 오브젝트에 발(오브젝트의 아래 방향)이 닿은 상태라면
        if (isGround)
        {
            // 플레이어의 상태를 점프 상태로 전환
            player.StateMachine.ChangeState(player.StateMachine.Factory.GetPlayerState(PlayerStateType.Jump));
        }
        else if(!player.StateMachine.ContextData.CanCling)
        {
            // 플레이어가 고양이 형태로 벽에 붙은 상태라면 벽 점프 상태로 전환
            player.StateMachine.ChangeState(player.StateMachine.Factory.GetPlayerState(PlayerStateType.WallJump));
        }
    }

    public float GetWeight()
    {
        return Managers.Instance.GameManager.Player.FormControl.GetWeight();
    }

    public void StartLeafJump(Vector2 dropPosition,float jumpPower)
    {
        rigid.velocity = Vector2.zero;
        rigid.gravityScale = 1f;

        Vector2 impulse = dropPosition * jumpPower * rigid.mass;
        rigid.AddForce(impulse, ForceMode2D.Impulse);
    }

    public bool TryDetectBox(Vector2 dir)
    {

        Vector2 origin = (Vector2)boxCollider.bounds.center
        + Vector2.right * Mathf.Sign(dir.x) * (boxCollider.bounds.extents.x + 0.01f);

        Vector2 size = new Vector2(0.05f, boxCollider.bounds.size.y * 0.9f);
        Vector2 dirVec = Vector2.right * Mathf.Sign(dir.x);

        RaycastHit2D hit = Physics2D.BoxCast(
        origin, size, 0f,
        dirVec,
        pushDetectDistance,
        pushableLayer
    );

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

    /// <summary>
    /// 플레이어 조작 잠금 함수
    /// </summary>
    public void LockPlayer()
    {
        // 걸으면서 대화를 할 수 없도록 하기 위해서 Idle 상태로 변경
        player.StateMachine.ChangeState(player.StateMachine.Factory.GetPlayerState(PlayerStateType.Idle)); // Idle 상태로 변경
        isControllable = false;
        rigid.velocity = Vector2.zero;
    }
    
    /// <summary>
    /// 플레이어 조작 잠금 해제 함수
    /// </summary>
    public void UnlockPlayer()
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