using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISelectButton : PopupBase
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI selectionText;
    
    public void Init(int index, Action<int> onClickAction, string text)
    {
        selectionText.text = text;
        button.onClick.AddListener(() => onClickAction(index));
    }
    
    private void OnDisable()
    {
        button.onClick.RemoveAllListeners();
    }
}
