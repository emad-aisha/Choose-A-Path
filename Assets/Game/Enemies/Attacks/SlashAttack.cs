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

    public override void Attack() {
        if (!canAttack) return;

        StartCoroutine(OnScreen());
        StartCoroutine(AttackCooldown());
    }

    IEnumerator OnScreen() {
        yield return new WaitForSeconds(windup);
        hurtBox.SetActive(true);
        yield return new WaitForSeconds(timeOnScreen);
        hurtBox.SetActive(false);
    }
}
