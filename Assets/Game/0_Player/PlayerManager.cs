using System;
using UnityEngine;

// TODO: break up into an ability manager
// TODO: please
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
    Vector3 originalPosition = Vector3.zero;

    public void SetStartPoint() {
        originalPosition = transform.position;
    }

    public bool MoveToPoint(Vector3 pointToHit, float distance) {
        if (hitSomething) {
            hitSomething = false;
            return false;
        }
        Vector3 endPoint = (pointToHit - originalPosition) * distance;

        transform.position += endPoint;

        // TODO: cleanup
        // if past point
        float radius = playerMovementController.GetComponent<CharacterController>().radius;
        if (endPoint.normalized.x != 0) {
            if (pointToHit.x - transform.position.x < 0 && transform.position.x < pointToHit.x - radius) {
                Debug.Log("X - too far to left");
                originalPosition = Vector3.zero;
                return false;
            }
            else if (pointToHit.x - transform.position.x > 0 && transform.position.x > pointToHit.x - radius) {
                Debug.Log("X - too far to right");
                originalPosition = Vector3.zero;
                return false;
            }
        }
        else if (endPoint.normalized.y != 0) {
            if (pointToHit.y - transform.position.y < 0 && transform.position.y < pointToHit.y) {
                Debug.Log("Y - too far to down");
                originalPosition = Vector3.zero;
                return false;
            }
            else if (pointToHit.y - transform.position.y > 0 && transform.position.y > pointToHit.y) {
                Debug.Log("Y - too far to up");
                originalPosition = Vector3.zero;
                return false;
            }
        }

        return true;
    }

    void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player") && !other.CompareTag("Enemy")) {
            hitSomething = true;
        }
    }


}
