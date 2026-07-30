using System.Collections;
using UnityEngine;
// TODO: organize
public class PlayerManager : MonoBehaviour {
    public static PlayerManager instance;

    [SerializeField] MovementController playerMovementController;
    [SerializeField] CharacterController playerCharacterController;
    [SerializeField] CapsuleCollider capsuleCollider;

    Vector3 respawnPoint;

    void Awake() {
        if (instance == null) instance = this;
        capsuleCollider.radius = playerCharacterController.radius;
        capsuleCollider.height = playerCharacterController.height;
        capsuleCollider.center = playerCharacterController.center;

        respawnPoint = GameObject.FindGameObjectWithTag("Respawn").transform.position;
    }

    bool respawn = false;

    void LateUpdate() {
        Vector3 position = transform.position;
        if (respawn) position = respawnPoint;

        position.z = 0;
        transform.position = position;
    }

    public Vector3 GetRespawnPoint() { return respawnPoint; }
    public Transform GetTransform() { return transform; }
    public CharacterController GetCharacterController() { return playerCharacterController; }
    public MovementController GetMovementController() { return playerMovementController; }

    public void ResetJumpVelocity() { playerMovementController.ResetJumpVelocity(); }
    public void StopPlayer() { playerMovementController.GetComponent<CharacterController>().Move(Vector3.zero); }

    public void LockPlayerMovement() { playerMovementController.LockMovement(); }
    public void UnlockPlayerMovement() { playerMovementController.UnlockMovement(); }

    public bool IsPlayerGrounded() { return playerMovementController.GetComponent<CharacterController>().isGrounded; }

    public IEnumerator Respawn(float waitTime) {
        instance.LockPlayerMovement();
        yield return new WaitForSeconds(waitTime);

        respawn = true;
        yield return new WaitForSeconds(0.02f);
        respawn = false;
        UnlockPlayerMovement();
    }
}
