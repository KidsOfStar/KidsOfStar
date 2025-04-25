using System;
using UnityEngine;

// 챕터 1 특이사항
// 챕터 1 진행도는 총 4단계이며, 각 단계는 다음과 같습니다.
// 1. 마오름과 첫 대화(대화로 인해 진행도 1 증가 및 인간 폼으로 변경)
// 2. 마오름과 한번 더 대화 후 특정 장소에 도달하면 진행도 1 증가 : 이 때 NPC들의 위치 변경
// 3. 마지막으로 마오름과 대화하면 진행도 1 증가 및 추격 씬으로 이동
// 4. 추격 씬을 클리어하면 챕터 1 종료 및 씬 이동하여 챕터 2로 이동
public class Chapter01Base : SceneBase
{
    [Header("Chapter 1")]
    [SerializeField] private SceneEventTrigger sceneEventTrigger;
    
    protected override void InitSceneExtra(Action callback)
    {
        Managers.Instance.SoundManager.PlayBgm(BgmSoundType.Maorum);
        Managers.Instance.CutSceneManager.PlayCutScene(CutSceneType.Intro.GetName(), callback);
        sceneEventTrigger.Init();
    }
}