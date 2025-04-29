using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

// 컷씬용 다이얼로그 플레이어
public class DialogPlayer : MonoBehaviour
{
    [field: SerializeField] private SignalAsset dialogSignal;
    [field: SerializeField] public int[] DialogIndexes { get; private set; }
    [field: SerializeField] public CutSceneNpc[] Npcs { get; private set; }
    private int currentIndex;

    public void ShowDialog(PlayableDirector director)
    {
        director.playableGraph.GetRootPlayable(0).SetSpeed(0);

        var index = DialogIndexes[currentIndex];
        Managers.Instance.DialogueManager.SetCurrentDialogData(index);
        currentIndex++;
    }
}