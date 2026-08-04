using UnityEngine;

public class EnemyPatrolState : EnemyState
{
    private Transform target;

    public EnemyPatrolState(Enemy enemy, EnemyStateMachine stateMachine)
        : base(enemy, stateMachine)
    {

    }

    public override void Enter()
    {
        Debug.Log("Enemy Patrol Enter");

        enemy.EnemyMovement.SetPatrolSpeed();

        UpdateTarget();

        enemy.SetAnimationSpeed(1f);
    }

    public override void Update()
    {
        // Player entered Chase Range
        if (enemy.IsPlayerInChaseRange())
        {
            stateMachine.ChangeState(
                new EnemyNoticeState(enemy, stateMachine));

            return;
        }

        float distance = Mathf.Abs(
            enemy.transform.position.x - target.position.x);

        if (distance <= enemy.reachDistance)
        {
            enemy.MoveToPointA = !enemy.MoveToPointA;

            UpdateTarget();
        }
    }

    private void UpdateTarget()
    {
        target = enemy.MoveToPointA
            ? enemy.pointA
            : enemy.pointB;
    }

    public override void FixedUpdate()
    {
        if (target != null)
        {
            enemy.EnemyMovement.Move(target.position);
        }
    }

    public override void Exit()
    {
        Debug.Log("Enemy Patrol Exit");

        enemy.EnemyMovement.Stop();

        enemy.SetAnimationSpeed(0f);
    }
}