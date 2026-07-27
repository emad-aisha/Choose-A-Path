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
    float hoverSeed = 0;
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
            else if (!isHovering) StartHover();
        }
        else {
            if (!isHovering) StartHover();
        }

        if (isHovering) Hover();
        else Debug.Log("not hovering");
    }

    void TryAttack() {
        attack.Attack();
    }

    void Follow() {
        Vector3 endPoint = (PlayerManager.instance.GetPlayerTransform().position - transform.position) * (speed * Time.deltaTime);
        transform.position += endPoint;

        Vector2 roundedPoints = new Vector2((float)Math.Round(endPoint.x, 2), (float)Math.Round(endPoint.x, 2));
        roundedPoints = math.abs(roundedPoints);

        if (!isHovering && (roundedPoints.x < 0.02 || roundedPoints.y < 0.02)) StartHover();
        else if (isHovering && (roundedPoints.x > 0.05 || roundedPoints.y > 0.05)) isHovering = false;
    }

    void Hover() {
        float sin = math.sin(hoverSeed);
        float cos = math.cos(hoverSeed);
        hoverSeed += Time.deltaTime;

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

    void StartHover() {
        Debug.Log("hover");
        isHovering = true;
        centerpoint = transform.position;
    }


}
