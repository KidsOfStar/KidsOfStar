using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class VentDoor : MonoBehaviour
{
    [Header("Vent In")]
    public GameObject ventBG;
    public GameObject ventMap;
    public GameObject ventBlockMap;
    public GameObject ventObject;

    [Header("Vent Out")]
    public GameObject timeMap;
    public GameObject Hide;

    private bool isVentInOut = false; // 벤트 안으로 들어갔는지 여부

    List<GameObject> ventIn = new List<GameObject>();
    List<GameObject> ventOut = new List<GameObject>();

    private SkillBTN skillBTN;

    // Start is called before the first frame update
    void Start()
    {
        skillBTN = Managers.Instance.UIManager.Get<PlayerBtn>().skillPanel;

        VentIn();
        VentOut();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            skillBTN.ShowInteractionButton(true); // 상호작용 버튼 비활성화
            skillBTN.OnInteractBtnClick += OnVentInteraction; // 상호작용 버튼 클릭 이벤트 등록
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            skillBTN.ShowInteractionButton(false); // 상호작용 버튼 비활성화
            skillBTN.OnInteractBtnClick -= OnVentInteraction; // 상호작용 버튼 클릭 이벤트 등록
        }
    }

    private void SetActiveGroup(List<GameObject> objects, bool isActive)
    {
        foreach (GameObject obj in objects)
        {
            // 오브젝트가 null이 아닐 경우에만 활성화/비활성화
            if (obj != null)
                obj.SetActive(isActive);
        }
    }

    private void VentIn()
    {
        ventIn.Add(ventBG);
        ventIn.Add(ventMap);
        ventIn.Add(ventBlockMap);
        ventIn.Add(ventObject);
    }

    private void VentOut()
    {
        ventOut.Add(timeMap);
        ventOut.Add(Hide);
    }

    private void OnVentInteraction()
    {
        if (!isVentInOut)  
        {
            Debug.Log($"{isVentInOut} - 벤트 안으로 들어감");
            SetActiveGroup(ventIn, true);
            SetActiveGroup(ventOut, false);
            isVentInOut = true; // 벤트 안으로 들어갔는지 여부
        }
        else
        {
            Debug.Log($"{isVentInOut} - 벤트 밖으로 나감");
            SetActiveGroup(ventIn, false);
            SetActiveGroup(ventOut, true);
            isVentInOut = false; // 벤트 안으로 들어갔는지 여부
        }
    }
}
