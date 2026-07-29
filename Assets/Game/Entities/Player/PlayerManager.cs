using UnityEngine;


public class PlayerManager : MonoBehaviour {
    public static PlayerManager instance;

    [SerializeField] MovementController playerMovementController;
    [SerializeField] CharacterController playerCharacterController;
    [SerializeField] CapsuleCollider capsuleCollider;

    void Awake() {
        if (instance == null) instance = this;
        capsuleCollider.radius = playerCharacterController.radius;
        capsuleCollider.height = playerCharacterController.height;
        capsuleCollider.center = playerCharacterController.center;
    }

    public Transform GetTransform() { return transform; }
    public CharacterController GetCharacterController() { return playerCharacterController; }
    public MovementController GetMovementController() { return playerMovementController; }

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
