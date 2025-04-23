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

    void Start()
    {
        skillBGs = new List<GameObject> { hideBG, catBG, dogBG, squirrelBG };
    }

    public void ShowSkillBG(GameObject skillBG)
    {
        foreach (var bg in skillBGs)
        {
            bg.SetActive(bg == skillBG);
        }
    }

}
