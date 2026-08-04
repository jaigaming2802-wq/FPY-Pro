using UnityEngine;

public class BlockState : PlayerState
{
    private float parryWindow = 0.2f;

    private float timer;

    public BlockState(PlayerMovement player,
                      PlayerStateMachine stateMachine)
        : base(player, stateMachine)
    {
    }

    public override void Enter()
    {
        if (!player.PlayerJump.IsGrounded)
        {
            stateMachine.ChangeState(new FallState(player, stateMachine));
            return;
        }

        player.Stop();
        player.SetAnimationSpeed(0f);

        player.PlayerHealth.IsBlocking = true;
        player.PlayerHealth.IsParrying = true;

        player.anim.SetTrigger("Block");

        timer = parryWindow;
    }

    public override void Update()
    {
        // Parry Window
        if (timer > 0f)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                player.PlayerHealth.IsParrying = false;
            }
        }

        // Release Block
        if (!player.BlockHeld)
        {
            player.PlayerHealth.IsBlocking = false;
            player.PlayerHealth.IsParrying = false;

            if (Mathf.Abs(player.MoveInput.x) > 0.01f)
            {
                stateMachine.ChangeState(
                    new MoveState(player, stateMachine));
            }
            else
            {
                stateMachine.ChangeState(
                    new IdleState(player, stateMachine));
            }
        }
    }

    public override void FixedUpdate()
    {
        player.Stop();
    }

    public override void Exit()
    {
        player.PlayerHealth.IsBlocking = false;
        player.PlayerHealth.IsParrying = false;

        Debug.Log("Block Exit");
    }
}