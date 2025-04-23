using System;
using UnityEngine;

public class Chapter01PuzzleBase : SceneBase
{
    [Header("Chapter1 Puzzle")]
    [SerializeField] private ObstaclesSpawner spawner;
    [SerializeField] private Animator maorum;
    private readonly int walkHash = Animator.StringToHash("IsWalk");
    
    protected override void InitSceneExtra(Action playIntroCallback)
    {
        Managers.Instance.SoundManager.PlayBgm(BgmSoundType.MaorumChase);
        PlayChaseAnim();
        spawner.StartSpawn();
    }
    
    private void PlayChaseAnim()
    {
        maorum.speed = 1.5f;
        maorum.SetBool(walkHash, true);
    }
}