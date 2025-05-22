using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackStateTypes { Idle, Windup, Impact, Cooldown }

public class MeleeFighter : MonoBehaviour
{
    [SerializeField] List<AttackData> attackDatas;
    [SerializeField] GameObject sword;
    BoxCollider swordColl;
    SphereCollider leftHandColl, rightHandColl, leftFootColl, rightFootColl;

    Animator anim;
    public bool inAction { get; private set; } = false;

    public AttackStateTypes AttackStates;
    bool doCombo;
    int comboCount = 0;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        if (sword != null)
        {
            swordColl = sword.GetComponent<BoxCollider>();
            leftHandColl = anim.GetBoneTransform(HumanBodyBones.LeftHand).GetComponent<SphereCollider>();
            rightHandColl = anim.GetBoneTransform(HumanBodyBones.RightHand).GetComponent<SphereCollider>();
            leftFootColl = anim.GetBoneTransform(HumanBodyBones.LeftFoot).GetComponent<SphereCollider>();
            rightFootColl = anim.GetBoneTransform(HumanBodyBones.RightFoot).GetComponent<SphereCollider>();

            DisableAllHitBox();
        }
    }

    void DisableAllHitBox()
    {
        swordColl.enabled = false;

        leftHandColl.enabled = false;
        rightHandColl.enabled = false;
        leftFootColl.enabled = false;
        rightFootColl.enabled = false;
    }


    public void TryToAttack()
    {
        if (!inAction)
        {
            StartCoroutine(Attack());
        }
        else if (AttackStates == AttackStateTypes.Impact || AttackStates == AttackStateTypes.Cooldown)
        {
            doCombo = true;
        }
    }

    IEnumerator Attack()
    {
        inAction = true;

        AttackStates = AttackStateTypes.Windup;

        anim.CrossFade(attackDatas[comboCount].AnimName, 0.2f); // Play+Blend, Duration(%)을 줄수있음
        // anim.CrossFadeInFixedTime("Slash", 0.2f); // Play+Blend, Duration(Second)을 줄수있음

        yield return null;

        var animState = anim.GetNextAnimatorStateInfo(1);

        float timer = 0f;

        while (timer <= animState.length)
        {
            timer += Time.deltaTime;

            float normalizedTime = timer / animState.length;

            if (AttackStates == AttackStateTypes.Windup)
            {
                if (normalizedTime >= attackDatas[comboCount].ImpactStartTime)
                {
                    AttackStates = AttackStateTypes.Impact;
                    // swordColl.enabled = true;
                    EnableHitBox(attackDatas[comboCount]);
                }
            }
            else if (AttackStates == AttackStateTypes.Impact)
            {
                if (normalizedTime >= attackDatas[comboCount].ImpactEndTime)
                {
                    AttackStates = AttackStateTypes.Cooldown;
                    // swordColl.enabled = false;
                    DisableAllHitBox();
                }
            }
            else if (AttackStates == AttackStateTypes.Cooldown)
            {
                if (doCombo)
                {
                    doCombo = false;

                    comboCount = (comboCount + 1) % attackDatas.Count;
                    // ++comboCount;
                    // if (comboCount >= attackDatas.Count)
                    // {
                    //     comboCount = 0;
                    // }

                    StartCoroutine(Attack());
                    yield break; // 코루틴 여러개 돌기 방지
                }
            }

            yield return null;
        }

        // yield return new WaitForSeconds(animState.length); // 애니메이션 시간 체크해서 wait 이었는데 while문땜에 필요없어짐

        AttackStates = AttackStateTypes.Idle;
        comboCount = 0;
        inAction = false;
    }

    void EnableHitBox(AttackData aData)
    {
        switch (aData.HitBoxToUse)
        {
            case AttackHitBox.LeftHand:
                leftHandColl.enabled = true;
                break;
            case AttackHitBox.RightHand:
                rightHandColl.enabled = true;
                break;
            case AttackHitBox.LeftFoot:
                leftFootColl.enabled = true;
                break;
            case AttackHitBox.RightFoot:
                rightFootColl.enabled = true;
                break;
            case AttackHitBox.Sword:
                swordColl.enabled = true;
                break;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HitBox") && !inAction)
        {
            StartCoroutine(PlayHitReaction());
        }
    }

    IEnumerator PlayHitReaction()
    {
        inAction = true;
        anim.CrossFade("SwordImpact", 0.2f); // Play+Blend, Duration(%)을 줄수있음

        yield return null;

        var animState = anim.GetNextAnimatorStateInfo(1);

        yield return new WaitForSeconds(animState.length); // 애니메이션 시간 체크해서 wait

        inAction = false;
    }

}
