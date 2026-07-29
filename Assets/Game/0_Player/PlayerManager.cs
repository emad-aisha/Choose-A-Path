using UnityEngine;

// TODO: break up into an ability manager
// TODO: please
public class PlayerManager : MonoBehaviour {
    public static PlayerManager instance;

    [SerializeField] MovementController playerMovementController;
    [SerializeField] CharacterController playerCharacterController;

    void Awake() {
        if (instance == null) instance = this;
    }

    public Transform GetTransform() { return transform; }
    public CharacterController GetCharacterController() { return playerCharacterController; }

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
