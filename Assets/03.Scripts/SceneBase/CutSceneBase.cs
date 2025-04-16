using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CutSceneBase : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;
    public PlayableDirector Director => director;
    [SerializeField] private SignalReceiver signalReceiver;
    [SerializeField] private SignalAsset dialogSignal;
    [SerializeField] private SignalAsset destroySignal;

    [Header("CutSceneData")]
    [SerializeField] private CutSceneData cutSceneData;

    private readonly UnityEvent showDialogEvent = new();
    private readonly UnityEvent destroyEvent = new();
    private int currentIndex = 0;

    private Camera mainCamera;
    private LetterBoxer letterBoxer;
    private void Start()
    {
        if (cutSceneData.Npcs != null)
        {
            Managers.Instance.DialogueManager.InitCutSceneNPcs(cutSceneData.Npcs);
        }
        Managers.Instance.DialogueManager.OnDialogEnd += ResumeCutScene;

        showDialogEvent.AddListener(ShowDialog);
        signalReceiver.AddReaction(dialogSignal, showDialogEvent);
        destroyEvent.AddListener(DestroyPrefab);
        signalReceiver.AddReaction(destroySignal, destroyEvent);

        mainCamera = Camera.main;

        if (mainCamera != null)
        {
            letterBoxer = mainCamera.GetComponent<LetterBoxer>();
            if (letterBoxer == null)
            {
                letterBoxer = mainCamera.gameObject.AddComponent<LetterBoxer>();
                letterBoxer.x = 16;
                letterBoxer.y = 9;
                letterBoxer.onUpdate = false;
            }
            letterBoxer.enabled = true;
            letterBoxer.PerformSizing(); 
        }
    }

    public void Play(string cutsceneName, int dialogStartIndex)
    {
        director.Play();

    }

    public void ShowDialog()
    {
        //director.Evaluate();
        //director.Pause();
        director.playableGraph.GetRootPlayable(0).SetSpeed(0);
        var index = cutSceneData.DialogIndexes[currentIndex];
        Managers.Instance.DialogueManager.SetCurrentDialogData(index);
        currentIndex++;
    }

    public void ResumeCutScene()
    {
        director.playableGraph.GetRootPlayable(0).SetSpeed(1);

        director.Resume();
    }

    public void DestroyPrefab()
    {
        Destroy(gameObject);

    }
   
}