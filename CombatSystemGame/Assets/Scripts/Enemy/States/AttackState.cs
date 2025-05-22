using System.Collections;
using UnityEngine;

public class AttackState : State<EnemyController>
{
    [SerializeField] float attackDistance = 1f;
    bool isAttacking;
    EnemyController enemy;
    public override void Enter(EnemyController owner)
    {
        enemy = owner;

        enemy.NavMeshAgent.stoppingDistance = attackDistance;
    }

    public override void Execute()
    {
        if (isAttacking) { return; }

        enemy.NavMeshAgent.SetDestination(enemy.Target.transform.position);

        if (Vector3.Distance(enemy.Target.transform.position, enemy.transform.position) <= attackDistance + 0.03f)
        {
            StartCoroutine(Attack());
        }
    }

    public override void Exit()
    {
        enemy.NavMeshAgent.ResetPath();
    }

    IEnumerator Attack(int comboCount = 1)
    {
        isAttacking = true;
        enemy.Anim.applyRootMotion = true;

        for (int i = 0; i < comboCount; i++)
        {
        yield return new WaitUntil(() => enemy.Fighter.AttackStates == AttackStateTypes.Cooldown);
            enemy.Fighter.TryToAttack();
        }

        yield return new WaitUntil(() => enemy.Fighter.AttackStates == AttackStateTypes.Idle);

        enemy.Anim.applyRootMotion = false;
        isAttacking = false;

        enemy.ChangeState(EnemyStates.RetreatAfterAttack);
    }
}
