using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class FollowPlayer : MonoBehaviour {
    [SerializeField] LayerMask playerMask;

    [Header("Speed Stats")]
    [SerializeField] float hoverDistance;
    [SerializeField] float triggerRange;
    [SerializeField] float speed;
    float distance_from_player;

    [Header("Attack Cooldown")]
    [SerializeField] BasicAttack attack;
    [SerializeField] float cooldown;
    bool canAttack;

    NavMeshAgent agent;


    void Start() {
        canAttack = true;
        agent = GetComponent<NavMeshAgent>();

        agent.speed = speed;
        agent.stoppingDistance = hoverDistance;
    }

    void Update() {
        // slowly float towards player
        distance_from_player = math.distance(transform.position, PlayerManager.instance.GetPlayerTransform().position);

        if (distance_from_player < triggerRange) {
            if (canAttack && distance_from_player < hoverDistance) TryAttack();
            Follow();
        }

    }

    void TryAttack() {
        attack.Attack();
    }

    void Follow() {
        agent.SetDestination(PlayerManager.instance.GetPlayerTransform().position);
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


}
