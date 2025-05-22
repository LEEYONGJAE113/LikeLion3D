using UnityEngine;

public class CombatController : MonoBehaviour
{
    MeleeFighter meleeFighter;

    void Awake()
    {
        meleeFighter = GetComponent<MeleeFighter>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            meleeFighter.TryToAttack();
        }
    }
}
