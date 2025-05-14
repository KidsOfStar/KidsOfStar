using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tallman : MonoBehaviour
{

    [SerializeField] private GameObject bubbleTextPrefab;   // 말풍선 프리팹
    private GameObject bubbleTextInstance;                  // 생성된 프리팹 인스턴스

    private Coroutine warningCoroutine;                     // 경고 코루틴


    private void OnInteraction()
    {
        if (warningCoroutine != null) return;

        var player = Managers.Instance.GameManager.Player;
        string currentForm = player?.FormControl?.CurFormData?.FormName?.ToLower();

        if (string.IsNullOrEmpty(currentForm)) return;

        // 동물 폼인 경우에만 경고 시작
        //if (animalForms.Contains(currentForm))
        //{
        //    warningCoroutine = StartCoroutine(ShowWarning());
        //    Debug.Log($"StartCoroutine(ShowWarning())");
        //}
    }

    private IEnumerator ShowWarning()
    {
        var player = Managers.Instance.GameManager.Player;

        if (bubbleTextPrefab == null) yield break;

        bubbleTextInstance = Instantiate(bubbleTextPrefab, player.transform);
        bubbleTextInstance.transform.localPosition = new Vector3(0, 2f, 0);

        var bubbleText = bubbleTextInstance.GetComponentInChildren<DoorPopup>();

        if (bubbleText != null)
        {
            bubbleText.SetText("정체를 들킬 것 같다.");
            Debug.Log("정체를 들킬 것 같다.");
        }
        yield return new WaitForSeconds(1f);

        if (bubbleText != null)
        {
            bubbleText.SetText("당장 피해야 한다.");
        }
        yield return new WaitForSeconds(1f);

        if (bubbleText != null)
        {
            bubbleText.SetText("위험하다.");
        }
        yield return new WaitForSeconds(1f);

        //TriggerCaughtEnding();
    }
}
