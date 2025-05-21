using UnityEngine;
using UnityEngine.UI;

public class LoadTop : PopupBase
{
    private int slotIndex {  get; set; }
    public Button checkButton;

    public void SetUp(int index)
    {
        slotIndex = index;
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        checkButton.onClick.AddListener(OnCheck);

    }
    private void Load()
    {
        Managers.Instance.SaveManager.Load(slotIndex);
    }
    
    private void OnCheck()
    {
        // 씬 불러오기
        Load();
        Time.timeScale = 1;
        var loadChapter = Managers.Instance.GameManager.CurrentChapter;
        var loadScene = GetLoadSceneType(loadChapter);
        Managers.Instance.SceneLoadManager.LoadScene(loadScene);
    }

    private SceneType GetLoadSceneType(ChapterType chapter)
    {
        return chapter switch
        {
            ChapterType.Chapter1   => SceneType.Chapter1,
            ChapterType.Chapter2   => SceneType.Chapter2,
            ChapterType.Chapter3   => SceneType.Chapter3,
            ChapterType.Chapter4   => SceneType.Chapter4,
            ChapterType.Chapter5 => SceneType.Chapter501,
            _                      => throw new System.ArgumentOutOfRangeException(nameof(chapter), chapter, null)
        };
    }
}
