using System.Collections;
using UnityEngine;

public class Dash : BasicAttack {
    [Header("Dash Stats")]
    [SerializeField] float speed;
    [SerializeField] float dashDuration;
    float currentSeed;

    [Header("Disabling")]
    [SerializeField] float disableTime;

    Vector3 originalPosition;
    Vector3 playerPosition;

    bool isDashing;

    void Start() {
        originalPosition = transform.position;
        canAttack = true;
        currentSeed = speed;
    }

    void Update() {
        if (!isDashing) { originalPosition = transform.position; return; }

        currentSeed += speed * Time.deltaTime;

        Vector3 endPoint = (playerPosition - originalPosition).normalized * (currentSeed * Time.deltaTime);
        transform.position += endPoint;
    }

    public override bool Attack() {
        if (!canAttack) return false;
        currentSeed = speed;
        playerPosition = PlayerManager.instance.GetTransform().position;

        StartCoroutine(DashCooldown());
        StartCoroutine(AttackCooldown());
        return true;
    }

    IEnumerator DashCooldown() {
        animationManager.SetAttacking(true);
        animationManager.SetWinding(true);
        yield return new WaitForSeconds(windup);
        animationManager.SetWinding(false);
        isDashing = true;
        yield return new WaitForSeconds(dashDuration);
        animationManager.SetAttacking(false);
        isDashing = false;
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            StartCoroutine(AbilityManager.instance.DisableAbilities(disableTime));
        }
    }

}
