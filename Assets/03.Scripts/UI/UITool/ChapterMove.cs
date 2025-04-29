using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChapterMove : MonoBehaviour
{
    [Header("Chapter Settings")]
    public Button btnChapterMove;
    public SceneType targetChapter; // 이동할 챕터 타입

    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // 버튼 유지
    }

    private void Start()
    {
        //btnChapterMove.onClick.AddListener(OnClickChapterMove);
    }

    public void OnClickChapterMove()
    {
        SceneType sceneName = targetChapter;

        Managers.Instance.SceneLoadManager.LoadScene(sceneName);

    }

   
}
