using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {
    [SerializeField] LayerMask playerMask;
    [Header("Speed Stats")]
    [SerializeField] float hoverDistance;
    [SerializeField, Range(0, 2)] float speed;
    float distance_from_player;

    [Header("Attack Cooldown")]
    [SerializeField] float cooldown;
    bool canAttack;


    BasicAttack attack;

    void Start() {
        canAttack = true;
    }

    void Update() {
        // slowly float towards player
        distance_from_player = math.distance(transform.position, PlayerManager.instance.GetPlayerTransform().position);

        if (distance_from_player > hoverDistance) Follow();
        else BackUp();
    }

    void Follow() {
        // raycast to player
        Vector3 directionToPlayer = (transform.position - PlayerManager.instance.GetPlayerTransform().position).normalized;
        RaycastHit hit;

        // TODO: use this to avoid obstacles
        Physics.Raycast(transform.position, directionToPlayer, out hit, math.INFINITY, playerMask);

        // point to player
        Vector3 endPoint = (PlayerManager.instance.GetPlayerTransform().position - transform.position) * (speed * Time.deltaTime);
        transform.position += endPoint;


    }

    void BackUp() {

    }

    void AvoidObstacles() {

    }

    void OnTriggerEnter(Collider other) {
        if (canAttack && other.CompareTag("Player")) {
            Health playerHealth;
            other.TryGetComponent(out playerHealth);
            if (!playerHealth) return;
            playerHealth.Hurt(1);
            StartCoroutine(AttackCooldown());
        }
    }

    IEnumerator AttackCooldown() {
        canAttack = false;
        yield return new WaitForSeconds(cooldown);
        canAttack = true;
    }

}
