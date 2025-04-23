using UnityEngine;

public class InteractableHandler : MonoBehaviour
{
    public SkillBTN skillBTN;

    // Start is called before the first frame update
    void Start()
    {
        skillBTN = Managers.Instance.UIManager.Get<PlayerBtn>().skillPanel;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            skillBTN.ShowInteractionButton(true);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            skillBTN.ShowInteractionButton(false);
        }
    }
}
