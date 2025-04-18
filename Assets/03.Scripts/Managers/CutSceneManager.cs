using System;

public class CutSceneManager
{
    public bool IsCutScenePlaying { get; private set; } = false;
    public Action OnCutSceneStart { get; set; }
    public Action OnCutSceneEnd { get; set; }
    public LetterBoxer LetterBoxer { get; private set; }
    
    private const string CutScenePath = "CutScenes/";

    public CutSceneManager()
    {
        OnCutSceneEnd += () => IsCutScenePlaying = false;
    }
    
    public void PlayCutScene(string cutsceneName)
    {
        //letterBoxer = Managers.Instance.GameManager.MainCamera.GetComponent<LetterBoxer>();
        string prefabPath = $"{CutScenePath}{cutsceneName}";
        var cutSceneBase = Managers.Instance.ResourceManager.Instantiate<CutSceneBase>(prefabPath);
        //letterBoxer.PerformSizing();

        if (!cutSceneBase)
        {
            EditorLog.Log($"컷씬 프리팹이 없습니다: {prefabPath}");
            return;
        }

        cutSceneBase.Init();
        cutSceneBase.Play();

        // PlayableDirector 찾기 및 등록
        // baseComp.Play
        OnCutSceneStart?.Invoke();
        IsCutScenePlaying = true;
    }
}
