using MainTable;

// 첫번째 액션인지, 두번째 액션인지
public interface IDialogActionHandler
{
    void Execute(DialogData dialogData, bool isFirst);
}

public class NoneAction : IDialogActionHandler
{
    public void Execute(DialogData dialogData, bool isFirst)
    {
        // 두번째 액션에서 end콜백이나 다음 대화를 처리하기 때문에
        // 첫번째 액션이라면 아무것도 하지 않음
        if (isFirst) return;
        
        // NextIndex가 없으면 대화 종료
        if (dialogData.NextIndex.Count <= 0)
        {
            Managers.Instance.DialogueManager.InvokeSceneDialogEnd();
            Managers.Instance.DialogueManager.OnDialogEnd?.Invoke();
            return;
        }

        var nextIndex = dialogData.NextIndex[0];
        Managers.Instance.DialogueManager.SetCurrentDialogData(nextIndex);
    }
}