using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SafePopup : PopupBase
{
    [SerializeField] private GameObject exclamationInstance;
    public Door door;

    [Header("퍼즐 시스템")]
    private SceneType sceneType;
    public TreePuzzleData[] datas; // 전체 9개 퍼즐 SO
    public SafePuzzleSystem[] puzzleSystems; // 3개 퍼즐 시스템 (각 퍼즐 UI와 연결됨)

    private int[] puzzleIndex;        // 현재 씬에서 사용할 퍼즐 3개의 인덱스
    public int countIndex = 0;

    protected override void Start()
    {
        door = GameObject.FindWithTag("Interactable").GetComponent<Door>();
        /*puzzleIndex = GetIndexSetForScene(sceneType);

        currentPuzzle = puzzleSystems[0];
        nextPuzzle();
        SetupAllPuzzles(); // 퍼즐 시스템 전체 초기화 및 세팅
        StartPuzzleAtIndex(0); // 첫 퍼즐 시작*/
    }

    public override void Opened(params object[] param)
    {
        if (param.Length > 0 && param[0] is SceneType scene)
        {
            sceneType = scene;
        }
        Managers.Instance.GameManager.Player.Controller.LockPlayer();

        puzzleIndex = GetIndexSetForScene(sceneType); // 꼭 세팅해줘야 함

        SetupAllPuzzles();
        StartPuzzleAtIndex(countIndex);

        Debug.Log($"[SafePopup] 씬 {sceneType} 의 퍼즐 인덱스 배열: {string.Join(", ", puzzleIndex)}");

    }
    public void nextPuzzle()
    {
        Debug.Log($"[SafePopup] nextPuzzle 호출됨 - 현재 countIndex: {countIndex}");

        if (countIndex >= puzzleIndex.Length)
        {
            Debug.LogWarning("[SafePopup] 퍼즐이 모두 완료되었습니다. 더 이상 진행할 퍼즐이 없습니다.");
            return;
        }

        // 인덱스 유효성 점검
        if (countIndex >= puzzleSystems.Length)
        {
            Debug.LogError($"[SafePopup] puzzleSystems 배열 인덱스 초과! countIndex: {countIndex}, puzzleSystems.Length: {puzzleSystems.Length}");
            return;
        }

        if (countIndex >= puzzleIndex.Length)
        {
            Debug.LogError($"[SafePopup] curSceneIndex 배열 인덱스 초과! countIndex: {countIndex}, curSceneIndex.Length: {puzzleIndex.Length}");
            return;
        }

        Debug.Log($"[SafePopup] 다음 퍼즐로 이동 준비 중...");
        Debug.Log($" - countIndex = {countIndex}");
        Debug.Log($" - 사용될 SO 인덱스 = {puzzleIndex[countIndex]}");
        Debug.Log($" - 사용할 PuzzleSystem = {puzzleSystems[countIndex].name}");

        StartPuzzleAtIndex(countIndex);

        countIndex++;
    }


    // 실패 시 퍼즐 리셋
    public void FullReset()
    {
        countIndex = 0;

        foreach (var puzzleSystem in puzzleSystems)
        {
            puzzleSystem.ResetSystem();
        }
        Debug.Log("SafePopup과 모든 퍼즐이 완전히 초기화되었습니다.");


        // 씬에 맞는 퍼즐 데이터 재설정
        puzzleIndex = GetIndexSetForScene(sceneType);
        SetupAllPuzzles();
        countIndex++;

        Debug.Log("SafePopup과 모든 퍼즐이 완전히 초기화되었습니다.");

    }

    public override void HideDirect()
    {
        base.HideDirect();
        Managers.Instance.GameManager.Player.Controller.UnlockPlayer();
    }

    private void StartPuzzleAtIndex(int index)
    {
        Debug.Log($"[StartPuzzleAtIndex] 호출됨 - index: {index}");

        if (index >= puzzleSystems.Length || index >= puzzleIndex.Length)
        {
            Debug.LogWarning($"[StartPuzzleAtIndex] 퍼즐 인덱스 범위 초과 - index: {index}, puzzleSystems.Length: {puzzleSystems.Length}, curSceneIndex.Length: {puzzleIndex.Length}");
            return;
        }

        var data = datas[puzzleIndex[index]];
        var system = puzzleSystems[index];

        Debug.Log($"[StartPuzzleAtIndex] 퍼즐 시작 - Scene: {sceneType}, index: {index}");
        Debug.Log($" - 사용될 TreePuzzleData 이름: {data.name}");
        Debug.Log($" - 연결된 퍼즐 시스템: {system.name}");

        system.SetupPuzzle(data, 2);
        system.GeneratePuzzle();
        system.StartPuzzle();
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
                return new int[] { 0, 1, 2};
        }
    }

    // 퍼즐 시스템에 데이터 미리 세팅 (생성 전에 필요한 준비)
    private void SetupAllPuzzles()
    {
        for(int i = 0; i < puzzleSystems.Length; i++)
        {
            if(i < puzzleIndex.Length)
            {
                puzzleSystems[i].SetupPuzzle(datas[puzzleIndex[i]], i);
            }
        }
    }
}
