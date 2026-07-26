using Unity.Mathematics;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    public static PlayerManager instance;

    MovementController playerMovementController;

    void Awake() {
        if (instance == null) instance = this;
        playerMovementController = GetComponent<MovementController>();
        SetUpdates();
    }

    void Update() {
        SetUpdates();
    }

    void SetUpdates() {
    }

    public Transform GetPlayerTransform() { return transform; }
    public void ResetJumpVelocity() { playerMovementController.ResetJumpVelocity(); }
    public void StopPlayer() { playerMovementController.GetComponent<CharacterController>().Move(Vector3.zero); }

    public void LockPlayerMovement() { playerMovementController.LockMovement(); }
    public void UnlockPlayerMovement() { playerMovementController.UnlockMovement(); }

    public bool IsPlayerGrounded() { return playerMovementController.GetComponent<CharacterController>().isGrounded; }

    bool hitSomething = false;
    public bool MoveToPoint(Vector3 pointToHit, float time) {
        if (hitSomething) {
            hitSomething = false;
            return false;
        }

        Vector3 movePosition = (pointToHit - transform.position) * (time / 30);
        transform.position += movePosition;

        Vector3 roundedPlayerPosition = math.abs(transform.position);
        Vector3 roundedPoint = math.abs(pointToHit);
        return roundedPlayerPosition != roundedPoint;
    }

    void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player") && !other.CompareTag("Enemy")) {
            hitSomething = true;
        }
    }


}
