using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어와의 상호작용을 통한 퍼즐 진입을 담당
/// </summary>
public class WirePuzzleTrigger : MonoBehaviour
{
    [SerializeField, Tooltip("튜토리얼을 띄울 트리거인지 여부")] private bool isTutorialDoor = false;
    // 튜터리얼 트리거인 경우, 이미 튜토리얼을 보였는지 여부
    private bool tytorialShown = false;
    // 퍼즐이 이미 작동되었는지 여부
    private bool triggered = false;

    [SerializeField, Tooltip("연결된 퍼즐 데이터")] private WirePuzzleData puzzleData;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
