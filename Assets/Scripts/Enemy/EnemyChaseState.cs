using UnityEngine;

public class EnemyChaseState : EnemyState
{
    public EnemyChaseState(
        Enemy enemy,
        EnemyStateMachine stateMachine)
        : base(enemy, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Enemy Chase Enter");

        enemy.EnemyMovement.SetChaseSpeed();

        enemy.SetAnimationSpeed(1f);
    }

    public override void Update()
    {
        // Player escaped Chase Range
        if (!enemy.IsPlayerInChaseRange())
        {
            stateMachine.ChangeState(
                new EnemyPatrolState(enemy, stateMachine));

            return;
        }

        // Player landed -> face player
        if (enemy.PlayerJump == null ||
            enemy.PlayerJump.IsGrounded)
        {
            enemy.EnemyMovement.FaceTarget(
                enemy.Player.position);
        }

        // Attack
        if (enemy.IsPlayerInAttackRange())
        {
            stateMachine.ChangeState(
                new EnemyAttackState(enemy, stateMachine));

            return;
        }
    }

    public override void FixedUpdate()
    {
        enemy.EnemyMovement.Move(
            enemy.Player.position);
    }

    public override void Exit()
    {
        Debug.Log("Enemy Chase Exit");

        enemy.EnemyMovement.Stop();

        enemy.SetAnimationSpeed(0f);
    }
}