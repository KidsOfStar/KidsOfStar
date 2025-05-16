
public class ClearPuzzlePopup : PopupBase
{
    //TODO: 모든 퍼즐 시스템 통합 및 모든 퍼즐 팝업을 리펙토링하고 로직 수정
    private TreePuzzleSystem treePuzzle;
    private WirePuzzleSystem wirePuzzle;

    public override void Opened(params object[] param)
    {
        base.Opened(param);

        if (param.Length > 0 && param[0] is TreePuzzleSystem ts)
            treePuzzle = ts;
        else
            treePuzzle = null;

        closeBtn.onClick.RemoveAllListeners();
        closeBtn.onClick.AddListener(() =>
        {
            // 1) 팝업 닫기
            Managers.Instance.UIManager.Hide<ClearPuzzlePopup>();

            // 2) TreePuzzleSystem이 있으면 컷신 재생 & 진행도 업데이트
            if (treePuzzle != null)
                treePuzzle.OnClearButtonClicked();
            if (wirePuzzle != null)
            {
                EditorLog.Log(wirePuzzle);
                wirePuzzle.OnClearButtonClicked();
            }
        });
    }
}
