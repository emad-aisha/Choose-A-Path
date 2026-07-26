using System;
using Unity.Mathematics;
using UnityEngine;

// TODO: break up into an ability manager
public class PlayerManager : MonoBehaviour {
    public static PlayerManager instance;

    MovementController playerMovementController;

    void Awake() {
        if (instance == null) instance = this;
        playerMovementController = GetComponent<MovementController>();
    }

    void Update() {
    }

    public Transform GetPlayerTransform() { return transform; }
    public void ResetJumpVelocity() { playerMovementController.ResetJumpVelocity(); }
    public void StopPlayer() { playerMovementController.GetComponent<CharacterController>().Move(Vector3.zero); }

    public void LockPlayerMovement() {
        playerMovementController.LockMovement();
        FacingDirectionManager.instance.LockDirection();
    }
    public void UnlockPlayerMovement() {
        playerMovementController.UnlockMovement();
        FacingDirectionManager.instance.UnlockDirection();
    }

    public bool IsPlayerGrounded() { return playerMovementController.GetComponent<CharacterController>().isGrounded; }

    public void SetMaxJumps(int newJumps) { playerMovementController.SetMaxJumps(newJumps); }

    bool hitSomething = false;
    Vector3 lastRoundedPlayerPosition = Vector3.zero;
    public bool MoveToPoint(Vector3 pointToHit, float distance) {
        if (hitSomething) {
            hitSomething = false;
            return false;
        }
        Vector3 endPoint = (pointToHit - transform.position) * distance;

        transform.position += endPoint;
        Vector3 roundedPlayerPosition = new Vector3((float)Math.Round(transform.position.x, 2), (float)Math.Round(transform.position.y, 2), (float)Math.Round(transform.position.z, 2));
        Debug.Log(roundedPlayerPosition);
        Debug.Log(lastRoundedPlayerPosition);

        if (roundedPlayerPosition == lastRoundedPlayerPosition) {
            lastRoundedPlayerPosition = Vector3.zero;
            return false;
        }
        if (roundedPlayerPosition != lastRoundedPlayerPosition) {
            lastRoundedPlayerPosition = roundedPlayerPosition;
            return true;
        }
        return false;
    }

    void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player") && !other.CompareTag("Enemy")) {
            hitSomething = true;
        }
    }


}
