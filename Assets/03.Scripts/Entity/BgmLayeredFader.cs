using System.Collections;
using UnityEngine;

public class BgmLayeredFader : MonoBehaviour
{
    private float bgmVolume;
    
    public void Init()
    {
        // BGM Stop
        Managers.Instance.SoundManager.StopBgm();
        Managers.Instance.SoundManager.StopAmbience();
        
        // 동시에 모든 오디오 재생
        // 이벤트에 재생하는 함수 구독
    }

    private IEnumerator FadeInAudio(AudioSource src, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;  // Time.timeScale 무관
            src.volume = Mathf.Lerp(0f, bgmVolume, elapsed / duration);
            yield return null;
        }
        src.volume = bgmVolume;
    }

    // private IEnumerator FadeOutAudio()
    // {
    //     
    // }
    
    // fade로 재생하는 기능
    // fade out까지 만들어야되네 개씨발
}
