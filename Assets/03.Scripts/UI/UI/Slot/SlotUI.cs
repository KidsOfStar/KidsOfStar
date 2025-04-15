using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour
{
    [Header("Slot UI")]
    public TextMeshProUGUI slotName;
    private Button slotButton;

    private int slotIndex;
    public void SetUp(int index, string saveName, System.Action onClicked)
    {
        slotName.text = $"Slot{index + 1} : {saveName}";

        slotButton = GetComponent<Button>();
        slotButton.onClick.RemoveAllListeners();
        slotButton.onClick.AddListener(() => onClicked?.Invoke());
    }

    internal void UpdateSlotname(int index, string saveName)
    {
        slotName.text = $"Slot{index + 1} : {saveName}";
    }
}
