
public class ClearPuzzlePopup : PopupBase
{
    //TODO: 모든 퍼즐 시스템 통합 및 모든 퍼즐 팝업을 리펙토링하고 로직 수정
    private PuzzleSystemBase puzzleSystem;

    public override void Opened(params object[] param)
    {
        base.Opened(param);

        puzzleSystem = null;

        foreach (var p in param)
        {
            if (p is PuzzleSystemBase system)
            {
                puzzleSystem = system;
                break; // 하나만 받는 구조라면 break 가능
            }
        }

        closeBtn.onClick.RemoveAllListeners();
        closeBtn.onClick.AddListener(() =>
        {
            // 1) 팝업 닫기
            Managers.Instance.UIManager.Hide<ClearPuzzlePopup>();

            // 2) 공통 PuzzleSystemBase에서 처리
            puzzleSystem?.OnClearButtonClicked(); // 가상 메서드로 처리
        });
    }
}
