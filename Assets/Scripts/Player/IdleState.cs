using UnityEngine;

public class IdleState : PlayerState
{
    public IdleState(PlayerMovement player,
                     PlayerStateMachine stateMachine)
        : base(player, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Idle State Enter");

        player.SetAnimationSpeed(0f);
        player.Stop();
    }

    public override void Update()
    {
        // Dash
        if (player.DashPressed &&
            player.PlayerDash.CanDash)
        {
            stateMachine.ChangeState(
                new DashState(player, stateMachine));
            return;
        }

        // Move
        if (Mathf.Abs(player.MoveInput.x) > 0.2f)
        {
            stateMachine.ChangeState(
                new MoveState(player, stateMachine));
            return;
        }

        // Walk off a platform -> Fall
        if (!player.PlayerJump.IsGrounded)
        {
            stateMachine.ChangeState(
                new FallState(player, stateMachine));
            return;
        }

        // Jump
        if (player.JumpPressed &&
            player.PlayerJump.IsGrounded)
        {
            stateMachine.ChangeState(
                new JumpState(player, stateMachine));
            return;
        }

        // Light Attack
        if (player.Attack1Pressed)
        {
            stateMachine.ChangeState(
                new AttackState(player, stateMachine, false));
            return;
        }

        // Heavy Attack
        if (player.Attack2Pressed)
        {
            stateMachine.ChangeState(
                new AttackState(player, stateMachine, true));
            return;
        }

        // Block
        if (player.BlockPressed)
        {
            stateMachine.ChangeState(
                new BlockState(player, stateMachine));
            return;
        }
    }

    public override void Exit()
    {
        Debug.Log("Idle State Exit");
    }
}