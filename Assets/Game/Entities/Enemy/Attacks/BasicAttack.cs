using System.Collections;
using UnityEngine;

public abstract class BasicAttack : MonoBehaviour {

    [Header("Basic Attack Stats")]
    [SerializeField] protected int damage;
    [SerializeField] protected float cooldown;
    [SerializeField] protected float windup;
    protected bool canAttack;

    protected EnemyAnimationManager animationManager;

    void Start() {
        canAttack = true;
    }

    public abstract bool Attack();
    public bool IsAttacking() {
        return !canAttack;
    }

    public void SetAnimationMangaer(EnemyAnimationManager manager) { animationManager = manager; }

    protected IEnumerator AttackCooldown() {
        canAttack = false;
        animationManager.SetAttacking(true);
        animationManager.SetWinding(true);
        yield return new WaitForSeconds(windup);
        animationManager.SetWinding(false);
        yield return new WaitForSeconds(cooldown);
        canAttack = true;
    }
}
