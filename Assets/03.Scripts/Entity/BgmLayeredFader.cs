using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmLayeredFader : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource[] mainSources;

    [Header("Dialog")]
    [SerializeField] private int firstDialog = 5012;
    [SerializeField] private int[] dialogIndexes;
    [SerializeField] private int finalDialog = 5021;
    
    [Header("Loop Time")]
    [SerializeField] private float loopTime = 192f;

    private readonly Dictionary<MainBgmSourceType, AudioSource> audioDict = new();
    private float bgmVolume;
    private double dspStart;
    
    public void Start()
    {
        bgmVolume = Managers.Instance.GameManager.BgmVolume;
        
        // BGM Stop
        var soundManager = Managers.Instance.SoundManager;
        soundManager.StopBgm();
        soundManager.StopAmbience();
        
        // 볼륨 변경 이벤트에 구독
        soundManager.SetBgmVolumeCallback += SetVolume;

        InitDictionary();
        
        // 동시에 모든 오디오 재생
        // Rise Effect 제외 재생
        dspStart = AudioSettings.dspTime + 0.1;
        double dspLoopEnd = dspStart + loopTime;
        for (int i = 0; i < mainSources.Length - 1; i++)
        {
            mainSources[i].PlayScheduled(dspStart);
            mainSources[i].SetScheduledEndTime(dspLoopEnd);
        }

        var piano = audioDict[MainBgmSourceType.Piano];
        StartCoroutine(FadeInAudio(piano, 4f));
    }

    // 컷씬 시작 시 marimba 재생
    private void OnCutSceneStart()
    {
        var currentCutScene = Managers.Instance.CutSceneManager.CurrentCutSceneName;
        if (currentCutScene == "FinalChoice")
            PlayScr(MainBgmSourceType.Marimba);
    }

    // 특정 대사마다 3~8번 트랙 재생
    private void OnPlayAudioSources(int dialogIndex)
    {
        
    }

    // 9번 라이즈 이펙트 재생과 동시에 2~8번 멈춤
    private void OnPlayRiseEffect(int dialogIndex)
    {
        
    }

    // 특정 대화 이후 루프 캔슬
    private void OnLoopCancel()
    {
        double now = AudioSettings.dspTime;

        // 대사 완료 이후엔 남은 구간(3:12→3:13)만 재생하고 정지
        foreach (var src in mainSources)
        {
            double elapsed = now - dspStart;
            double positionInClip = elapsed % src.clip.length;
            double remaining = src.clip.length - positionInClip;
            src.SetScheduledEndTime(now + remaining);
        }
    }

    private void InitDictionary()
    {
        var count = 0;
        foreach (MainBgmSourceType value in Enum.GetValues(typeof(MainBgmSourceType)))
        {
            audioDict[value] = mainSources[count];
            count++;
        }
    }
    
    private void PlayScr(MainBgmSourceType srcType)
    {
        var scr = audioDict[srcType];
        var duration = srcType == MainBgmSourceType.StrMelody2 ? 4f : 2f;
        StartCoroutine(FadeInAudio(scr, duration));
    }
    
    private void SetVolume(float volume)
    {
        bgmVolume = volume;
    }
    
    private IEnumerator FadeInAudio(AudioSource src, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            src.volume = Mathf.Lerp(0f, bgmVolume, elapsed / duration);
            yield return null;
        }
        src.volume = bgmVolume;
    }

    private IEnumerator FadeOutAudio(AudioSource src, float duration)
    {
        float elapsed = 0f;       
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            src.volume = Mathf.Lerp(bgmVolume, 0f, elapsed / duration);
            yield return null;
        }                         
        src.volume = bgmVolume;   
    }

    private void OnDestroy()
    {
        Managers.Instance.SoundManager.SetBgmVolumeCallback -= SetVolume;
    }
}

public enum MainBgmSourceType
{
    Piano,
    StrMelody1,
    Marimba,
    StringPizz,
    Glock,Celesta,
    Woodwinds,
    Bass,
    StrMelody2,
    RiseEffect,
}
