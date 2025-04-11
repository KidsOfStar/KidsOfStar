using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

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
        controller.CurState = factory.GetFormState("Human");
        controller.CurState.OnEnter();
        //Init();
        //curFormData = formDataDictionary["Human"];
    }

    //void Init()
    //{
    //    // 형태의 총 갯수
    //    int count = formData.PlayerFromDataList.Count;

    //    // 딕셔너리에 옮겨 넣어준다
    //    for (int i = 0; i < count; i++)
    //    {
    //        formDataDictionary.Add(formData.PlayerFromDataList[i].FormName, formData.PlayerFromDataList[i]);
    //    }
    //}

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
            || !controller.IsGround || !controller.IsControllable) return;

        controller.CurState.OnExit();
        
        if(formName == controller.CurState.FormData.FormName)
        {
            factory.GetFormState("Human").OnEnter();
        }
        else
        {
            nextState.OnEnter();
        }
    }

    // 스프라이트 렌더러 플립
    //public void FlipControl(Vector2 dir)
    //{
    //    if(dir.x > 0)
    //    {
    //        if(curFormData.Direction == DefaultDirection.Right)
    //        {
    //            spriteRenderer.flipX = false;
    //        }
    //        else
    //        {
    //            spriteRenderer.flipX = true;
    //        }
    //    }
    //    else if(dir.x < 0)
    //    {
    //        if(curFormData.Direction == DefaultDirection.Right)
    //        {
    //            spriteRenderer.flipX = true;
    //        }
    //        else
    //        {
    //            spriteRenderer.flipX = false;
    //        }
    //    }
    //}

    public float GetWeight()
    {
        return controller.CurState.FormData.Weight;
    }

    public float GetPushPower()
    {
        return controller.CurState.FormData.Force;
    }
}