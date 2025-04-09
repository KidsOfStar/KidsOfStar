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
        get { return playerSc; }
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
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        controller = playerSc.Controller;
        Init();
        FormChange(formDataDictionary["Stone"]);
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
                FormChange(formDataDictionary["Human"]);
            }
            else if(curFormData.FormName == "Human")
            {
                FormChange(formDataDictionary["Squirrel"]);
            }
            else if(curFormData.FormName == "Squirrel")
            {
                FormChange(formDataDictionary["Dog"]);
            }
            else if(curFormData.FormName == "Dog")
            {
                FormChange(formDataDictionary["Cat"]);
            }
            else if(curFormData.FormName == "Cat")
            {
                FormChange(formDataDictionary["Hide"]);
            }
            else
            {
                FormChange(formDataDictionary["Stone"]);
            }
        }
    }

    // 형태변화 함수
    // 플레이어 캐릭터의 여러 속성을 변경해준다
    public void FormChange(FormData data)
    {
        if (data == curFormData || !data.IsActive || !controller.IsGround || !controller.IsControllable) return;

        curFormData = data;
        spriteRenderer.sprite = curFormData.FormImage;
        boxCollider.size = new Vector2(curFormData.SizeX, curFormData.SizeY);
        controller.JumpForce = curFormData.JumpForce;
    }
}