using UnityEngine;

public class IdleState : State<EnemyController>
{
    EnemyController enemy;
    public override void Enter(EnemyController owner)
    {
        enemy = owner;
    }

    public override void Execute()
    {
        foreach (var target in enemy.TargetsInRange)
        {
            var vecToTarget = target.transform.position - transform.position;
            float angle = Vector3.Angle(transform.forward, vecToTarget);
            // Angle은 가운데선에서 몇도 차이나는지 절대 각도만 반환하기 때문에 /2를 해줘야함

            if (angle <= enemy.Fov / 2)
            {
                enemy.Target = target;
                enemy.ChangeState(EnemyStates.CombatMovement);
                break;
            }
        }
    }

    public override void Exit()
    {

    }
}
