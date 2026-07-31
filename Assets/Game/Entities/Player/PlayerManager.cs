using System.Collections;
using UnityEngine;

// TODO: organize
public class PlayerManager : MonoBehaviour {
    public static PlayerManager instance;

    [SerializeField] MovementController playerMovementController;
    [SerializeField] SpriteRenderer playerSprite;
    [SerializeField] Health health;

    [Header("Capsules")]
    [SerializeField] CharacterController playerCharacterController;
    [SerializeField] CapsuleCollider capsuleCollider;
    [SerializeField] CapsuleCollider healthCollider;
    float radius;
    float height;
    Vector3 center;

    Vector3 respawnPoint;

    void Awake() {
        if (instance == null) instance = this;
        radius = playerCharacterController.radius;
        height = playerCharacterController.height;
        center = playerCharacterController.center;

        capsuleCollider.radius = radius;
        capsuleCollider.height = height;
        capsuleCollider.center = center;

        healthCollider.radius = radius;
        healthCollider.height = height;
        healthCollider.center = center;

        respawnPoint = GameObject.FindGameObjectWithTag("Respawn").transform.position;
    }

    bool respawn = false;

    void LateUpdate() {
        Vector3 position = transform.position;
        if (respawn) position = respawnPoint;

        position.z = 0;
        transform.position = position;
    }

    public void SetRespawnPoint(Vector3 newposition) { respawnPoint = newposition; }
    public Vector3 GetRespawnPoint() { return respawnPoint; }
    public Transform GetTransform() { return transform; }
    public CharacterController GetCharacterController() { return playerCharacterController; }
    public MovementController GetMovementController() { return playerMovementController; }

    public void SetSpriteRendererColor(Color color) { playerSprite.color = color; }
    public void ResetSpriteRendererColor() { playerSprite.color = Color.white; }

    public void UpdateCharacterCapsule(float height, Vector3 center) {
        playerCharacterController.height = height;
        playerCharacterController.center = center;

        capsuleCollider.height = height;
        capsuleCollider.center = center;

        healthCollider.height = height;
        healthCollider.center = center;
    }
    public void ResetCharacterCapsule() {
        playerCharacterController.height = height;
        playerCharacterController.center = center;

        capsuleCollider.height = height;
        capsuleCollider.center = center;

        healthCollider.height = height;
        healthCollider.center = center;
    }



    public void ResetJumpVelocity() { playerMovementController.ResetJumpVelocity(); }
    public void StopPlayer() { playerMovementController.GetComponent<CharacterController>().Move(Vector3.zero); }

    public void LockPlayerMovement() { playerMovementController.LockMovement(); }
    public void UnlockPlayerMovement() { playerMovementController.UnlockMovement(); }

    public bool IsPlayerGrounded() { return playerMovementController.GetComponent<CharacterController>().isGrounded; }

    public IEnumerator Respawn(float waitTime) {
        LockPlayerMovement();
        yield return new WaitForSeconds(waitTime);

        respawn = true;
        yield return new WaitForSeconds(0.02f);
        StartCoroutine(health.IFrameTime(true));
        respawn = false;
        UnlockPlayerMovement();
    }
}
