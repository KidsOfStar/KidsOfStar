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

    private Dictionary<int, (GameObject bg, GameObject icon)> skillMap;

    void Awake()
    {
        skillBGs = new List<GameObject> { hideBG, catBG, dogBG, squirrelBG };

        // 챕터와 BG/Icon 매핑
        skillMap = new Dictionary<int, (GameObject, GameObject)>
        {
            { 1, (squirrelBG, squirrelIcon) },
            { 2, (dogBG, dogIcon) },
            { 3, (catBG, catIcon) },
            { 4, (hideBG, hideIcon) },
        };
    }

    public void ShowSkillBG(GameObject skillBG)
    {
        foreach (var bg in skillBGs)
        {
            bg.SetActive(bg == skillBG);
        }
    }

    public void UnlockSkill(int chapter)
    {
        if (skillMap.TryGetValue(chapter, out var skillPair))
        {
            skillPair.bg.SetActive(true);
            skillPair.icon.SetActive(true);
        }
        else
        {
            Debug.LogWarning($"No skill assigned to chapter {chapter}");
        }
    }
}
