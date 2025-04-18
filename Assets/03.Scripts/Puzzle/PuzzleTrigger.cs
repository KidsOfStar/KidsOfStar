using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PuzzleTrigger : MonoBehaviour
{
    [SerializeField] private GameObject puzzlePopup;
    [SerializeField] private string[] allowedFormsNames; 

    public void TryOpenPuzzle()
    {
        var formControl = Managers.Instance.GameManager.Player.FormControl;
        string currentForm = formControl.ReturnCurFormName(); // 현재 형태 이름

        if (!allowedFormsNames.Contains(currentForm))
        {
            // 다람쥐 형태면 대사 출력
            if (currentForm == "Squirrel")
            {
               // ("세심하게 만져야 한다. 다른 방법이 없을까?");
            }

            // 그 외 형태는 무시
            return;
        }


        puzzlePopup.SetActive(true);
        puzzlePopup.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }
}
