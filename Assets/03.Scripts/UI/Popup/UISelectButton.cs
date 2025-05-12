using Febucci.UI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISelectButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI selectionText;
    [SerializeField] private TypewriterByCharacter typewriter;
    
    public void DefaultInit(int index, Action<int> onClickAction, string text)
    {
        typewriter.useTypeWriter = false;
        selectionText.text = text;
        button.onClick.AddListener(() => onClickAction(index));
        gameObject.SetActive(true);
    }

    public void HighlightInit(int index, Action<int> onClickAction, string text)
    {
        typewriter.useTypeWriter = true;
        selectionText.text = text;
        button.enabled = false;
        button.onClick.AddListener(() => onClickAction(index));
        typewriter.onTextShowed.AddListener(() => button.enabled = true);
        
        gameObject.SetActive(true);
        typewriter.ShowText(text);
    }
    
    private void OnDisable()
    {
        button.onClick.RemoveAllListeners();
        typewriter.onTextShowed.RemoveAllListeners();
    }
}
