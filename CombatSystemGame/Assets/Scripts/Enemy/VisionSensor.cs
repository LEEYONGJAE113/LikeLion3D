using UnityEngine;

public class VisionSensor : MonoBehaviour
{
    [SerializeField] EnemyController enemy;
    void OnTriggerEnter(Collider other)
    {
        var fighter = other.GetComponent<MeleeFighter>(); // 레이어 설정해둬서 가능

        if (fighter != null)
        {
            enemy.TargetsInRange.Add(fighter);
            EnemyManager.instance.AddEnemyInRange(enemy);
        }
    }

    void OnTriggerExit(Collider other)
    {
        var fighter = other.GetComponent<MeleeFighter>(); // 레이어 설정해둬서 가능

        if (fighter != null)
        {
            enemy.TargetsInRange.Remove(fighter);
            EnemyManager.instance.RemoveEnemyInRange(enemy);
        }
    }
}
