using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DialogPlayer : MonoBehaviour
{
    [field: SerializeField] private SignalAsset DialogSignal;
    [field: SerializeField] public int[] DialogIndexes { get; private set; }
    [field: SerializeField] public CutSceneNpc[] Npcs { get; private set; }
    public int CurrentIndex { get; private set; } = 0;

    public void ShowDialog(PlayableDirector director)
    {
        director.playableGraph.GetRootPlayable(0).SetSpeed(0);

        var index = DialogIndexes[CurrentIndex];
        Managers.Instance.DialogueManager.SetCurrentDialogData(index);
        CurrentIndex++;
    }
}