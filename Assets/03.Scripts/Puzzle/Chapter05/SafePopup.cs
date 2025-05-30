using System.Diagnostics.Contracts;
using UnityEngine;

public class SafePopup : PuzzlePopupBase<SafePuzzleSystem, TreePuzzleData>
{
    [Header("퍼즐 시스템")]
    private SceneType sceneType;
    public TreePuzzleData[] datas; // 전체 9개의 퍼즐 SO 배열
    private int[] puzzleIndex;     // 현재 씬에서 사용할 퍼즐 인덱스 3개
    public int curIndex = 0;

    public SafePuzzle safePuzzle;

    public override void Opened(params object[] param)
    {
        //base.Opened(param);

        if (param.Length > 0 && param[0] is SceneType scene)
        {
            sceneType = scene;
            puzzleIndex = GetIndexSetForScene(scene);
            curIndex = 0;
        }

        NextPuzzle();
    }

    public void NextPuzzle()
    {
        if (curIndex >= puzzleIndex.Length)
        {
            EditorLog.LogWarning("[SafePopup] 퍼즐이 모두 완료되었습니다. 더 이상 진행할 퍼즐이 없습니다.");
            return;
        }

        if (curIndex >= puzzleIndex.Length)
        {
            EditorLog.LogError($"[SafePopup] curSceneIndex 배열 인덱스 초과! countIndex: {curIndex}, curSceneIndex.Length: {puzzleIndex.Length}");
            return;
        }

        var data = datas[puzzleIndex[curIndex]];
        int index = curIndex;
        curIndex++;

        if (safePuzzle != null && safePuzzle.puzzleSystem != null)
        {
            safePuzzle.puzzleSystem.SetupPuzzle(data, curIndex - 1);
            safePuzzle.puzzleSystem.GeneratePuzzle();
        }
        else
        {
            Debug.LogError("[SafePopup] SafePuzzle 또는 SafePuzzleSystem이 할당되지 않았습니다.");
        }

        base.Opened(data, index);

    }

    public override void HideDirect()
    {
        base.HideDirect();
        Managers.Instance.GameManager.Player.Controller.UnlockPlayer();
        Managers.Instance.SoundManager.PlayBgm(BgmSoundType.Aquarium);
        Managers.Instance.SoundManager.PlayAmbience(AmbienceSoundType.Aquarium);
    }

    private int[] GetIndexSetForScene(SceneType sceneName)
    {
        switch (sceneName)
        {
            case SceneType.Chapter501:
                return new int[] { 0, 1, 2 }; // Chapter501에서 사용할 퍼즐 인덱스
            case SceneType.Chapter502:
                return new int[] { 3, 4, 5 }; // Chapter502에서 사용할 퍼즐 인덱스
            case SceneType.Chapter504:
                return new int[] { 6, 7, 8 }; // Chapter504에서 사용할 퍼즐 인덱스
            default:
                return new int[] { 0, 1, 2 }; // 기본값
        }
    }
}
