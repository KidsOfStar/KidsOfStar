using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class PuzzleSystemBase : MonoBehaviour
{
    protected int puzzleIndex;
    protected float timeLimit;
    protected float currentTime;
    protected bool isRunning;
    protected int challengeCount;
    protected HashSet<int> clearPuzzleSet = new();
    [SerializeField] protected int totalPuzzleCount = 2;
    [SerializeField] protected TextMeshProUGUI timerTxt;

    public abstract void SetupPuzzle(ScriptableObject puzzleData, int index);
    public abstract void GeneratePuzzle();
    public abstract void StartPuzzle();

    protected virtual void Update()
    {
        if (!isRunning) return;

        currentTime -= Time.deltaTime;
        timerTxt.text = Mathf.CeilToInt(currentTime).ToString();

        if (currentTime <= 0f)
        {
            FailPuzzle();
        }
    }

    public virtual void StopPuzzle()
    {
        isRunning = false;
        Managers.Instance.UIManager.Hide<PopupBase>();

        var trigger = Managers.Instance.PuzzleManager.GetTrigger(puzzleIndex);
        trigger?.ResetTrigger();
    }

    protected virtual void FailPuzzle()
    {
        isRunning = false;
        Managers.Instance.SoundManager.PlaySfx(SfxSoundType.PuzzleFail);

        Managers.Instance.UIManager.Show<GameOverPopup>();
        OnExit();

        var trigger = Managers.Instance.PuzzleManager.GetTrigger(puzzleIndex);
        trigger?.ResetTrigger();
    }

    public virtual void OnExit()
    {
        // Managers.Instance.SoundManager.PlayBgm(BgmSoundType.InForest); // 또는 상황에 따라 분기
        Time.timeScale = 1;
        Managers.Instance.GameManager.Player.Controller.UnlockPlayer();
    }
}

