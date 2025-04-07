using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ҽ� Ÿ���� �����ϴ� ������
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

// ���ҽ��� Load, Instantiate, Destroy �� �����ϴ� ���ҽ� �Ŵ���. 
public class ResourceManager : Singleton<ResourceManager>
{
    public Dictionary<string, UIBase> uiList = new Dictionary<string, UIBase>(); // UI ����Ʈ�� Dictionary�� �����Ͽ� �̸����� ���� �����ϰ� ��

    /// <summary>
    /// UI�� �ε��ϴ� �Լ�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T LoadUI<T>() where T : UIBase
    {
        string key = typeof(T).Name; // UI�� �̸��� Ű�� ���
        if (uiList.ContainsKey(key))
        {
            return uiList[key] as T; // �̹� ������ UI ��ȯ
        }

        // ��ųʸ��� Ű�� ���ٸ� ���� - ���� ���� �ÿ��� ���
        var ui = Resources.Load<UIBase>($"UI/{key}") as T;
        if (ui == null) return null;    // ���ҽ��� ������ null ��ȯ
        uiList.Add(ui.name, ui); // UI �̸� : Key, UIBase : Value
        return ui;
    }


    /// <summary>
    /// ���ҽ��� �ε��ϴ� �Լ�
    /// </summary>
    /// <param name="path">Resources ������ ��� (Ȯ���� ����)</param>
    public T LoadAsset<T>(string path, ResourceType type = ResourceType.Default) where T : Object
    {
        string fullPath = GetFullPath(path, type);

        T obj = Resources.Load<T>(fullPath);
        if (obj == null)
        {
            Debug.LogError($"ResourceManager: {fullPath}�� ã�� �� �����ϴ�.");
        }
        return obj;
    }

    /// <summary>
    /// ���ҽ��� Instantiate�ϴ� �Լ�
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
    /// ���� ������Ʈ ����
    /// </summary>
    /// <param name="go"></param>
    public void Destroy(GameObject go)
    {
        if (go == null) return;
        Object.Destroy(go);
    }

    /// <summary>
    /// ���ҽ� ��� ���� �����
    /// </summary>
    /// <param name="path"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    private string GetFullPath(string path, ResourceType type)
    {
        return type switch
        {
            ResourceType.UI => $"UI/{path}",    // UI �������� "Resources/UI/" ������ ��ġ
            ResourceType.Default => path, // �⺻ ���
            _ => path   // ������ ���ҽ��� "Resources/" ������ ��ġ
        };
    }

}
