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

    private bool isVentDoor = false; // 벤트 안으로 들어갔는지 여부

    List<GameObject> ventIn = new List<GameObject>();
    List<GameObject> ventOut = new List<GameObject>();

    private SkillBTN skillBTN;

    // Start is called before the first frame update
    void Start()
    {
        skillBTN = Managers.Instance.UIManager.Get<PlayerBtn>().skillPanel;

        skillBTN.OnInteractBtnClick += OnVentInteraction; // 상호작용 버튼 클릭 이벤트 등록
        VentIn();
        VentOut();
    }

    private void OnDestroy()
    {
        if (skillBTN != null)
        {
            skillBTN.OnInteractBtnClick -= OnVentInteraction; // 상호작용 버튼 클릭 이벤트 해제
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("나야!");
            isVentDoor = true; // 벤트 안으로 들어갔는지 여부
            skillBTN.ShowInteractionButton(true); // 상호작용 버튼 비활성화
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("나가요~");
            isVentDoor = false; // 벤트 안으로 들어갔는지 여부
            skillBTN.ShowInteractionButton(false); // 상호작용 버튼 비활성화
        }
    }

    private void SetActiveGroup(List<GameObject> objects, bool isActive)
    {
        foreach (GameObject obj in objects)
        {
            // Check if the object is not null before setting active
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
        if (isVentDoor)   // isVentDoor = true
        {
            SetActiveGroup(ventIn, true);
            SetActiveGroup(ventOut, false);
        }
        else
        {
            SetActiveGroup(ventIn, false);
            SetActiveGroup(ventOut, true);
        }
    }
}
