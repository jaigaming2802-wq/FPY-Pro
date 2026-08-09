using UnityEngine;

public class DeathState : PlayerState
{
    private bool animationStarted;


    public DeathState(
        PlayerMovement player,
        PlayerStateMachine stateMachine)
        : base(player, stateMachine)
    {
    }


    public override void Enter()
    {
        // Player has died, so stop normal movement.
        player.Stop();


        // Stop movement animation.
        player.SetAnimationSpeed(0f);


        animationStarted = false;


        // Play Death animation.
        player.anim.SetTrigger("Death");
    }


    public override void Update()
    {
        AnimatorStateInfo state =
            player.anim.GetCurrentAnimatorStateInfo(0);


        // Check whether Death animation has started.
        if (state.IsName("Death"))
        {
            animationStarted = true;
        }


        // Wait until Death animation is completed.
        if (animationStarted &&
            state.IsName("Death") &&
            state.normalizedTime >= 1f)
        {
            // Destroy player after Death animation.
            Object.Destroy(player.gameObject);
        }
    }


    public override void FixedUpdate()
    {
        // Keep player stopped while Death animation plays.
        player.Stop();
    }


    public override void Exit()
    {
        // Nothing required when leaving Death State.
    }
}