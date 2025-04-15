using MainTable;
using System;

public class DataSaveAction : IDialogActionHandler
{
    public void Execute(DialogData playerData)
    {
        EditorLog.Log("데이터 세이브 시작");

        // 슬롯이랑 [난이도] 챕터 날짜 시간 string
        int slotIndex = 0; // 외부에서 지정하기

        Managers.Instance.SaveManager.Save(slotIndex, (saveName) =>
        {
            string slotName = saveName;
            EditorLog.Log($"슬롯 {slotIndex}에 저장됨: {slotName}");
        });

    }
}
