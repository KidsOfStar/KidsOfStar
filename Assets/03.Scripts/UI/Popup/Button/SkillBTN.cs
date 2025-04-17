using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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


    // Start is called before the first frame update
    void Start()
    {
        jumpBtn.onClick.AddListener(OnJump);
        hideBtn.onClick.AddListener(OnHide);
        catBtn.onClick.AddListener(OnCat);
        dogBtn.onClick.AddListener(OnDog);
        squirrelBtn.onClick.AddListener(OnSquirrel);
    }


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
            bg.SetActive(bg == skillBG);
        }
    }

}
