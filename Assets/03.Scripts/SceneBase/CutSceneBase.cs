using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CutSceneBase : MonoBehaviour
{
    [field: SerializeField] public PlayableDirector Director { get; private set; }
    [field: SerializeField] private SceneType sceneType;
    [SerializeField] private SignalReceiver signalReceiver;
    [SerializeField] private SignalAsset dialogSignal;
    [SerializeField] private SignalAsset destroySignal;

    [Header("CutSceneData")]
    [SerializeField] private CutSceneData cutSceneData;

    private int currentIndex;

    public void Init()
    {
        if (cutSceneData.Npcs != null)
        {
            Managers.Instance.DialogueManager.InitCutSceneNPcs(cutSceneData.Npcs);
        }
        
        Managers.Instance.DialogueManager.OnDialogEnd += ResumeCutScene;
    }

    public void Play()
    {
        Director.Play();
    }

    public void ShowDialog()
    {
        //director.Evaluate();
        //director.Pause();
        Director.playableGraph.GetRootPlayable(0).SetSpeed(0);

        var index = cutSceneData.DialogIndexes[currentIndex];
        Managers.Instance.DialogueManager.SetCurrentDialogData(index);
        currentIndex++;
    }

    private void ResumeCutScene()
    {
        Director.playableGraph.GetRootPlayable(0).SetSpeed(1);
        Director.Resume();
    }

    public void DestroyPrefab()
    {
        //Managers.Instance.CutSceneManager.LetterBoxer.DisableLetterBox();
        Managers.Instance.DialogueManager.OnDialogEnd -= ResumeCutScene;
        Managers.Instance.CutSceneManager.OnCutSceneEnd?.Invoke();
        if (sceneType != SceneType.Title)
            Managers.Instance.SceneLoadManager.LoadScene(sceneType);
        
        Destroy(gameObject);
    }
}