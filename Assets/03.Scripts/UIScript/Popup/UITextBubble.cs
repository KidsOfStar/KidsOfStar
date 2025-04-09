using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITextBubble : UIBase
{
    [SerializeField] private Image bubbleImage;
    [SerializeField] private RectTransform tailRectTr;
    [SerializeField] private TextMeshProUGUI dialogText;

    public void SetDialog(string dialog, Vector3 pos)
    {
        dialogText.text = dialog;
    }
}
