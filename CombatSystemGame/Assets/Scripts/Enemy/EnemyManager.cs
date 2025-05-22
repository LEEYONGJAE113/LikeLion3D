using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] Vector2 timeRangeBetweenAttack = new Vector2(1, 4);
    public static EnemyManager instance { get; private set; }
    List<EnemyController> enemiesInRange = new List<EnemyController>();
    float notAttackingTimer = 2;

    void Awake()
    {
        instance = this;
    }

    public void AddEnemyInRange(EnemyController enemy)
    {
        if (!enemiesInRange.Contains(enemy))
        {
            enemiesInRange.Add(enemy);
        }
    }

    public void RemoveEnemyInRange(EnemyController enemy)
    {
        enemiesInRange.Remove(enemy);
    }

    void Update()
    {
        if(enemiesInRange.Count == 0) { return; }

        if (!enemiesInRange.Any(e => e.IsInState(EnemyStates.Attack)))
        {
            if (notAttackingTimer > 0)
            {
                notAttackingTimer -= Time.deltaTime;
            }
            if (notAttackingTimer <= 0)
            {
                // 공격
                var attackingEnemy = SelectEnemyForAttack();
                attackingEnemy.ChangeState(EnemyStates.Attack);
                notAttackingTimer = Random.Range(timeRangeBetweenAttack.x, timeRangeBetweenAttack.y);
            }
        }
    }

    EnemyController SelectEnemyForAttack()
    {
        return enemiesInRange
        .OrderByDescending(e => e.combatMovementTimer)
        .FirstOrDefault();
    }
}
