using System.Collections.Generic;
using UnityEngine;

public class SkillUnlock : MonoBehaviour
{
    [Header("UI Skill BG")] // 각 스킬에 대한 배경 오브젝트
    public GameObject jumpBG;
    public GameObject hideBG;
    public GameObject catBG;
    public GameObject dogBG;
    public GameObject squirrelBG;

    [Header("UI Skill Icon")] // 각 스킬에 대한 아이콘 오브젝트
    public GameObject jumpIcon;
    public GameObject hideIcon;
    public GameObject catIcon;
    public GameObject dogIcon;
    public GameObject squirrelIcon;

    private List<GameObject> skillBGs; // 스킬 배경 오브젝트 리스트
    private Dictionary<string, (GameObject bg, GameObject icon)> skillMap; // 챕터별 스킬 BG/Icon 매핑
    //private List<string> unlockedSkills = Managers.Instance.GameManager.UnlockedForms;

    //private HashSet<int> unlockedSkills = new HashSet<int>(); // 스킬 잠금 해제 상태 저장

    void Awake()
    {
        unlockedSkills = Managers.Instance.GameManager.UnlockedForms;
        skillBGs = new List<GameObject> { hideBG, catBG, dogBG, squirrelBG };

        // 챕터 번호와 배경/아이콘 오브젝트 매핑
        skillMap = new Dictionary<string, (GameObject, GameObject)>
        {
            { "Squirrel", (squirrelBG, squirrelIcon) },
            { "Dog", (dogBG, dogIcon) },
            { "Cat", (catBG, catIcon) },
            { "Hide", (hideBG, hideIcon) },
        };
    }
    private void Start()
    {
        ApplyUnlockedSkills();
    }

    private void OnEnable()
    {
        // 이벤트 등록
        if (Managers.Instance.GameManager != null)
           Managers.Instance.GameManager.OnUnlockedForms += UnlockSkill;
    }

    private void OnDisable()
    {
        // 이벤트 해제
        if (Managers.Instance.GameManager != null)
            Managers.Instance.GameManager.OnUnlockedForms -= UnlockSkill;
    }

    // 선택한 스킬 BG만 활성화하고, 나머지는 비활성화
    public void ShowSkillBG(GameObject skillBG)
    {
        foreach (var bg in skillBGs)
        {
            bg.SetActive(bg == skillBG);
        }
    }

    // 저장된 잠금 해제 스킬들을 UI에 반영
    public void ApplyUnlockedSkills()
    {
        var unlockedSkills = Managers.Instance.GameManager.UnlockedForms;

        foreach (var chapter in unlockedSkills)
        {
            if (skillMap.TryGetValue(chapter, out var skillPair))
            {
                skillPair.bg.SetActive(true);
                skillPair.icon.SetActive(true);
            }
        }
    }

    // 특정 챕터의 스킬을 잠금 해제하고 바로 UI에 반영
    public void UnlockSkill(string chapter)
    {
        if (skillMap.TryGetValue(chapter, out var skillPair))
        {
            skillPair.bg.SetActive(false);
            skillPair.icon.SetActive(true);
        }
        else
        {
            Debug.LogWarning($"No skill assigned to chapter {chapter}");
        }
    }
}
