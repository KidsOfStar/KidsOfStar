using MainTable;

public class DataSaveAction : IDialogActionHandler
{
    public void Execute(DialogData playerData)
    {
        // TODO: ??? 구현하기
        
        // 슬롯이랑 [난이도] 챕터 날짜 시간 string
        int slotIndex = 0; // 외부에서 지정하기

        Managers.Instance.SaveManager.Save(slotIndex, (saveName) =>
        {
            EditorLog.Log($"슬롯 {slotIndex}에 저장됨: {saveName}");
        });
    }
}
