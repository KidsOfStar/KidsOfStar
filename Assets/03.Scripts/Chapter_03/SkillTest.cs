using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTest : MonoBehaviour
{

    public SkillUnlock skillUnlock;

    private void Start()
    {
        skillUnlock = Managers.Instance.UIManager.Get<PlayerBtn>().skillPanel.GetComponent<SkillUnlock>();
    }

    public void SkillView()
    {
        skillUnlock.UnlockSkill("Dog");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //skillUnlock.ShowSkillBG(skillUnlock.catBG);
            SkillView();
            Debug.Log("스킬 해금");
        }
    }
}
