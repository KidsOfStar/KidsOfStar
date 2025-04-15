using MainTable;
using System;

public class DataSaveAction : IDialogActionHandler
{
    public void Execute(DialogData playerData)
    {
        EditorLog.Log("데이터 세이브 시작");

        // 슬롯이랑 [난이도] 챕터 날짜 시간 string
        int sllotIndex = 0; // 외부에서 지정하기

        Managers.Instance.SaveManager.Save(sllotIndex, (timeStamp) =>
        {
            // SaveData.cs 의 timeStamp 포맷은 "Chapter01. 25-03-22 14:14:38"
            string slotName = timeStamp;
        });
    }
}
