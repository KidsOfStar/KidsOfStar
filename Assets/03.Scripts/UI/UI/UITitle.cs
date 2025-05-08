using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UITitle : UIBase
{
    [Header("Button")]
    public Button startBtn;
    public Button exitBtn;
    public Button optionBtn;
    public Button loadBtn;

    
    // Start is called before the first frame update
    void Start()
    {
        OnClickListener(startBtn, OnStartBtnClick, SfxSoundType.UIButton);
        OnClickListener(exitBtn, OnExitBtnClick, SfxSoundType.UIButton);
        OnClickListener(optionBtn, OnOptionBtnClick, SfxSoundType.UIButton);
        OnClickListener(loadBtn, OnLoadBtnClick, SfxSoundType.UIButton);
    }
    // 따로 스크립트로 빼서 관리하는게 좋을듯
    private void OnClickListener(Button button, UnityAction callback, SfxSoundType sfxType)
    {
        button.onClick.AddListener(() =>
        {
            Managers.Instance.SoundManager.PlaySfx(sfxType); // 효과음 재생
            callback?.Invoke(); // 콜백 실행
        });
    }

    private void OnLoadBtnClick()
    {
        // 로드 Popup 띄우기
        Managers.Instance.UIManager.Show<LoadPopup>();
        //Managers.Instance.UIManager.Show<SavePopup>();

    }

    private void OnOptionBtnClick()
    {
        // 옵션 Popup 띄우기
        Managers.Instance.UIManager.Show<OptionPopup>();
    }

    private void OnExitBtnClick()
    {
        Managers.Instance.UIManager.Show<WarningEndTop>(); // 종료 경고창 띄우기
    }

    private void OnStartBtnClick()
    {
        // 데이터가 있을 경우 
        //Managers.Instance.UIManager.Show<WarningStartTop>();

        // 아닐 경우 게임 시작
        //NextScene();
        if(Input.touchCount > 1)
        {
            return;
        }
        else
        {
            // 한 손가락으로 터치 시
            //Debug.Log("한 손가락으로 터치");
            LoadScene();
        }   
    }

    private void LoadScene()
    {
        Managers.Instance.SceneLoadManager.LoadScene(SceneType.Chapter1);
    }
}
