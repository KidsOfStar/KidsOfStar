using System.Collections;
using UnityEngine;

public class SoundManager : ISceneLifecycleHandler
{
    private readonly Transform sourceParent;
    private readonly AudioSource bgmSource;
    private readonly AudioSource sfxSource;
    private readonly AudioSource footstepSource;

    public SoundManager()
    {
        sourceParent = new GameObject("AudioSource").transform;
        sourceParent.SetParent(Managers.Instance.transform);
        GameObject audioSource =
            Managers.Instance.ResourceManager.Load<GameObject>($"{Define.PrefabPath}{Define.AudioSourceKey}");
        GameManager gameManager = Managers.Instance.GameManager;

        bgmSource = Object.Instantiate(audioSource, sourceParent).GetComponent<AudioSource>();
        bgmSource.name = "BGM";
        bgmSource.loop = true;
        bgmSource.volume = gameManager.BgmVolume;

        sfxSource = Object.Instantiate(audioSource, sourceParent).GetComponent<AudioSource>();
        sfxSource.name = "SFX";
        sfxSource.loop = false;
        sfxSource.volume = gameManager.SfxVolume;

        footstepSource = Object.Instantiate(audioSource, sourceParent).GetComponent<AudioSource>();
        footstepSource.name = "Footstep";
        footstepSource.loop = false;
        footstepSource.volume = gameManager.SfxVolume;
    }

    private void AttachAudioToCamera()
    {
        var currentScene = Managers.Instance.SceneLoadManager.CurrentScene;
        if (currentScene == SceneType.Title) return;

        var camera = Managers.Instance.GameManager.MainCamera;
#if UNITY_EDITOR
        if (Managers.Instance.IsDebugMode)
        {
            camera = Camera.main;
            sourceParent.SetParent(camera.transform);
            return;
        }
#endif
        if (camera == null)
        {
            EditorLog.LogError("SoundManager : Camera is not found.");
            return;
        }

        sourceParent.SetParent(camera.transform);
    }

    private void ReparentAudioToSoundManager()
    {
        var currentScene = Managers.Instance.SceneLoadManager.CurrentScene;
        if (currentScene == SceneType.Title)
            return;

        if (!sourceParent)
        {
            EditorLog.LogError("SoundManager : AudioSource is not found.");
            return;
        }

        sourceParent.SetParent(Managers.Instance.transform);
    }

    private AudioClip GetAudioClip(string key)
    {
        AudioClip audioClip = Managers.Instance.ResourceManager.Load<AudioClip>($"{Define.SoundPath}{key}");
        if (!audioClip)
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
        footstepSource.volume = volume;
    }

    // 효과음 재생하고 재생 시간만큼 대기
    public IEnumerator PlaySfxWithDelay(SoundType sound)
    {
        AudioClip clip = GetAudioClip(sound.GetName());
        sfxSource.PlayOneShot(clip);
        yield return new WaitForSeconds(clip.length); //사운드 종료되기 전 씬이 넘어가는 것을 방지
    }

    public void OnSceneLoaded()
    {
        AttachAudioToCamera();
    }

    public void OnSceneUnloaded()
    {
        ReparentAudioToSoundManager();
        StopBgm();
    }
}