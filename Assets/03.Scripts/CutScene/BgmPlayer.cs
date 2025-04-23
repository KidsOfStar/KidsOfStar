using UnityEngine;
using UnityEngine.Timeline;

public class BgmPlayer : MonoBehaviour
{
    [field: SerializeField] public BgmSoundType bgmSoundType {get; private set;}
    [field: SerializeField] public SignalAsset playBgmSignal {get; private set;}

    public void PlayBgm()
    {
        Managers.Instance.SoundManager.PlayBgm(bgmSoundType);
    }
}