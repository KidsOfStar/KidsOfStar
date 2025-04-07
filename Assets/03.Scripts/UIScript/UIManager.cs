using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public class UIManager : Singleton<UIManager>
{

    [SerializeField] private List<Transform> parents;
    private Dictionary<string, UIBase> uiList = new Dictionary<string, UIBase>(); // UI 리스트를 Dictionary로 변경하여 이름으로 접근 가능하게 함
    //private List<UIBase> uiList = new List<UIBase>();

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
        var uiDictionary = Instance.uiList.TryGetValue(uiName, out var ui);

        // 없으면 Resource에서 로드하여 생성
        if(!uiDictionary)
        {
            // ResourceManager에서 해당 UI 프리팹 로드
            var prefab = ResourceManager.Instance.LoadAsset<T>(uiName, ResourceType.UI);
            
            if(prefab == null) return null; // 프리팹이 없으면 null 반환


            ui = Instantiate(prefab, Instance.parents[(int)prefab.uiPosition]); // 지정된 위치에 생성
            ui.name = uiName;
            Instance.uiList.Add(uiName, ui);
        }

        // 같은 위치의 UI 비활성화
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
    /// UI를 숨기거나 파괴함
    /// </summary>
    public static void Hide<T>(params object[] param) where T : UIBase
    {
        string uiName = typeof(T).ToString();
        var uiDictionary = Instance.uiList.TryGetValue(uiName, out var ui);

        if (uiDictionary)
        {
            Instance.uiList.Remove(uiName);

            // 이전 UI 복원
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
    /// 특정 UI를 가져옴 (없으면 null)
    /// </summary>
    public static T Get<T>() where T : UIBase
    {
        string uiName = typeof(T).Name;
        return Instance.uiList.TryGetValue(uiName, out var ui) ? (T)ui : null;
    }

    /// <summary>
    /// 특정 UI가 열려있는지 확인
    /// </summary>
    public static bool IsOpened<T>() where T : UIBase
    {
        string uiName = typeof(T).Name;
        return Instance.uiList.TryGetValue(uiName, out var ui) && ui.gameObject.activeInHierarchy;
    }
}