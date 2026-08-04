using UnityEngine;

public class MoveState : PlayerState
{
    public MoveState(PlayerMovement player,
                     PlayerStateMachine stateMachine)
        : base(player, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Move State Enter");
    }

    public override void Update()
    {
        if (player.DashPressed &&
            player.PlayerDash.CanDash)
        {
            stateMachine.ChangeState(
                new DashState(player, stateMachine));
            return;
        }

        if (player.JumpPressed &&
            player.PlayerJump.IsGrounded)
        {
            stateMachine.ChangeState(
                new JumpState(player, stateMachine));
            return;
        }

        if (!player.PlayerJump.IsGrounded)
        {
            stateMachine.ChangeState(
                new FallState(player, stateMachine));
            return;
        }

        if (Mathf.Abs(player.MoveInput.x) < 0.2f)
        {
            stateMachine.ChangeState(
                new IdleState(player, stateMachine));
            return;
        }

        if (player.Attack1Pressed)
        {
            stateMachine.ChangeState(
                new AttackState(player, stateMachine, false));
            return;
        }

        if (player.Attack2Pressed)
        {
            stateMachine.ChangeState(
                new AttackState(player, stateMachine, true));
            return;
        }

        if (player.BlockPressed)
        {
            stateMachine.ChangeState(
                new BlockState(player, stateMachine));
            return;
        }
    }

    public override void FixedUpdate()
    {
        if (player.PlayerDash.IsDashing)
            return;

        player.Move();

        player.SetAnimationSpeed(Mathf.Abs(player.MoveInput.x));
    }

    public override void Exit()
    {
    }
}