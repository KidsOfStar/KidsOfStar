using UnityEngine;
[System.Serializable]
public class PlayerContextData
{
    public Player PlayerSc { get; private set; }
    public PlayerController Controller { get; private set; }
    public PlayerFormController FormController { get; private set; }
    public PlayerStateMachine StateMachine { get; private set; }
    public SpriteRenderer Renderer { get; private set; }
    public Rigidbody2D Rigid { get; private set; }
    public BoxCollider2D BoxCollider { get; private set; }
    
    public bool CanCling { get; set; }

    public PlayerContextData(Player player, PlayerController con, PlayerFormController formCon,
        PlayerStateMachine machine, SpriteRenderer sr, Rigidbody2D rb, BoxCollider2D box)
    {
        this.PlayerSc = player;
        this.Controller = con;
        this.FormController = formCon;
        this.StateMachine = machine;
        this.Renderer = sr;
        this.Rigid = rb;
        this.BoxCollider = box;
    }
}
