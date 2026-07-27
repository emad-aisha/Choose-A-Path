using System.Collections;
using UnityEngine;

public class Dash : BasicAttack {
    [Header("Dash Stats")]
    [SerializeField] float speed;
    [SerializeField] float dashDuration;
    float currentSeed;

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

    public override void Attack() {
        if (!canAttack) return;
        Debug.Log("dash");
        currentSeed = speed;
        playerPosition = PlayerManager.instance.GetPlayerTransform().position;

        StartCoroutine(DashCooldown());
        StartCoroutine(AttackCooldown());
    }

    IEnumerator DashCooldown() {
        yield return new WaitForSeconds(windup);
        isDashing = true;
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
    }

}
