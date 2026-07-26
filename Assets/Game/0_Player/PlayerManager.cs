using UnityEngine;

public class PlayerManager : MonoBehaviour {
    public static PlayerManager instance;

    Transform playerTransform;
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
        playerTransform = transform;
    }

    public Transform GetPlayerTransform() { return playerTransform; }
    public void ResetJumpVelocity() { playerMovementController.ResetJumpVelocity(); }
    public void LockPlayerMovement() { playerMovementController.LockMovement(); }
    public void UnlockPlayerMovement() { playerMovementController.UnlockMovement(); }

}
