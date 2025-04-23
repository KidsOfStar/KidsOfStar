using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillBTN : UIBase
{
    [Header("UI SkillBtn")] // 스킬 버튼들
    public Button jumpBtn;
    public Button hideBtn;
    public Button catBtn;
    public Button dogBtn;
    public Button squirrelBtn;
    public Button interactionBtn;

    [Header("UI HideSkill")] // 스킬 UI 오브젝트들 (버튼 아래 표시용)
    public GameObject hideSkill;
    public GameObject catSkill;
    public GameObject dogSkill;
    public GameObject squirrelSkill;

    public Action OnInteractBtnClick { get; set; }

    public SkillUnlock skillUnlock; // 스킬 잠금 해제 UI
    private bool isSkillActive = false; // 스킬 UI 활성화 여부
    private float skillCooldown = 0.5f; // 스킬 쿨타임
    // Start is called before the first frame update
    void Start()
    {
        jumpBtn.onClick.AddListener(OnJump);
        hideBtn.onClick.AddListener(OnHide);
        catBtn.onClick.AddListener(OnCat);
        dogBtn.onClick.AddListener(OnDog);
        squirrelBtn.onClick.AddListener(OnSquirrel);
        interactionBtn.onClick.AddListener(OnInteraction);

        interactionBtn.gameObject.SetActive(false); // 상호작용 버튼 비활성화

        skillUnlock = GetComponent<SkillUnlock>();
    }

    private void Update()
    {
        if (Managers.Instance.GameManager.Player != null)
        {
            IsGruoud(); // 플레이어가 땅에 있는지 확인
        }
    }

    // 플레이어가 땅에 있는지 확인하여 스킬 UI 표시 여부 결정
    private void IsGruoud()
    {
        //플레이어가 땅에 있는지 확인하여 스킬 UI 표시 여부 결정
        if (Managers.Instance.GameManager.Player.Controller.IsGround)
        {
            ToggleSkillUI(true);  // 땅에 닿았으면 스킬 UI 활성화
        }
        else
        {
            ToggleSkillUI(false); // 공중에 있으면 스킬 UI 비활성화
        }
    }

    public void OnJump()
    {
        ToggleSkillUI(false); // 점프 시 스킬 UI 숨기기
        OnBlink(skillUnlock.jumpBG, 0.1f); // 배경 깜빡임 효과

        // 공중일 경우 점프 불가
        if (!Managers.Instance.GameManager.Player.Controller.IsGround)
            return;

        // 실제 점프 실행
        Managers.Instance.GameManager.Player.Controller.Jump();
    }

    // 숨기기 스킬 버튼 클릭 시 호출
    public void OnHide()
    {
        if (isSkillActive) return;

        isSkillActive = true; // 스킬 활성화 상태로 변경

        Managers.Instance.GameManager.Player.FormControl.FormChange("Hide");

        // 현재 배경이 켜져 있으면 끄고, 아니면 보여주기
        if (skillUnlock.hideBG.activeSelf)
        {
            skillUnlock.hideBG.SetActive(false);
        }
        else
        {
            skillUnlock.ShowSkillBG(skillUnlock.hideBG);
        }
        StartCoroutine(ResetSkillCooldown(skillCooldown));

    }

    // 고양이 스킬 버튼 클릭 시 호출
    public void OnCat()
    {
        if (isSkillActive) return;

        isSkillActive = true; // 스킬 활성화 상태로 변경

        Managers.Instance.GameManager.Player.FormControl.FormChange("Cat");

        if (skillUnlock.catBG.activeSelf)
        {
            skillUnlock.catBG.SetActive(false);
        }
        else
        {
            skillUnlock.ShowSkillBG(skillUnlock.catBG);
        }
        StartCoroutine(ResetSkillCooldown(skillCooldown));
    }

    // 개 스킬 버튼 클릭 시 호출
    public void OnDog()
    {
        if (isSkillActive) return;

        isSkillActive = true; // 스킬 활성화 상태로 변경.

        Managers.Instance.GameManager.Player.FormControl.FormChange("Dog");

        if (skillUnlock.dogBG.activeSelf)
        {
            skillUnlock.dogBG.SetActive(false);
        }
        else
        {
            skillUnlock.ShowSkillBG(skillUnlock.dogBG);
        }

        StartCoroutine(ResetSkillCooldown(skillCooldown));
    }

    // 다람쥐 스킬 버튼 클릭 시 호출
    public void OnSquirrel()
    {
        if (isSkillActive) return;

        isSkillActive = true; // 스킬 활성화 상태로 변경

        Managers.Instance.GameManager.Player.FormControl.FormChange("Squirrel");

        if (skillUnlock.squirrelBG.activeSelf)
        {
            skillUnlock.squirrelBG.SetActive(false);
        }
        else
        {
            skillUnlock.ShowSkillBG(skillUnlock.squirrelBG);
        }
        StartCoroutine(ResetSkillCooldown(skillCooldown));

    }

    // 상호작용 버튼 클릭 시 대화 시작
    public void OnInteraction()
    {
        OnInteractBtnClick?.Invoke();
        Managers.Instance.DialogueManager.OnClick?.Invoke();
    }
    public void ShowInteractionButton(bool isActive)
    {
        interactionBtn.gameObject.SetActive(isActive);
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

    private void ToggleSkillUI(bool isActive)
    {
        hideSkill.SetActive(isActive);
        catSkill.SetActive(isActive);
        dogSkill.SetActive(isActive);
        squirrelSkill.SetActive(isActive);
    }

    private IEnumerator ResetSkillCooldown(float delay)
    {
        yield return new WaitForSeconds(delay);
        isSkillActive = false;
    }
}
