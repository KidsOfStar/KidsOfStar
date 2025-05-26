using UnityEngine;
using UnityEngine.UI;

public interface IPuzzleData
{
    Sprite backgroundSprite { get; }
}

public abstract class PuzzlePopupBase<TSystem, TData> : PopupBase
    where TSystem : PuzzleSystemBase
    where TData : ScriptableObject, IPuzzleData
{
    [SerializeField] protected TSystem puzzleSystem;
    [SerializeField] protected Image hintImage;

    public override void Opened(params object[] param)
    {
        base.Opened(param);

        if (param.Length < 2 || !(param[0] is TData data) || !(param[1] is int index))
        {
            EditorLog.LogError($"[PuzzlePopupBase] Opened 인자 오류! param[0]: {param[0]?.GetType()}, param[1]: {param[1]?.GetType()}");
            return;
        }

        if (hintImage != null)
            hintImage.sprite = data.backgroundSprite;

        Managers.Instance.GameManager.Player.Controller.LockPlayer();

        puzzleSystem.SetupPuzzle(data, index);
        puzzleSystem.GeneratePuzzle();
        puzzleSystem.StartPuzzle();
    }

    protected virtual void OnCancelButtonClicked()
    {
        Managers.Instance.SoundManager.PlaySfx(SfxSoundType.UICancel);
        puzzleSystem.StopPuzzle();
        puzzleSystem.OnExit();
    }
}
