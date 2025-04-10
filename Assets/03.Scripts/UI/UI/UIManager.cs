using System.Collections.Generic;
using UnityEngine;

public class UIManager : ISceneLifecycleHandler
{
    private List<Transform> parents;
    private Dictionary<string, UIBase> uiList = new Dictionary<string, UIBase>(); // UI 리스트를 Dictionary로 변경하여 이름으로 접근 가능하게 함


    /// <summary>
    /// UI를 생성할 부모 오브젝트 리스트를 설정
    /// 보통 Canvas 하위에 Background, UI, Popup 같은 위치들이 있음
    /// </summary>
    public void SetParents(List<Transform> parents)
    {
        this.parents = parents;
        uiList.Clear();    // 기존 UI 리스트 초기화
    }

    /// <summary>
    /// UI를 보여줌. 없으면 생성하고, 있으면 기존 UI를 재활용함.
    /// 없으면 리소스에서 로드하여 동적 생성
    /// </summary>

    public T Show<T>(params object[] param) where T : UIBase
    {
        // 이미 생성되어 있는지 확인
        string uiName = typeof(T).Name;

        var uiDictionary = uiList.TryGetValue(uiName, out var ui);

        // 없으면 Resource에서 로드하여 생성
        if(!uiDictionary)
        {
            // 기본 UI 프리팹 로드
            var prefab = Managers.Instance.ResourceManager.Load<T>(GetPath(uiName), false);

            if (prefab == null) return null; // 프리팹이 없으면 null 반환

            ui = Object.Instantiate(prefab, parents[(int)prefab.uiPosition]) as T; // 지정된 위치에 생성
            ui.name = uiName;
            uiList.Add(uiName, ui);
        }

        ui.SetActive(true);
        ui.Opened(param);
        return (T)ui;
    }

    private string GetPath(string name)
    {
        // 이미 생성되어 있는지 확인
        if (name.Contains("Canvas"))
        {
            return Define.UIPath + name;
        }
        else if (name.Contains("Popup"))
        {
            return Define.PopupPath + name ;
        }
        else if (name.Contains("Top"))
        {
            return Define.TopPath + name;
        }
        else
        {
            return Define.UIPath + name;
        }
    }

    /// <summary>
    /// UI를 숨기거나 파괴함
    /// </summary>
    public void Hide<T>(params object[] param) where T : UIBase
    {
        string uiName = typeof(T).Name;
        var uiDictionary = uiList.TryGetValue(uiName, out var ui);

        if (uiDictionary)
        {
            ui.closed?.Invoke(param);
            ui.gameObject.SetActive(false);

        }
    }

    /// <summary>
    /// 특정 UI를 가져옴 (없으면 null)
    /// </summary>
    public T Get<T>() where T : UIBase
    {
        string uiName = typeof(T).Name;
        return uiList.TryGetValue(uiName, out var ui) ? (T)ui : null;
    }

    /// <summary>
    /// 특정 UI가 열려있는지 확인
    /// </summary>
    public bool IsOpened<T>() where T : UIBase
    {
        string uiName = typeof(T).Name;
        return uiList.TryGetValue(uiName, out var ui) && ui.gameObject.activeInHierarchy;
    }

    public void OnSceneLoaded() // 씬 로드할 때마다
    {
        var canvasPrefab = Managers.Instance.ResourceManager.Load<Canvas>("UI/Canvas", true);

        if (canvasPrefab == null)
        {
            Debug.LogError("Canvas prefab not found at path: UI/Canvas");
            return;
        }

        // Canvas 인스턴스 생성
        var canvasInstance = Object.Instantiate(canvasPrefab);
        canvasInstance.name = "Canvas";

        List<Transform> parentList = new List<Transform>();

        // Canvas 하위에 UI, Popup, Top 위치 생성
        string[] childNames = { "UI", "Popup", "Top" };

        foreach (string childName in childNames)
        {
            var child = canvasInstance.transform.Find(childName);
            if (child != null)
            {
                parentList.Add(child);
            }
            else
            {
                Debug.LogWarning($"Canvas 하위에서 '{childName}' 오브젝트를 찾지 못했습니다.");
            }
        }

        // 생성된 parentList를 SetParents()에 전달하여 부모 목록을 설정
        SetParents(parentList);
    }

    public void OnSceneUnloaded()
    {
        
    }
}