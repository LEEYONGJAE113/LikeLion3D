using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyStates { Idle, CombatMovement, Attack, RetreatAfterAttack }
public class EnemyController : MonoBehaviour
{
    [field:SerializeField] public float Fov { get; private set; } = 180f; // 볼수있는 각도
    public List<MeleeFighter> TargetsInRange { get; private set; } = new List<MeleeFighter>();
    public MeleeFighter Target;
    public MeleeFighter Fighter;
    public float combatMovementTimer = 0f;
    public StateMachine<EnemyController> StateMachine { get; private set; }

    Vector3 prevPos;

    Dictionary<EnemyStates, State<EnemyController>> stateDict;
    public NavMeshAgent NavMeshAgent { get; private set; }
    public Animator Anim { get; private set; }
    

    void Start()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        Anim = GetComponent<Animator>();
        Fighter = GetComponent<MeleeFighter>();

        stateDict = new Dictionary<EnemyStates, State<EnemyController>>();
        stateDict[EnemyStates.Idle] = GetComponent<IdleState>();
        stateDict[EnemyStates.CombatMovement] = GetComponent<CombatMovementState>();
        stateDict[EnemyStates.Attack] = GetComponent<AttackState>();
        stateDict[EnemyStates.RetreatAfterAttack] = GetComponent<RetreatAfterAttackState>();

        StateMachine = new StateMachine<EnemyController>(this);
        StateMachine.ChangeState(stateDict[EnemyStates.Idle]);
    }

    public void ChangeState(EnemyStates targetState)
    {
        StateMachine.ChangeState(stateDict[targetState]);
    }

    public bool IsInState(EnemyStates state)
    {
        return StateMachine.CurrentState == stateDict[state];
    }

    void Update()
    {
        StateMachine.Execute();

        var deltaPos = Anim.applyRootMotion ? Vector3.zero : transform.position - prevPos;
        var velocity = deltaPos / Time.deltaTime;

        float forwardSpeed = Vector3.Dot(velocity, transform.forward);

        Anim.SetFloat("ForwardSpeed", forwardSpeed / NavMeshAgent.speed, 0.2f, Time.deltaTime);

        float angle = Vector3.SignedAngle(transform.forward, velocity, Vector3.up);
        float strafeSpeed = Mathf.Sin(angle * Mathf.Deg2Rad);
        Anim.SetFloat("StrafeSpeed", strafeSpeed, 0.2f, Time.deltaTime);

        prevPos = transform.position;
    }
}
