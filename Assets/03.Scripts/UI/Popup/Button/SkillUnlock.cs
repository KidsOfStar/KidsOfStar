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

    [Header("UI Skill Icon")] // 각 스킬에 대한 배경 오브젝트
    public GameObject jumpIcon;
    public GameObject hideIcon;
    public GameObject catIcon;
    public GameObject dogIcon;
    public GameObject squirrelIcon;

    private List<GameObject> skillBGs; // 스킬 배경 오브젝트 리스트
    private Dictionary<int, (GameObject bg, GameObject icon)> skillChapter; // 스킬 배경과 아이콘을 매핑하는 딕셔너리

    void Start()
    {
        skillBGs = new List<GameObject> { hideBG, catBG, dogBG, squirrelBG };

        skillChapter = new Dictionary<int, (GameObject bg, GameObject icon)>
        {
            { 1, (hideBG, hideIcon) },
            { 2, (catBG, catIcon) },
            { 3, (dogBG, dogIcon) },
            { 4, (squirrelBG, squirrelIcon) }
        };
    }

    public void ShowSkillBG(GameObject skillBG)
    {
        foreach (var bg in skillBGs)
        {
            bg.SetActive(bg == skillBG);
        }
    }

    // 각 챕터마다 스킬 잠금 해제
    public void UnlockSkill(int chapter)
    {
       if(skillChapter.TryGetValue(chapter, out var skill))
        {
            skill.bg.SetActive(false); // 스킬 배경 활성화
            skill.icon.SetActive(true); // 스킬 아이콘 활성화
        }
        else
        {
            Debug.LogWarning($"Chapter {chapter} not found in skillChapter dictionary.");
        }
    }
}
