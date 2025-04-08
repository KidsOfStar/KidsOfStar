using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    private Transform sourceParent;
    private AudioSource bgmSource;
    private AudioSource sfxSource;
    private AudioSource footstepSource;
    
    public void Init()
    {
        sourceParent = new GameObject("AudioSource").transform;
        GameObject audioSource = Managers.Instance.ResourceManager.Load<GameObject>($"{Define.PrefabPath}{Define.AudioSourceKey}");
        
        bgmSource = Object.Instantiate(audioSource, sourceParent).GetComponent<AudioSource>();
        bgmSource.name = "BGM";
        bgmSource.loop = true;
        bgmSource.volume = 0.7f; // TODO: 설정에서 가져오기
        
        sfxSource = Object.Instantiate(audioSource, sourceParent).GetComponent<AudioSource>();
        sfxSource.name = "SFX";
        sfxSource.loop = false;
        sfxSource.volume = 0.8f; // TODO: 설정에서 가져오기
        
        footstepSource = Object.Instantiate(audioSource, sourceParent).GetComponent<AudioSource>();
        footstepSource.name = "Footstep";
        footstepSource.loop = false;
        footstepSource.volume = 0.8f; // TODO: 설정에서 가져오기
        
        AttachAudioToCamera();
    }

    public void AttachAudioToCamera()
    {
        var camera = Managers.Instance.GameManager.mainCamera;
        if (camera == null)
        {
            EditorLog.LogError("SoundManager : Camera is not found.");
            return;
        }
        
        sourceParent.SetParent(camera.transform);
    }

    public void ReparentAudioToSoundManager()
    {
        if (sourceParent == null)
        {
            EditorLog.LogError("SoundManager : AudioSource is not found.");
            return;
        }
        
        sourceParent.SetParent(null);
    }
    
    private AudioClip GetAudioClip(string key)
    {
        AudioClip audioClip = Managers.Instance.ResourceManager.Load<AudioClip>($"{Define.SoundPath}{key}");
        if (audioClip == null)
        {
            EditorLog.LogError($"SoundManager : {key} is not found.");
            return null;
        }

        return audioClip;
    }

    // BGM 재생(Loop)
    public void PlayBgm(SoundType sound)
    {
        AudioClip clip = GetAudioClip(sound.GetName());
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    // BGM 정지
    public void StopBgm()
    {
        bgmSource.Stop();
        bgmSource.clip = null;
    }

    // 효과음 재생
    public void PlaySfx(SoundType sound)
    {
        AudioClip clip = GetAudioClip(sound.GetName());
        sfxSource.PlayOneShot(clip);
    }
    
    // 발소리 재생
    // TODO: 발소리 캐릭터마다 다르게 쓰려나?
    public void PlayFootstep(FootstepType sound)
    {
        AudioClip clip = GetAudioClip(sound.GetName());
        footstepSource.PlayOneShot(clip);
    }
    
    public void SetBgmVolume(float volume)
    {
        bgmSource.volume = volume;
    }
    
    public void SetSfxVolume(float volume)
    {
        sfxSource.volume = volume;
    }

    // 효과음 재생하고 재생 시간만큼 대기
    public IEnumerator PlaySfxWithDelay(SoundType sound)
    {
        AudioClip clip = GetAudioClip(sound.GetName());
        sfxSource.PlayOneShot(clip);
        yield return new WaitForSeconds(clip.length); //사운드 종료되기 전 씬이 넘어가는 것을 방지
    }
}