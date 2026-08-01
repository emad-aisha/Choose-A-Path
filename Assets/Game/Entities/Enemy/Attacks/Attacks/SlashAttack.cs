using System.Collections;
using UnityEngine;

public class Slash : BasicAttack {
    [Header("Slash Stats")]
    [SerializeField] GameObject hurtBox;
    [SerializeField] float timeOnScreen;

    void Start() {
        canAttack = true;
        hurtBox.GetComponent<Hurtbox>().SetDestroyOnHit(false);
    }

    public override bool Attack() {
        if (!canAttack) return false;

        StartCoroutine(OnScreen());
        StartCoroutine(AttackCooldown());
        return true;
    }

    IEnumerator OnScreen() {
        yield return new WaitForSeconds(windup);
        hurtBox.SetActive(true);
        yield return new WaitForSeconds(timeOnScreen);
        animationManager.SetAttacking(false);
        hurtBox.SetActive(false);
    }
}
