using UnityEngine;

public class DeathState : PlayerState
{
    private bool animationStarted;

    public DeathState(PlayerMovement player,
                      PlayerStateMachine stateMachine)
        : base(player, stateMachine)
    {
    }

    public override void Enter()
    {
        player.Stop();
        player.SetAnimationSpeed(0f);

        animationStarted = false;

        player.anim.SetTrigger("Death");
    }

    public override void Update()
    {
        AnimatorStateInfo state =
            player.anim.GetCurrentAnimatorStateInfo(0);

        if (state.IsName("Death"))
        {
            animationStarted = true;
        }

        if (animationStarted &&
            state.IsName("Death") &&
            state.normalizedTime >= 1f)
        {
            Object.Destroy(player.gameObject);
        }
    }

    public override void FixedUpdate()
    {
        player.Stop();
    }
}