using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "Combat System/Create AttackData")]
public class AttackData : ScriptableObject
{
    [field: SerializeField] public string AnimName { get; private set; }
    // field를 앞에 넣어야 버그 가능성 낮다고 하는데 private set이 아니라 private따로 public 따로 만들면 되던데 땀땀
    [field: SerializeField] public AttackHitBox HitBoxToUse { get; private set; }
    [field: SerializeField] public float ImpactStartTime { get; private set; }
    [field: SerializeField] public float ImpactEndTime { get; private set; }
}

public enum AttackHitBox { LeftHand, RightHand, LeftFoot, RightFoot, Sword }