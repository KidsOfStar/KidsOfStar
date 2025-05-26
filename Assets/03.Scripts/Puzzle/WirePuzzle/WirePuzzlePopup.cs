using UnityEngine;
using UnityEngine.UI;

public class WirePuzzlePopup : PuzzlePopupBase<WirePuzzleSystem, WirePuzzleData>
{
    // 퍼즐 취소 버튼 클릭 시 동작
    protected override void OnCancelButtonClicked()
    {
        base.OnCancelButtonClicked();
    }
}
