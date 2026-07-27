using System.Collections;
using UnityEngine;

public class Dash : BasicAttack {
    [Header("Dash Stats")]
    [SerializeField] float speed;
    [SerializeField] float dashCooldown;

    Vector3 originalPosition;
    Vector3 playerPosition;

    bool isDashing;

    void Start() {
        originalPosition = transform.position;
        canAttack = true;
    }

    void Update() {
        if (!isDashing) { originalPosition = transform.position; return; }

        speed += speed * Time.deltaTime;

        Vector3 endPoint = (playerPosition - originalPosition).normalized * (speed * Time.deltaTime);
        transform.position += endPoint;
    }

    public override void Attack() {
        if (!canAttack) return;
        Debug.Log("dash");
        playerPosition = PlayerManager.instance.GetPlayerTransform().position;

        StartCoroutine(DashCooldown());
        StartCoroutine(AttackCooldown());
    }

    IEnumerator DashCooldown() {
        isDashing = true;
        yield return new WaitForSeconds(dashCooldown);
        isDashing = false;
    }

}
