using System.Collections.Generic;
using UnityEngine;

public class PlayerFormController : MonoBehaviour
{
    [SerializeField, Tooltip("형태변환 데이터 모음집")] private PlayerFormData formData;
    // 형태변화 데이터 딕셔너리
    private Dictionary<string, FormData> formDataDictionary = new Dictionary<string, FormData>();

    private Player playerSc;
    public Player PlayerSc
    {
        set { playerSc = value; }
    }
    private PlayerController controller;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    // 현재변화 상태
    private FormData curFormData;
    public FormData CurFormData { get { return curFormData; } }

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        controller = playerSc.Controller;
        Init();
        curFormData = formDataDictionary["Human"];
    }

    void Init()
    {
        // 형태의 총 갯수
        int count = formData.PlayerFromDataList.Count;

        // 딕셔너리에 옮겨 넣어준다
        for (int i = 0; i < count; i++)
        {
            formDataDictionary.Add(formData.PlayerFromDataList[i].FormName, formData.PlayerFromDataList[i]);
        }
    }

    private void Update()
    {
        // 테스트용 입력
        // 나중에 지울 것
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (curFormData.FormName == "Stone")
            {
                FormChange("Human");
            }
            else if(curFormData.FormName == "Human")
            {
                FormChange("Squirrel");
            }
            else if(curFormData.FormName == "Squirrel")
            {
                FormChange("Dog");
            }
            else if(curFormData.FormName == "Dog")
            {
                FormChange("Cat");
            }
            else if(curFormData.FormName == "Cat")
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
        if (!formDataDictionary[formName].IsActive 
            || !controller.IsGround || !controller.IsControllable) return;

        if(formName == curFormData.FormName)
        {
            curFormData = formDataDictionary["Human"];
        }
        else
        {
            curFormData = formDataDictionary[formName];
        }

        spriteRenderer.sprite = curFormData.FormImage;
        boxCollider.size = new Vector2(curFormData.SizeX, curFormData.SizeY);
        controller.JumpForce = curFormData.JumpForce;
    }

    // 스프라이트 렌더러 플립
    public void FlipControl(Vector2 dir)
    {
        if(dir.x > 0)
        {
            if(curFormData.Direction == DefaultDirection.Right)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }
        }
        else if(dir.x < 0)
        {
            if(curFormData.Direction == DefaultDirection.Right)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }
        }
    }
}