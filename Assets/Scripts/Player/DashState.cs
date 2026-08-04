using UnityEngine;

public class DashState : PlayerState
{
    public DashState(PlayerMovement player,
                     PlayerStateMachine stateMachine)
        : base(player, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Dash Enter");

        player.StartCoroutine(player.PlayerDash.Dash());

        // Animation later
        // player.PlayDashAnimation();
    }

    public override void Update()
    {
        // Dash complete aana state change pannum
        if (!player.PlayerDash.IsDashing)
        {
            if (player.PlayerJump.IsGrounded)
            {
                if (Mathf.Abs(player.MoveInput.x) > 0.01f)
                {
                    stateMachine.ChangeState(
                        new MoveState(player, stateMachine)
                    );
                }
                else
                {
                    stateMachine.ChangeState(
                        new IdleState(player, stateMachine)
                    );
                }
            }
            else
            {
                stateMachine.ChangeState(
                    new FallState(player, stateMachine)
                );
            }
        }
    }

    public override void FixedUpdate()
    {

    }

    public override void Exit()
    {
        Debug.Log("Dash Exit");
    }
}