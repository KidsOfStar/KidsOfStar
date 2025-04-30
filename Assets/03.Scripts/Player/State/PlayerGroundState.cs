
public class PlayerGroundState : PlayerStateBase
{
    public PlayerGroundState(PlayerContextData data, PlayerStateFactory factory) : base(data, factory)
    {
    }

    public override void OnExit()
    {
        // 한 번 땅에 내려가면 다시 벽 타기가 가능하도록
        context.CanCling = true;
    }
}