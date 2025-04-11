using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 플레이어 관련 UI를 제어하는 클래스
public class PlayerBtn : UIBase
{
    [Header("UI SkillBtn")] // 스킬 버튼들
    public Button jumpBtn;
    public Button hideBtn;
    public Button catBtn;
    public Button dogBtn;
    public Button squirrelBtn;

    [Header("UI FunctionBtn")] // 게임 기능 버튼들
    public Button stopBtn;
    public Button skipBtn;
    public Button interactionBtn;

    [Header("UI Object")] // 각 스킬에 대한 배경 오브젝트
    public GameObject jumpBG;
    public GameObject hideBG;
    public GameObject catBG;
    public GameObject dogBG;
    public GameObject squirrelBG;

    [Header("UI HideSkill")] // 스킬 UI 오브젝트들 (버튼 아래 표시용)
    public GameObject hideSkill;
    public GameObject catSkill;
    public GameObject dogSkill;
    public GameObject squirrelSkill;

    private List<GameObject> skillBGs; // 스킬 배경 오브젝트 리스트

    private void Start()
    {
        // 각 버튼에 클릭 이벤트 연결
        jumpBtn.onClick.AddListener(OnJump);
        hideBtn.onClick.AddListener(OnHide);
        catBtn.onClick.AddListener(OnCat);
        dogBtn.onClick.AddListener(OnDog);
        squirrelBtn.onClick.AddListener(OnSquirrel);

        stopBtn.onClick.AddListener(OnStop);
        skipBtn.onClick.AddListener(OnSkip);
        interactionBtn.onClick.AddListener(OnInteraction);

        // 스킬 배경 리스트 초기화
        skillBGs = new List<GameObject> { hideBG, catBG, dogBG, squirrelBG };
    }

    private void Update()
    {
        // 플레이어가 땅에 있는지 확인하여 스킬 UI 표시 여부 결정
        if (Managers.Instance.GameManager.Player.Controller.IsGround)
        {
            ToggleSkillUI(true);  // 땅에 닿았으면 스킬 UI 활성화
        }
        else
        {
            ToggleSkillUI(false); // 공중에 있으면 스킬 UI 비활성화
        }
    }

    // 점프 버튼 클릭 시 호출
    public void OnJump()
    {
        ToggleSkillUI(false); // 점프 시 스킬 UI 숨기기
        OnBlink(jumpBG, 0.1f); // 배경 깜빡임 효과

        // 공중일 경우 점프 불가
        if (!Managers.Instance.GameManager.Player.Controller.IsGround)
            return;

        // 실제 점프 실행
        Managers.Instance.GameManager.Player.Controller.Jump();
    }

    // 숨기기 스킬 버튼 클릭 시 호출
    public void OnHide()
    {
        Managers.Instance.GameManager.Player.FormControl.FormChange("Hide");

        // 현재 배경이 켜져 있으면 끄고, 아니면 보여주기
        if (hideBG.activeSelf)
        {
            hideBG.SetActive(false);
        }
        else
        {
            ShowSkillBG(hideBG);
        }
    }

    // 고양이 스킬 버튼 클릭 시 호출
    public void OnCat()
    {
        Managers.Instance.GameManager.Player.FormControl.FormChange("Cat");

        if (catBG.activeSelf)
        {
            catBG.SetActive(false);
        }
        else
        {
            ShowSkillBG(catBG);
        }
    }

    // 개 스킬 버튼 클릭 시 호출
    public void OnDog()
    {
        Managers.Instance.GameManager.Player.FormControl.FormChange("Dog");

        if (dogBG.activeSelf)
        {
            dogBG.SetActive(false);
        }
        else
        {
            ShowSkillBG(dogBG);
        }
    }

    // 다람쥐 스킬 버튼 클릭 시 호출
    public void OnSquirrel()
    {
        Managers.Instance.GameManager.Player.FormControl.FormChange("Squirrel");

        if (squirrelBG.activeSelf)
        {
            squirrelBG.SetActive(false);
        }
        else
        {
            ShowSkillBG(squirrelBG);
        }
    }

    // UI 깜빡임 효과 코루틴
    private IEnumerator BlinkEffect(GameObject obj, float duration)
    {
        obj.SetActive(true);
        yield return new WaitForSeconds(duration);
        obj.SetActive(false);
    }

    // 깜빡임 효과 실행 함수
    public void OnBlink(GameObject obj, float duration)
    {
        StartCoroutine(BlinkEffect(obj, duration));
    }

    // 정지 버튼 클릭 시 게임 일시정지
    public void OnStop()
    {
        Time.timeScale = 0;
        Managers.Instance.UIManager.Show<OptionPopup>();
    }

    // 스킵 버튼 클릭 시 호출될 메소드 (현재 비어 있음)
    public void OnSkip()
    {
        // 씬 스킵 처리
    }

    // 상호작용 버튼 클릭 시 대화 시작
    public void OnInteraction()
    {
        Managers.Instance.DialogueManager.OnClick?.Invoke();
    }

    // 스킬 UI를 일괄 활성화/비활성화
    private void ToggleSkillUI(bool isActive)
    {
        hideSkill.SetActive(isActive);
        catSkill.SetActive(isActive);
        dogSkill.SetActive(isActive);
        squirrelSkill.SetActive(isActive);
    }

    // 선택한 스킬 배경만 보여주고 나머지는 끔
    private void ShowSkillBG(GameObject skillBG)
    {
        foreach (var bg in skillBGs)
        {
            bg.SetActive(bg == skillBG);
        }
    }
}
