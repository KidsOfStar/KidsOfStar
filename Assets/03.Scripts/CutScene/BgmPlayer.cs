using UnityEngine;
using UnityEngine.Timeline;

// 컷씬용 BGM 플레이어
public class BgmPlayer : MonoBehaviour
{
    [field: SerializeField] public BgmSoundType BgmSoundType {get; private set;}
    [field: SerializeField] public SignalAsset PlayBgmSignal {get; private set;}

    public void PlayBgm()
    {
        Managers.Instance.SoundManager.PlayBgm(BgmSoundType);
    }
}