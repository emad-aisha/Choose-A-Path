using System.Collections;
using UnityEngine;

public abstract class BasicAttack : MonoBehaviour {
    [Header("Basic Attack Stats")]
    [SerializeField] protected int damage;
    [SerializeField] protected float cooldown;
    protected bool canAttack;


    void Start() {
        canAttack = true;
    }

    void Update() {
        // if withing range, dash to attack
    }

    public abstract void Attack();

    protected IEnumerator AttackCooldown() {
        canAttack = false;
        yield return new WaitForSeconds(cooldown);
        canAttack = true;
    }
}
