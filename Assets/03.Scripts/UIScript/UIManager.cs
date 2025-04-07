using System.Collections.Generic;
using UnityEngine;


public class UIManager : Singleton<UIManager>
{

    [SerializeField] private List<Transform> parents;
    private List<UIBase> uiList = new List<UIBase>();

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
        var ui = Instance.uiList.Find(obj => obj.name == uiName);

        // ������ Resource���� �ε��Ͽ� ����
        if(ui == null)
        {
            // ResourceManager���� �ش� UI ������ �ε�
            var prefab = ResourceManager.Instance.LoadAsset<T>(typeof(T).ToString(), ResourceType.UI);
            
            ui = Instantiate(prefab, Instance.parents[(int)prefab.uiPosition]); // ������ ��ġ�� ����
            ui.name = ui.name.Replace("(Clone)", ""); // �̸� ����
            Instance.uiList.Add(ui);
        }
        if (ui.uiPosition == eUIPosition.UI)
        {
            Instance.uiList.ForEach(obj =>
            {
                if (obj.uiPosition == eUIPosition.UI) obj.gameObject.SetActive(false);
            });
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
        var ui = Instance.uiList.Find(obj => obj.name == uiName);

        if (ui != null)
        {
            Instance.uiList.Remove(ui);

            // ���� UI ����
            if (ui.uiPosition == eUIPosition.UI)
            {
                var prevUI = Instance.uiList.FindLast(obj => obj.uiPosition == eUIPosition.UI);
                prevUI.SetActive(true);
            }

            ui.closed?.Invoke(param);

            if (ui.uiOptions.isDestroyOnHide)
            {
                Destroy(ui.gameObject);
            }
            else
            {
                ui.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Ư�� UI�� ������ (������ null)
    /// </summary>
    public static T Get<T>() where T : UIBase
    {
        return (T)Instance.uiList.Find(obj => obj.name == typeof(T).ToString());
    }

    /// <summary>
    /// Ư�� UI�� �����ִ��� Ȯ��
    /// </summary>
    public static bool IsOpened<T>() where T : UIBase
    {
        var ui = Instance.uiList.Find(obj => obj.name == typeof(T).ToString());
        return ui != null && ui.gameObject.activeInHierarchy;
    }
}