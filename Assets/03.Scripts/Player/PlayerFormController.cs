using System.Collections.Generic;
using UnityEngine;

public interface IWeightable
{
    float GetWeight();
}

public interface IPusher
{
    float GetPushPower();
}

public class PlayerFormController : MonoBehaviour, IWeightable, IPusher
{
    [SerializeField, Tooltip("형태변환 데이터 모음집")] private PlayerFormData formData;

    private Player playerSc;
    public Player PlayerSc
    {
        set { playerSc = value; }
    }
    private PlayerController controller;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private PlayerStateContext stateContext;
    private PlayerFormStateFactory factory;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        controller = playerSc.Controller;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        stateContext = new PlayerStateContext(playerSc, controller, this, spriteRenderer, rb, boxCollider);
        factory = new PlayerFormStateFactory(stateContext, formData);
        FormChange("Human");
    }

    private void Update()
    {
        // 테스트용 입력
        // 나중에 지울 것
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (controller.CurState is StoneFormState)
            {
                FormChange("Human");
            }
            else if (controller.CurState is HumanFormState)
            {
                FormChange("Squirrel");
            }
            else if (controller.CurState is SquirrelFormState)
            {
                FormChange("Dog");
            }
            else if (controller.CurState is DogFormState)
            {
                FormChange("Cat");
            }
            else if (controller.CurState is CatFormState)
            {
                FormChange("Hide");
            }
            else
            {
                FormChange("Stone");
            }
        }
    }

    // 형태변화 함수
    // 플레이어 캐릭터의 여러 속성을 변경해준다
    public void FormChange(string formName)
    {
        IFormState nextState = factory.GetFormState(formName);

        if (nextState == null || !nextState.FormData.IsActive 
            || (controller.CurState != null && !controller.IsGround) || !controller.IsControllable) return;

        controller.CurState?.OnExit();
        
        if(formName == controller.CurState?.FormData.FormName)
        {
            factory.GetFormState("Human").OnEnter();
        }
        else
        {
            nextState.OnEnter();
        }
    }

    public Dictionary<string, bool> GetFormLock()
    {
        Dictionary<string, bool> activeDic =
            new Dictionary<string, bool>()
            {
                {"Squirrel", factory.GetFormState("Squirrel").FormData.IsActive },
                {"Dog", factory.GetFormState("Dog").FormData.IsActive },
                {"Cat", factory.GetFormState("Cat").FormData.IsActive },
                {"Hide", factory.GetFormState("Hide").FormData.IsActive },
            };

        return activeDic;
    }

    public float GetWeight()
    {
        return controller.CurState.FormData.Weight;
    }

    public float GetPushPower()
    {
        return controller.CurState.FormData.Force;
    }
}