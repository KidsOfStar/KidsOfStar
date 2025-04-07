using System.Collections.Generic;
using UnityEngine;


public class UIManager : Singleton<UIManager>
{

    [SerializeField] private List<Transform> parents;
    private List<UIBase> uiList = new List<UIBase>();

    /// <summary>
    /// UI를 생성할 부모 오브젝트 리스트를 설정
    /// 보통 Canvas 하위에 Background, UI, Popup 같은 위치들이 있음
    /// </summary>
    public static void SetParents(List<Transform> parents)
    {
        Instance.parents = parents;
        Instance.uiList.Clear();    // 기존 UI 리스트 초기화
    }

    /// <summary>
    /// UI를 보여줌. 없으면 생성하고, 있으면 기존 UI를 재활용함.
    /// 없으면 리소스에서 로드하여 동적 생성
    /// </summary>

    public static T Show<T>(params object[] param) where T : UIBase
    {
        // 이미 생성되어 있는지 확인
        string uiName = typeof(T).ToString();
        var ui = Instance.uiList.Find(obj => obj.name == uiName);

        // 없으면 Resource에서 로드하여 생성
        if(ui == null)
        {
            // ResourceManager에서 해당 UI 프리팹 로드
            var prefab = ResourceManager.Instance.LoadAsset<T>(typeof(T).ToString(), ResourceType.UI);
            
            ui = Instantiate(prefab, Instance.parents[(int)prefab.uiPosition]); // 지정된 위치에 생성
            ui.name = ui.name.Replace("(Clone)", ""); // 이름 정리
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
    /// UI를 숨기거나 파괴함
    /// </summary>
    public static void Hide<T>(params object[] param) where T : UIBase
    {
        string uiName = typeof(T).ToString();
        var ui = Instance.uiList.Find(obj => obj.name == uiName);

        if (ui != null)
        {
            Instance.uiList.Remove(ui);

            // 이전 UI 복원
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
    /// 특정 UI를 가져옴 (없으면 null)
    /// </summary>
    public static T Get<T>() where T : UIBase
    {
        return (T)Instance.uiList.Find(obj => obj.name == typeof(T).ToString());
    }

    /// <summary>
    /// 특정 UI가 열려있는지 확인
    /// </summary>
    public static bool IsOpened<T>() where T : UIBase
    {
        var ui = Instance.uiList.Find(obj => obj.name == typeof(T).ToString());
        return ui != null && ui.gameObject.activeInHierarchy;
    }
}