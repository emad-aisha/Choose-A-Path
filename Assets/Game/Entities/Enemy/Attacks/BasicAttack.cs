using System.Collections;
using UnityEngine;

public abstract class BasicAttack : MonoBehaviour {
    [Header("Basic Attack Stats")]
    [SerializeField] protected int damage;
    [SerializeField] protected float cooldown;
    [SerializeField] protected float windup;
    protected bool canAttack;


    void Start() {
        canAttack = true;
    }

    public abstract bool Attack();

    protected IEnumerator AttackCooldown() {
        canAttack = false;
        yield return new WaitForSeconds(windup);
        yield return new WaitForSeconds(cooldown);
        canAttack = true;
    }
}
