using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TreeWarningPopup : PopupBase
{
    [SerializeField] private TextMeshProUGUI messageText;

    public override void Opened(params object[] param)
    {
        base.Opened(param);

        messageText.text = "세심하게 만져야 한다. 다른 방법이 없을까?";
    }
}
