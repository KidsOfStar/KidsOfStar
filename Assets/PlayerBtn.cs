using System;
using System.Collections;
using System.Collections.Generic;
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
    public GameObject jumpBG;       // 점프 배경
    public GameObject hideBG;       // 숨기기 배경
    public GameObject catBG;        // 고양이 배경
    public GameObject dogBG;        // 개 배경
    public GameObject squirrelBG;   // 다람쥐 배경

    [Header("UI HideSkill")]
    public GameObject hideSkill;    // 숨기기 스킬
    public GameObject catSkill;     // 고양이 스킬
    public GameObject dogSkill;     // 개 스킬
    public GameObject squirrelSkill; // 다람쥐 스킬

    private List<GameObject> skillBGs;

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

        // 스킬 UI 비활성화
        skillBGs = new List<GameObject> { hideBG, catBG, dogBG, squirrelBG };
    }

    private void Update()
    {
        // 스킬 UI 활성화 여부에 따라 버튼 비활성화
        if (Managers.Instance.GameManager.Player.Controller.IsGround)
        {
            ToggleSkillUI(true); // 땅에 닿으면 스킬 UI 활성화
        }
        else
        {
            ToggleSkillUI(false); // 공중에 있으면 스킬 UI 비활성화
        }
    }

    public void OnJump()
    {
        ToggleSkillUI(false); // 스킬 UI 비활성화

        // 점프 메소드 호출
        OnBlink(jumpBG, 0.1f); // 점프 배경 깜빡임 효과
        if (!Managers.Instance.GameManager.Player.Controller.IsGround)
            return; // 땅에 닿지 않으면 점프 불가
        else 
            Managers.Instance.GameManager.Player.Controller.Jump();
    }

    public void OnHide()
    {
        // 형태변화 스크립트에서 호출
        Managers.Instance.GameManager.Player.FormControl.FormChange("Hide");
        ShowSkillBG(hideBG);

    }
    public void OnCat()
    {
        // 형태변화 스크립트에서 호출
        Managers.Instance.GameManager.Player.FormControl.FormChange("Cat");
        ShowSkillBG(catBG); // 고양이 스킬 UI 비활성화

    }
    public void OnDog()
    {
        // 형태변화 스크립트에서 호출
        Managers.Instance.GameManager.Player.FormControl.FormChange("Dog");
        ShowSkillBG(dogBG);

    }
    public void OnSquirrel()
    {
        // 형태변화 스크립트에서 호출
        Managers.Instance.GameManager.Player.FormControl.FormChange("Squirrel");
        ShowSkillBG(squirrelBG);
    }

    private IEnumerator BlinkEffect(GameObject obj, float duration)
    {
        // 오브젝트 깜빡임 효과
        obj.SetActive(true);
        yield return new WaitForSeconds(duration);
        obj.SetActive(false);
    }

    public void OnBlink(GameObject obj, float duration)
    {
        // 오브젝트 깜빡임 효과
        StartCoroutine(BlinkEffect(obj, duration));
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

    private void ToggleSkillUI(bool isActive)
    {
        hideSkill.SetActive(isActive);
        catSkill.SetActive(isActive);
        dogSkill.SetActive(isActive);
        squirrelSkill.SetActive(isActive);
    }

    private void ShowSkillBG(GameObject skillBG)
    {
        foreach (var bg in skillBGs)
        {
            bg.SetActive(bg == skillBG); // 모든 스킬 배경 비활성화
        }
    }
}
