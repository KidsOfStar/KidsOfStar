using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 리소스 타입을 정의하는 열거형
/// </summary>
public enum ResourceType
{
    Default,
    UI,
    Audio,
    Texture,
    Material,
    Shader,
    Animation,
    ScriptableObject,
}

// 리소스의 Load, Instantiate, Destroy 를 관리하는 리소스 매니저. 
public class ResourceManager : Singleton<ResourceManager>
{
    public Dictionary<string, UIBase> uiList = new Dictionary<string, UIBase>(); // UI 리스트를 Dictionary로 변경하여 이름으로 접근 가능하게 함

    /// <summary>
    /// UI를 로드하는 함수
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T LoadUI<T>() where T : UIBase
    {
        string key = typeof(T).Name; // UI의 이름을 키로 사용
        if (uiList.ContainsKey(key))
        {
            return uiList[key] as T; // 이미 생성된 UI 반환
        }

        // 딕셔너리에 키가 없다면 생성 - 최초 생성 시에만 사용
        var ui = Resources.Load<UIBase>($"UI/{key}") as T;
        if (ui == null) return null;    // 리소스가 없으면 null 반환
        uiList.Add(ui.name, ui); // UI 이름 : Key, UIBase : Value
        return ui;
    }


    /// <summary>
    /// 리소스를 로드하는 함수
    /// </summary>
    /// <param name="path">Resources 하위의 경로 (확장자 없이)</param>
    public T LoadAsset<T>(string path, ResourceType type = ResourceType.Default) where T : Object
    {
        string fullPath = GetFullPath(path, type);

        T obj = Resources.Load<T>(fullPath);
        if (obj == null)
        {
            Debug.LogError($"ResourceManager: {fullPath}를 찾을 수 없습니다.");
        }
        return obj;
    }

    /// <summary>
    /// 리소스를 Instantiate하는 함수
    /// </summary>
    /// <param name="obj"></param>
    public GameObject Instantiate(string path, Transform parent = null, 
        ResourceType type = ResourceType.Default)
    {
        GameObject prefab = LoadAsset<GameObject>(path, type);
        if(prefab == null)  return null;
        return Object.Instantiate(prefab, parent);
    }

    /// <summary>
    /// 게임 오브젝트 제거
    /// </summary>
    /// <param name="go"></param>
    public void Destroy(GameObject go)
    {
        if (go == null) return;
        Object.Destroy(go);
    }

    /// <summary>
    /// 리소스 경로 구성 도우미
    /// </summary>
    /// <param name="path"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    private string GetFullPath(string path, ResourceType type)
    {
        return type switch
        {
            ResourceType.UI => $"UI/{path}",    // UI 프리팹은 "Resources/UI/" 하위에 위치
            ResourceType.Default => path, // 기본 경로
            _ => path   // 나머지 리소스는 "Resources/" 하위에 위치
        };
    }

}
