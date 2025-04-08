public class DialogueManager
{
    // 대사 @로 구분해서 한줄씩 출력하기
    // 선택지?
    // NPC를 가져와서 NPC의 말풍선 위에 띄우기

    public void Init()
    {
        var test = Managers.Instance.DataManager.GetPlayerData(1000);
        foreach (var option in test.SelectOption)
        {
            EditorLog.Log(option);
        }
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
