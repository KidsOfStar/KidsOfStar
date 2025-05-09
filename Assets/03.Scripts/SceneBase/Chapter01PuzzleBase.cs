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
        playIntroCallback?.Invoke();
        LockPlayerMove();
        PlayChaseAnim();
        spawner.StartSpawn();
    }

    protected override void CutSceneEndCallback()
    {
        Managers.Instance.SoundManager.PlayBgm(BgmSoundType.MaorumChase);
    }

    private void PlayChaseAnim()
    {
        maorum.speed = 1.5f;
        maorum.SetBool(walkHash, true);
    }

    private void LockPlayerMove()
    {
        var player = Managers.Instance.GameManager.Player;
        player.Controller.IsChaseMode = true;
        player.Controller.MoveSpeed = 0;
        player.Controller.Anim.SetBool(PlayerAnimHash.AnimMove, true);
    }
}