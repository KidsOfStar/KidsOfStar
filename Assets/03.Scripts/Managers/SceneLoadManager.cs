using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class SceneLoadManager : MonoBehaviour
{
    private const float fakeMinDuration = 2f;
    private const float fakeMaxDuration = 3f;
    
    private SceneType currentScene;
    
    public void LoadScene(SceneType loadScene)
    {
        StartCoroutine(LoadSceneCoroutine(loadScene));
    }
    
    // 씬을 로드하는 코루틴
    private IEnumerator LoadSceneCoroutine(SceneType loadScene)
    {
        // 로딩 씬을 먼저 로드
        yield return SceneManager.LoadSceneAsync(SceneType.Loading.GetName(), LoadSceneMode.Additive);
        
        // TODO: 로딩 씬에서 로딩바를 0%로 초기화
        // Managers.Instance.UIManager.OnChangeLoadingProgress?.Invoke(0);

        // 씬 로드 시작
        AsyncOperation operation = SceneManager.LoadSceneAsync(loadScene.GetName(), LoadSceneMode.Additive);
        if (operation == null)
        {
            Debug.LogError("SceneLoader.LoadSceneAsync: operation is null");
            yield break;
        }
        operation.allowSceneActivation = false;

        // 최소 로딩 시간을 보장하기 위해 가짜 로딩 시간을 설정
        float minDuration = Random.Range(fakeMinDuration, fakeMaxDuration);
        float fakeLoadTime = 0f;
        float progress = 0f;

        // 씬이 90% 로드될 때까지 로딩바를 채움
        while (progress < 0.9f)
        {
            fakeLoadTime += Time.deltaTime;
            var fakeLoadRatio = fakeLoadTime / minDuration;

            progress = Mathf.Min(operation.progress, fakeLoadRatio - 0.1f);
            
            // TODO: 로딩 씬에서 로딩바를 업데이트
            // Managers.Instance.UIManager.OnChangeLoadingProgress?.Invoke(loadRatio);

            yield return null;
        }

        // 로드 씬 활성화
        SceneManager.UnloadSceneAsync(currentScene.GetName());
        operation.allowSceneActivation = true;
        while (!operation.isDone) yield return null;

        
        // 실제 씬 전환 완료 이후 초기화 및 로딩 시간 병렬 대기
        float postLoadTime = 0f;
        float initProgress = progress;
        bool isInitComplete = false;

        StartCoroutine(InitCoroutine(loadScene, () => isInitComplete = true));

        while (!isInitComplete || postLoadTime < minDuration)
        {
            postLoadTime += Time.deltaTime;
            float timeRatio = Mathf.Clamp01(postLoadTime / minDuration);
            float finalProgress = Mathf.Lerp(initProgress, 1f, timeRatio);
            // TODO: 로딩 씬에서 로딩바를 업데이트
            // Managers.Instance.UIManager.OnChangeLoadingProgress?.Invoke(finalProgress);
            yield return null;
        }
        
        // TODO: 로딩 씬에서 로딩바를 100%로 업데이트
        // Managers.Instance.UIManager.OnChangeLoadingProgress?.Invoke(1f);
        yield return new WaitForSeconds(0.1f);

        SceneManager.UnloadSceneAsync(SceneType.Loading.GetName());
    }

    private IEnumerator InitCoroutine(SceneType loadScene, Action onComplete)
    {
        switch (loadScene)
        {
            case SceneType.Main:
                // TODO: 각 씬에서 초기화 작업을 수행하는 메서드 호출
                // MainSceneBase.Instance.OnMainSceneInitComplete += () => onComplete?.Invoke();
                break;
            default:
                onComplete?.Invoke();
                break;
        }

        yield return null;
    }
}