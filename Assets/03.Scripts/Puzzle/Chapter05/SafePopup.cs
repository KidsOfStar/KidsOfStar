using UnityEngine;

public class SafePopup : PopupBase
{
    /// Chapter 5에서 사용되는 퍼즐 시스템
    [Header("퍼즐 시스템")]
    public bool elevatorPuzzle = false;
    public bool safePuzzle = false;

    public override void Opened(params object[] param)
    {
        base.Start();
    }

    // 모든 퍼즐이 완료되었는지 확인하는 메서드
    public bool IsAllPuzzlesCompleted()
    {
        return elevatorPuzzle && safePuzzle;
    }

    // 모든 퍼즐이 완료가 되면 챕터 진행도 올리기
    public void UpdateChapterProgress()
    {
        if (IsAllPuzzlesCompleted())
        {
            // 챕터 진행도 올리기
            Managers.Instance.GameManager.UpdateProgress();
            Debug.Log("모든 퍼즐이 완료되었습니다. 챕터 진행도를 올립니다.");
        }
    }
}
