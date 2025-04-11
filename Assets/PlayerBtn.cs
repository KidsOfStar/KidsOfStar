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
        // 점프 메소드 호출
        OnBlink(jumpBG, 0.1f); // 점프 배경 깜빡임 효과
        Managers.Instance.GameManager.Player.PlayerController.OnJump();

        // 점프한 후 땅에 착지했을 때 스킬창 보이기
        HideEffet();

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

    public void ShowEffet(GameObject ObjBG)
    {
        // 스킬을 클릭 시 배경 나오고
        ObjBG.SetActive(true);
        
        // 같은 스킬을 클릭 시 배경 사라짐
        if(!ObjBG.activeSelf) ObjBG.SetActive(false);

        //// 다른 스킬을 클릭 시 배경 사라짐
        else if (!ObjBG.activeSelf) ObjBG.SetActive(false);
    }

    public void HideEffet()
    {
        // 점프 후 지면에 착지했을 때 스킬창 보이기
       if(Managers.Instance.GameManager.Player.Controller.IsGround)
        {
            hideSkill.SetActive(true); // 숨기기 스킬 비활성화
            catSkill.SetActive(true); // 고양이 스킬 비활성화
            dogSkill.SetActive(true); // 개 스킬 비활성화
            squirrelSkill.SetActive(true); // 다람쥐 스킬 비활성화

        }
        else
        {
            hideSkill.SetActive(false); // 숨기기 스킬 비활성화
            catSkill.SetActive(false); // 고양이 스킬 비활성화
            dogSkill.SetActive(false); // 개 스킬 비활성화
            squirrelSkill.SetActive(false); // 다람쥐 스킬 비활성화

        }

    }
}
