using UnityEngine;

public class DialogueManager
{
    [SerializeField] private Transform maorum;
    private UITextBubble textBubble;
    
    public void Init()
    {
        textBubble = Managers.Instance.UIManager.Show<UITextBubble>();
        textBubble.HideDirect();
    }

    public void Test(Vector3 offset)
    {
        // NPC에서 말풍선 위치를 가져와서 띄우는 것까지.
        var camera = Managers.Instance.GameManager.MainCamera;
        Vector3 screenPos = camera.WorldToScreenPoint(maorum.position + offset);
        
        // TODO: 화면을 벗어나면 안쪽으로 당기기

        textBubble.SetDialog("이건 테스트란다.");
    }
    
    // None
    /// nextDialog가 없을 때까지 출력해야 함
    // ShowSelect
    /// 대사 출력이 끝나고 선택지를 보여줘야 함
    // DataSave
    /// 대사 출력이 끝나고 데이터를 세이브할지 말지 결정해야함
    // ModifyTrust
    /// 대사 출력이 끝나고 신뢰도를 수정해야함
    
    
    // 대사가 끝나면 액션에 따라 할 일이 달라짐
    // Enum 타입으로 가지고 있음
}
