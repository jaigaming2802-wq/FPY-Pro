using UnityEngine;

public class HurtState : PlayerState
{
    private bool animationStarted;


    public HurtState(
        PlayerMovement player,
        PlayerStateMachine stateMachine)
        : base(player, stateMachine)
    {
    }


    public override void Enter()
    {
        // Stop movement animation speed
        // while Hurt animation is playing.
        player.SetAnimationSpeed(0f);


        animationStarted = false;


        // Prevent attack animations from playing
        // while the player is in Hurt State.
        player.anim.ResetTrigger("Attack1");
        player.anim.ResetTrigger("Attack2");


        // Play Hurt animation.
        player.anim.SetTrigger("Hurt");
    }


    public override void Update()
    {
        // Prevent attack animations while
        // the player is in Hurt State.
        player.anim.ResetTrigger("Attack1");
        player.anim.ResetTrigger("Attack2");


        AnimatorStateInfo state =
            player.anim.GetCurrentAnimatorStateInfo(0);


        // Check whether Hurt animation has started.
        if (state.IsName("Hurt"))
        {
            animationStarted = true;
        }


        // Wait until Hurt animation is completed.
        if (animationStarted &&
            state.IsName("Hurt") &&
            state.normalizedTime >= 1f)
        {
            // If player died, go to Death State.
            if (player.PlayerHealth.IsDead)
            {
                stateMachine.ChangeState(
                    new DeathState(
                        player,
                        stateMachine));
            }


            // If player is moving, return to Move State.
            else if (Mathf.Abs(player.MoveInput.x) > 0.01f)
            {
                stateMachine.ChangeState(
                    new MoveState(
                        player,
                        stateMachine));
            }


            // Otherwise return to Idle State.
            else
            {
                stateMachine.ChangeState(
                    new IdleState(
                        player,
                        stateMachine));
            }
        }
    }


    public override void FixedUpdate()
    {
        // Knockback is currently disabled.
        // Player movement is not affected by knockback.
    }


    public override void Exit()
    {
        // Nothing required when leaving Hurt State.
    }
}