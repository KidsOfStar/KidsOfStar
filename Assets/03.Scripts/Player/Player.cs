using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerController controller;
    public PlayerController Controller { get { return controller; } }

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
    }
}
