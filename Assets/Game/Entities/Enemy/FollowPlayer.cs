using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class FollowPlayer : MonoBehaviour {
    [Header("Bob Stats")]
    [SerializeField, Range(0, 0.1f)] float range;
    [SerializeField, Range(0, 0.1f)] float wait;
    float bobValue;

    [Header("Speed Stats")]
    [SerializeField] float hoverDistance;
    [SerializeField] float triggerRange;
    [SerializeField] float speed;
    float distance_from_player;

    [Header("Attack Cooldown")]
    [SerializeField] BasicAttack attack;
    [SerializeField] float cooldown;
    bool canAttack;
    bool isAttacking;

    NavMeshAgent agent;
    bool isUp;

    Vector2 originalPosition;

    Quaternion originalRotation;
    void Start() {
        canAttack = true;
        attack = GetComponent<BasicAttack>();
        agent = GetComponent<NavMeshAgent>();
        originalRotation = transform.rotation;

        agent.speed = speed;
        agent.stoppingDistance = hoverDistance;
        agent.updateRotation = false;
        isUp = false;
    }

    Coroutine hovering;

    void Update() {
        // slowly float towards player
        distance_from_player = math.distance(transform.position, PlayerManager.instance.GetTransform().position);

        if (distance_from_player < triggerRange) {
            if (canAttack && distance_from_player < hoverDistance) TryAttack();
            Follow();
        }

        if (hovering == null && !canAttack) hovering = StartCoroutine(Hover());
    }

    void LateUpdate() {
        if (isAttacking && attack is FireballAttack && ((FireballAttack)attack).CompareType(FireballAttack.Type.Boomerang)) {
            transform.position = originalPosition;
        }

        transform.rotation = originalRotation;
    }

    void TryAttack() {
        if (attack.Attack()) StartCoroutine(AttackCooldown());
    }

    void Follow() {
        agent.SetDestination(PlayerManager.instance.GetTransform().position);
    }

    IEnumerator Hover() {
        if (isUp) {
            bobValue += Time.deltaTime;
            if (bobValue > range) isUp = false;
        }
        else {
            bobValue -= Time.deltaTime;
            if (bobValue < -range) isUp = true;
        }

        NavMeshHit hit;
        if (!NavMesh.SamplePosition(transform.position + new Vector3(0, bobValue, 0), out hit, 0.1f, NavMesh.AllAreas)) yield return new WaitForSeconds(wait);

        transform.position += new Vector3(0, bobValue, 0);
        yield return new WaitForSeconds(wait);
        hovering = null;
    }

    // HURT 
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Health playerHealth;
            other.TryGetComponent(out playerHealth);
            if (!playerHealth) return;
            playerHealth.Hurt(1);
        }
    }


    // COOLDOWN
    IEnumerator AttackCooldown() {
        originalPosition = transform.position;
        isAttacking = true;
        canAttack = false;
        yield return new WaitForSeconds(cooldown);
        canAttack = true;
        isAttacking = false;
    }


}
