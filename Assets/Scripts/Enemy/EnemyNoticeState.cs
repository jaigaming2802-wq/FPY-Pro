using UnityEngine;

public class EnemyNoticeState : EnemyState
{
    private float timer;

    // Inspector la later maathalaam
    private const float noticeTime = 0.3f;

    public EnemyNoticeState(Enemy enemy, EnemyStateMachine stateMachine)
        : base(enemy, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Enemy Notice Enter");

        timer = 0f;

        // Stop moving
        enemy.EnemyMovement.Stop();

        // Face player once
        enemy.EnemyMovement.FaceTarget(enemy.Player.position);

        // Idle / Alert
        enemy.SetAnimationSpeed(0f);
    }

    public override void Update()
    {
        // Player escaped before notice finished
        if (!enemy.IsPlayerInChaseRange())
        {
            stateMachine.ChangeState(
                new EnemyPatrolState(enemy, stateMachine));

            return;
        }

        // Keep facing player
        enemy.EnemyMovement.FaceTarget(enemy.Player.position);

        timer += Time.deltaTime;

        if (timer >= noticeTime)
        {
            stateMachine.ChangeState(
                new EnemyChaseState(enemy, stateMachine));
        }
    }

    public override void FixedUpdate()
    {

    }

    public override void Exit()
    {
        Debug.Log("Enemy Notice Exit");
    }
}