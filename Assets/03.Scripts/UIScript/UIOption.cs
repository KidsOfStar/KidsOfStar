using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOption : UIBase
{
    [Header("UI Audio")]
    public Slider bgmSlider; // BGM 슬라이더
    public Slider sfxSlider; // SFX 슬라이더

    [Header("UI Btn")]
    public Button closeBtn; // 닫기 버튼
    public Button retryBtn; // 재시작 버튼
    public Button endBtn; // 메인 메뉴 버튼

    private SoundManager soundManager;

    // Start is called before the first frame update
    void Start()
    {
        soundManager = Managers.Instance.SoundManager;

        // 슬라이더 초기화
        soundManager.Init();

        bgmSlider.onValueChanged.AddListener(soundManager.SetBgmVolume);
        sfxSlider.onValueChanged.AddListener(soundManager.SetSfxVolume);

    }



    
}
