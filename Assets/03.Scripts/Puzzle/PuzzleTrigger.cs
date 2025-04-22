using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PuzzleTrigger : MonoBehaviour
{
    [SerializeField] private string[] allowedFormsNames;

    private bool triggered = false;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered) return;

        var formControl = Managers.Instance.GameManager.Player.FormControl;
        string currentForm = formControl.ReturnCurFormName();

        // 허용되지 않은 형태일 경우
        if (allowedFormsNames.Contains(currentForm))
        {
            if (currentForm == "Squirrel")
            {
                // 다람쥐면 경고창 띄움
                Managers.Instance.UIManager.Show<WarningPopup>();
                return;
            }
            triggered = true;
            var popup = Managers.Instance.UIManager.Show<TreePuzzlePopup>();
        }
    }
}
