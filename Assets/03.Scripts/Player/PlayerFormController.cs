using System.Collections.Generic;
using UnityEngine;

public class PlayerFormController : MonoBehaviour
{
    [SerializeField, Tooltip("형태변환 데이터 모음집")] private PlayerFormData formData;
    // 형태변환 데이터 딕셔너리
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

    // 현재 변환 상태
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
        StateChange(formDataDictionary["Stone"]);
    }

    void Init()
    {
        int count = formData.PlayerFromDataList.Count;

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
                StateChange(formDataDictionary["Human"]);
            }
            else
            {
                StateChange(formDataDictionary["Stone"]);
            }
        }
    }

    public void StateChange(FormData data)
    {
        if (data == curFormData || !data.IsActive) return;

        curFormData = data;
        spriteRenderer.sprite = curFormData.FormImage;
        boxCollider.size = new Vector2(curFormData.SizeX, curFormData.SizeY);
        controller.JumpForce = curFormData.JumpForce;
    }
}
