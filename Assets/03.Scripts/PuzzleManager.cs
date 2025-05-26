using System.Collections.Generic;


public class PuzzleManager 
{
    private Dictionary<int, IPuzzleTrigger> triggerMap = new();

    public void OnSceneLoaded()
    {
        triggerMap.Clear();
        // 필요한 경우 기본 데이터 로딩 등 수행
    }

    public void RegisterTrigger(int puzzleId, IPuzzleTrigger trigger)
    {
        triggerMap[puzzleId] = trigger;
    }

    public void UnregisterTrigger(IPuzzleTrigger trigger)
    {
        if (triggerMap.ContainsKey(trigger.SequenceIndex))
            triggerMap.Remove(trigger.SequenceIndex);
    }

    public IPuzzleTrigger GetTrigger(int index)
    {
        triggerMap.TryGetValue(index, out var trigger);
        return trigger;
    }
}
