using UnityEngine;
using UnityEngine.SceneManagement;
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
            ChapterType.Chapter01 => SceneType.Chapter1,
            ChapterType.Chapter02 => SceneType.Chapter2,
            ChapterType.Chapter03 => SceneType.Chapter3,
            _                     => throw new System.ArgumentOutOfRangeException(nameof(chapter), chapter, null)
        };
    }
}
