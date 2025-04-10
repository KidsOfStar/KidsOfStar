using UnityEngine;
using UnityEngine.UI;

public class PlayerBtn : UIBase
{
    [Header("UI SkillBtn")]
    public Button jumpBtn;          // 점프 버튼
    public Button hideBtn;          // 숨기기 버튼
    public Button catBtn;           // 고양이 버튼
    public Button dogBtn;           // 개 버튼
    public Button squirrelBtn;      // 다람쥐 버튼

    [Header("UI FunctionBtn")]
    public Button stopBtn;          // 정지 버튼
    public Button skipBtn;          // 스킵 버튼
    public Button interactionBtn;   // 상호작용 버튼

    [Header("UI Object")]
    public Image jumpbg;       // 점프 배경
    public GameObject jumpbg1;       // 점프 배경

    public Image hidebg;       // 숨기기 배경
    public Image catbg;        // 고양이 배경
    public Image dogbg;        // 개 배경
    public Image squirrelbg;   // 다람쥐 배경


    private void Start()
    {
        // 스킬 버튼
        jumpBtn.onClick.AddListener(OnJump);
        hideBtn.onClick.AddListener(OnHide);
        catBtn.onClick.AddListener(OnCat);
        dogBtn.onClick.AddListener(OnDog);
        squirrelBtn.onClick.AddListener(OnSquirrel);
        // 기능 버튼
        stopBtn.onClick.AddListener(OnStop);
        skipBtn.onClick.AddListener(OnSkip);
        interactionBtn.onClick.AddListener(OnInteraction);
    }

    public void OnJump()
    {
        //jumpbg.enabled = true; // 점프 배경 활성화
        jumpbg1.gameObject.SetActive(true); // 점프 배경 활성화
        // 점프 메소드 호출
        //Managers.Instance.GameManager.Player.PlayerController.OnJump();
        //jumpbg.enabled = false; // 점프 배경 비활성화
        jumpbg1.gameObject.SetActive(false); // 점프 배경 활성화

    }

    public void OnHide()
    {
        // 형태변화 스크립트에서 호출
        //Managers.Instance.GameManager.Player.PlayerFormController.FormChange("Hide");
    }
    public void OnCat()
    {
        // 형태변화 스크립트에서 호출
        //Managers.Instance.GameManager.Player.PlayerFormController.FormChange("Cat");
    }
    public void OnDog()
    {
        // 형태변화 스크립트에서 호출
        //Managers.Instance.GameManager.Player.PlayerFormController.FormChange("Dog");

    }
    public void OnSquirrel()
    {
        // 형태변화 스크립트에서 호출
        //Managers.Instance.GameManager.Player.PlayerFormController.FormChange("Squirrel");

    }
    public void OnStop()
    {
        Time.timeScale = 0; // 게임 일시 정지
        Managers.Instance.UIManager.Show<OptionPopup>(); // 옵션 팝업 열기
    }
    public void OnSkip()
    {
        // 씬 스킵 메소드 호출
    }
    public void OnInteraction()
    {
        // NPC와 상호작용 메소드 호출

        Managers.Instance.DialogueManager.OnClick?.Invoke();
    }

}
