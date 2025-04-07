using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public class UIManager : Singleton<UIManager>
{

    [SerializeField] private List<Transform> parents;
    private Dictionary<string, UIBase> uiList = new Dictionary<string, UIBase>(); // UI ����Ʈ�� Dictionary�� �����Ͽ� �̸����� ���� �����ϰ� ��
    //private List<UIBase> uiList = new List<UIBase>();

    /// <summary>
    /// UI�� ������ �θ� ������Ʈ ����Ʈ�� ����
    /// ���� Canvas ������ Background, UI, Popup ���� ��ġ���� ����
    /// </summary>
    public static void SetParents(List<Transform> parents)
    {
        Instance.parents = parents;
        Instance.uiList.Clear();    // ���� UI ����Ʈ �ʱ�ȭ
    }

    /// <summary>
    /// UI�� ������. ������ �����ϰ�, ������ ���� UI�� ��Ȱ����.
    /// ������ ���ҽ����� �ε��Ͽ� ���� ����
    /// </summary>

    public static T Show<T>(params object[] param) where T : UIBase
    {
        // �̹� �����Ǿ� �ִ��� Ȯ��
        string uiName = typeof(T).ToString();
        var uiDictionary = Instance.uiList.TryGetValue(uiName, out var ui);

        // ������ Resource���� �ε��Ͽ� ����
        if(!uiDictionary)
        {
            // ResourceManager���� �ش� UI ������ �ε�
            var prefab = ResourceManager.Instance.LoadAsset<T>(uiName, ResourceType.UI);
            
            if(prefab == null) return null; // �������� ������ null ��ȯ


            ui = Instantiate(prefab, Instance.parents[(int)prefab.uiPosition]); // ������ ��ġ�� ����
            ui.name = uiName;
            Instance.uiList.Add(uiName, ui);
        }

        // ���� ��ġ�� UI ��Ȱ��ȭ
        if (ui.uiPosition == eUIPosition.UI)
        {
            foreach (var other in Instance.uiList.Values)
            {
                if (other != ui && other.uiPosition == eUIPosition.UI)
                {
                    other.SetActive(false);
                }
            }
        }

        ui.SetActive(true);
        ui.Opened(param);
        return (T)ui;
    }

    /// <summary>
    /// UI�� ����ų� �ı���
    /// </summary>
    public static void Hide<T>(params object[] param) where T : UIBase
    {
        string uiName = typeof(T).ToString();
        var uiDictionary = Instance.uiList.TryGetValue(uiName, out var ui);

        if (uiDictionary)
        {
            Instance.uiList.Remove(uiName);

            // ���� UI ����
            if (ui.uiPosition == eUIPosition.UI)
            {
                foreach(var kvp in Instance.uiList.Reverse())
                {
                    if(kvp.Value.uiPosition == eUIPosition.UI)
                    {
                        kvp.Value.gameObject.SetActive(true);
                        break;
                    }
                }
            }

            ui.closed?.Invoke(param);

            if (ui.uiOptions.isDestroyOnHide)
            {
                Destroy(ui.gameObject);
            }
            else
            {
                ui.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Ư�� UI�� ������ (������ null)
    /// </summary>
    public static T Get<T>() where T : UIBase
    {
        string uiName = typeof(T).Name;
        return Instance.uiList.TryGetValue(uiName, out var ui) ? (T)ui : null;
    }

    /// <summary>
    /// Ư�� UI�� �����ִ��� Ȯ��
    /// </summary>
    public static bool IsOpened<T>() where T : UIBase
    {
        string uiName = typeof(T).Name;
        return Instance.uiList.TryGetValue(uiName, out var ui) && ui.gameObject.activeInHierarchy;
    }
}