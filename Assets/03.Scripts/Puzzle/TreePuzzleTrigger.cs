using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TreePuzzleTrigger : MonoBehaviour
{
    [SerializeField] private string[] allowedFormsNames;

    private bool triggered = false;
    private SkillBTN skillBTN;

    private void Start()
    {
        skillBTN = Managers.Instance.UIManager.Get<PlayerBtn>().skillPanel;

        skillBTN.OnInteractBtnClick = TryStartPuzzle;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered) return;

        var formControl = Managers.Instance.GameManager.Player.FormControl;
        string currentForm = formControl.ReturnCurFormName();

        // 허용되지 않은 형태일 경우
        if (currentForm == "Squirrel")
        {
            // 다람쥐면 경고창 띄움
            Managers.Instance.UIManager.Show<TreeWarningPopup>();
            skillBTN.ShowInteractionButton(false);
            return;
        }
        if (allowedFormsNames.Contains(currentForm))
        {
            skillBTN.ShowInteractionButton(true); // 상호작용 버튼 표시
        }
    }

    private void TryStartPuzzle()
    {
        if (triggered) return;

        triggered = true;
        Managers.Instance.UIManager.Show<TreePuzzlePopup>();
        skillBTN.ShowInteractionButton(false); // 한 번만 작동하게끔 비활성화
    }

}
