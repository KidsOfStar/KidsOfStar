using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TreeWarningPopup : PopupBase
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private TextMeshProUGUI boxWarningText;
    public override void Opened(params object[] param)
    {
        base.Opened(param);

        messageText.text = "세심하게 만져야 한다. 다른 방법이 없을까?";
        boxWarningText.text = "중요한 상자를 두고왔다. 얼른 가서 다시 찾아오자";
    }
}