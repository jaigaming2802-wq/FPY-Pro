public abstract class PlayerState
{
    protected PlayerMovement player;
    protected PlayerStateMachine stateMachine;


    public PlayerState(PlayerMovement player, PlayerStateMachine stateMachine)
    {
        this.player = player;
        this.stateMachine = stateMachine;
    }


    public virtual void Enter()
    {

    }


    public virtual void Update()
    {

    }


    public virtual void FixedUpdate()
    {

    }


    public virtual void Exit()
    {

    }
}