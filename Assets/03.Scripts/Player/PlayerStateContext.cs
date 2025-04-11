using UnityEngine;

public class PlayerStateContext
{
    public Player PlayerSc {  get; private set; }
    public PlayerController Controller { get; private set; }
    public PlayerFormController FormController { get; private set; }
    public SpriteRenderer Renderer { get; private set; }
    public Rigidbody2D Rigid {  get; private set; }
    public BoxCollider2D BoxCollider { get; private set; }
    
    public PlayerStateContext(Player player, PlayerController con, PlayerFormController formCon,
        SpriteRenderer sr, Rigidbody2D rb, BoxCollider2D box)
    {
        this.PlayerSc = player;
        this.Controller = con;
        this.FormController = formCon;
        this.Renderer = sr;
        this.Rigid = rb;
        this.BoxCollider = box;
    }
}