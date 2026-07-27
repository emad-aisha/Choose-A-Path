using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {
    [SerializeField] LayerMask playerMask;

    [Header("Hover Stats")]
    [SerializeField] float hoverDistance;
    [SerializeField] float hoverTime;
    [SerializeField] float radius;
    float time = 0;
    Vector3 centerpoint;

    [Header("Speed Stats")]
    [SerializeField] float triggerRange;
    [SerializeField, Range(0, 2)] float speed;
    float distance_from_player;

    [Header("Attack Cooldown")]
    [SerializeField] BasicAttack attack;
    [SerializeField] float attackRange;
    [SerializeField] float cooldown;
    bool canAttack;

    bool isHovering;

    void Start() {
        canAttack = true;
        isHovering = true;
        centerpoint = transform.position;
    }

    void Update() {
        // slowly float towards player
        distance_from_player = math.distance(transform.position, PlayerManager.instance.GetPlayerTransform().position);

        if (distance_from_player < triggerRange) {
            if (canAttack && distance_from_player < attackRange) TryAttack();
            if (distance_from_player > hoverDistance) Follow();
        }

        if (isHovering) Hover();
    }

    void TryAttack() {
        attack.Attack();
    }

    void Follow() {
        Vector3 endPoint = (PlayerManager.instance.GetPlayerTransform().position - transform.position) * (speed * Time.deltaTime);
        transform.position += endPoint;
        Debug.Log(endPoint);

        if (endPoint.x == 0 && endPoint.y == 0) StartCoroutine(HoverTimer());
        else isHovering = false;
    }

    void Hover() {
        float sin = math.sin(time);
        float cos = math.cos(time);
        time += Time.deltaTime;

        transform.position = centerpoint + new Vector3(cos * radius, sin * radius, 0);
    }


    // HURT 
    void OnTriggerEnter(Collider other) {
        if (canAttack && other.CompareTag("Player")) {
            Health playerHealth;
            other.TryGetComponent(out playerHealth);
            if (!playerHealth) return;
            playerHealth.Hurt(1);
            StartCoroutine(AttackCooldown());
        }
    }


    // COOLDOWN
    IEnumerator AttackCooldown() {
        canAttack = false;
        yield return new WaitForSeconds(cooldown);
        canAttack = true;
    }

    IEnumerator HoverTimer() {
        isHovering = true;
        centerpoint = transform.position;
        yield return new WaitForSeconds(hoverTime);
        isHovering = false;
    }


}
