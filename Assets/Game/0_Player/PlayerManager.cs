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


}
